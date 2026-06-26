using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiliu.Oral.Sdk.Abstractions.Services;

namespace Shiliu.Oral.Sdk.Infrastructure.Speech
{
    /// <summary>
    /// Baidu speech recognition via WebSocket.
    /// Adapted from the original BaiduSpeechClient.
    /// </summary>
    public class BaiduSpeechRecognizer : ISpeechRecognizer
    {
        private const string DefaultWebSocketUri = "wss://aip.baidubce.com/ws/realtime_speech_trans";

        private readonly ILogger<BaiduSpeechRecognizer> _logger;
        private readonly string _webSocketUri;
        private readonly string _appId;
        private readonly string _appKey;
        private readonly int _sampleRate;

        private ClientWebSocket _webSocket;
        private readonly ConcurrentQueue<byte[]> _audioQueue = new ConcurrentQueue<byte[]>();
        private CancellationTokenSource _sendCts;

        public event Action<string> OnTextMessage;
        public event Action<MemoryStream> OnBinaryMessage;
        public event Action OnEndMessage;

        public BaiduSpeechRecognizer(
            ILogger<BaiduSpeechRecognizer> logger,
            string appId = "116450031",
            string appKey = "X3YmhBMw5aUcmppkJZocbemp",
            int sampleRate = 16000,
            string webSocketUri = null)
        {
            _logger = logger;
            _appId = appId;
            _appKey = appKey;
            _sampleRate = sampleRate;
            _webSocketUri = webSocketUri ?? DefaultWebSocketUri;
        }

        public async Task ConnectAsync(CancellationToken ct = default)
        {
            _webSocket = new ClientWebSocket();
            await _webSocket.ConnectAsync(new Uri(_webSocketUri), ct);
            _logger?.LogDebug("BaiduSpeech WebSocket connected");

            _ = ReceiveMessagesAsync();
        }

        public async Task SendStartMessageAsync(string fromLanguage, string toLanguage, CancellationToken ct = default)
        {
            if (_webSocket?.State != WebSocketState.Open) return;

            var startMessage = new
            {
                type = "START",
                from = fromLanguage,
                to = toLanguage,
                app_id = _appId,
                app_key = _appKey,
                sampling_rate = _sampleRate
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(startMessage);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct);
        }

        public void EnqueueAudioData(byte[] audioData)
        {
            _audioQueue.Enqueue(audioData);
        }

        public async Task SendAudioStreamAsync(CancellationToken ct = default)
        {
            _sendCts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            while (_webSocket?.State == WebSocketState.Open && !_sendCts.Token.IsCancellationRequested)
            {
                if (_audioQueue.TryDequeue(out var audioData))
                {
                    await _webSocket.SendAsync(
                        new ArraySegment<byte>(audioData),
                        WebSocketMessageType.Binary,
                        true,
                        _sendCts.Token);
                }
                await Task.Delay(40, _sendCts.Token);
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[1024 * 4];

            try
            {
                while (_webSocket?.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result;
                    MemoryStream textStream = null;
                    MemoryStream audioStream = null;

                    try
                    {
                        do
                        {
                            result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                if (textStream == null) textStream = new MemoryStream();
                                textStream.Write(buffer, 0, result.Count);
                            }
                            else if (result.MessageType == WebSocketMessageType.Binary)
                            {
                                if (audioStream == null) audioStream = new MemoryStream();
                                await audioStream.WriteAsync(buffer, 0, result.Count);
                            }
                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var textMessage = Encoding.UTF8.GetString(textStream.ToArray());
                            OnTextMessage?.Invoke(textMessage);
                        }
                        else if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            OnBinaryMessage?.Invoke(audioStream);
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            _logger?.LogDebug("BaiduSpeech WebSocket closed, state: {State}", _webSocket?.State);
                            OnEndMessage?.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "BaiduSpeech receive error");
                    }
                    finally
                    {
                        textStream?.Dispose();
                        audioStream?.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "BaiduSpeech receive loop failed");
            }

            _logger?.LogDebug("BaiduSpeech ReceiveMessagesAsync ended");
        }

        public async Task DisconnectAsync()
        {
            _sendCts?.Cancel();
            if (_webSocket?.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
            }
        }

        public void Dispose()
        {
            _sendCts?.Cancel();
            _sendCts?.Dispose();
            _webSocket?.Dispose();
            _webSocket = null;
        }
    }
}

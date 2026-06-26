using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Shiliu.Oral.Sdk.Infrastructure.WebSocket
{
    /// <summary>
    /// Generic WebSocket channel with auto-reconnect.
    /// Adapted from the original WebSocketClient.
    /// </summary>
    public class WebSocketChannel : IDisposable
    {
        private readonly ILogger<WebSocketChannel> _logger;
        private Uri _uri;
        private ClientWebSocket _clientWebSocket;

        public event Action<string> OnTextMessage;
        public event Action<MemoryStream> OnBinaryMessage;
        public event Action OnEndMessage;

        public WebSocketChannel(ILogger<WebSocketChannel> logger)
        {
            _logger = logger;
        }

        public async Task ConnectAsync(Uri uri, CancellationToken ct = default)
        {
            _uri = uri;
            _clientWebSocket = new ClientWebSocket();
            _clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(30);

            await _clientWebSocket.ConnectAsync(uri, ct);
            _logger?.LogDebug("WebSocket connected to {Uri}", uri);

            _ = ReceiveMessagesAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            if (_clientWebSocket?.State == WebSocketState.Open)
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendAudioDataAsync(byte[] audioData)
        {
            if (_clientWebSocket?.State == WebSocketState.Open)
            {
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(audioData), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        public async Task SendAudioFileAsync(string filePath)
        {
            if (_clientWebSocket?.State != WebSocketState.Open)
            {
                _logger?.LogDebug("WebSocket not connected, reconnecting...");
                await ReconnectAsync();
            }

            var audioData = File.ReadAllBytes(filePath);
            if (_clientWebSocket?.State == WebSocketState.Open)
            {
                _logger?.LogDebug("Sending audio file: {Path}", filePath);
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(audioData), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        public async Task ReconnectAsync()
        {
            _clientWebSocket?.Dispose();
            _clientWebSocket = new ClientWebSocket();

            await _clientWebSocket.ConnectAsync(_uri, CancellationToken.None);
            _logger?.LogDebug("WebSocket reconnected to {Uri}", _uri);

            _ = ReceiveMessagesAsync();
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[1024 * 4];

            try
            {
                while (_clientWebSocket?.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result;
                    MemoryStream textStream = null;
                    MemoryStream audioStream = null;

                    try
                    {
                        do
                        {
                            result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
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
                            _logger?.LogDebug("Binary message received: {Size} bytes", audioStream.Length);
                            OnBinaryMessage?.Invoke(audioStream);
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            _logger?.LogDebug("WebSocket closed, state: {State}", _clientWebSocket?.State);
                            OnEndMessage?.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "WebSocket receive error");
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
                _logger?.LogError(ex, "WebSocket receive loop failed");
            }

            _logger?.LogDebug("WebSocket ReceiveMessagesAsync ended");
        }

        public async Task DisconnectAsync()
        {
            if (_clientWebSocket?.State == WebSocketState.Open)
            {
                await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                _clientWebSocket.Dispose();
                _clientWebSocket = null;
            }
        }

        public void Dispose()
        {
            _clientWebSocket?.Dispose();
            _clientWebSocket = null;
        }
    }
}

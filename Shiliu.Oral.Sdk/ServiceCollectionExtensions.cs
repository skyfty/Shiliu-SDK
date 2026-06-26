using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shiliu.Oral.Sdk.Abstractions.Auth;
using Shiliu.Oral.Sdk.Abstractions.Services;
using Shiliu.Oral.Sdk.Infrastructure.Audio;
using Shiliu.Oral.Sdk.Infrastructure.Http;
using Shiliu.Oral.Sdk.Infrastructure.Services;
using Shiliu.Oral.Sdk.Infrastructure.Speech;
using Shiliu.Oral.Sdk.Infrastructure.WebSocket;

namespace Shiliu.Oral.Sdk
{
    /// <summary>
    /// Extension methods for registering Shiliu Oral SDK services in the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all Shiliu Oral SDK services using the provided options.
        /// Callers must also register an ITokenProvider implementation.
        /// </summary>
        public static IServiceCollection AddShiliuOralSdk(
            this IServiceCollection services,
            ShiliuOralOptions options)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (options == null) throw new ArgumentNullException(nameof(options));

            // Configure named HTTP clients
            services.AddHttpClient("AiTalkClient", client =>
            {
                client.BaseAddress = new Uri(options.AiTalkBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddHttpMessageHandler(sp => new AuthHeaderHandler(
                sp.GetRequiredService<ITokenProvider>(),
                options.Source));

            services.AddHttpClient("SoeClient", client =>
            {
                client.BaseAddress = new Uri(options.SoeBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Register core services
            services.AddSingleton<IAiTalkService, AiTalkService>();

            services.AddSingleton<ISpeechEvaluationService>(sp => new SpeechEvaluationService(
                sp.GetRequiredService<IHttpClientFactory>(),
                sp.GetRequiredService<ILogger<SpeechEvaluationService>>(),
                options.SoeAppKey,
                options.SoeAppSecret));

            services.AddSingleton<ISpeechRecognizer>(sp => new BaiduSpeechRecognizer(
                sp.GetRequiredService<ILogger<BaiduSpeechRecognizer>>(),
                options.BaiduAppId,
                options.BaiduAppKey,
                webSocketUri: options.BaiduAsrWebSocketUri));

            services.AddSingleton<ITranslationService, TranslationService>();
            // Note: TranslationService uses "AiTalkClient" named HttpClient (same base URL, same auth).

            services.AddSingleton<IAudioCaptureService, NAudioCaptureService>();
            services.AddSingleton<IAudioPlayerService, WavePlayerService>();

            services.AddSingleton<WebSocketChannel>();

            // Register the unified facade
            services.AddSingleton<IShiliuOralClient, ShiliuOralClient>();

            return services;
        }

        /// <summary>
        /// Registers all Shiliu Oral SDK services with default options.
        /// </summary>
        public static IServiceCollection AddShiliuOralSdk(
            this IServiceCollection services,
            Action<ShiliuOralOptions> configureOptions = null)
        {
            var options = new ShiliuOralOptions();
            configureOptions?.Invoke(options);
            return services.AddShiliuOralSdk(options);
        }
    }
}

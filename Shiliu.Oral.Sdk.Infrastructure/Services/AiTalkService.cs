using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiliu.Oral.Sdk.Abstractions.Auth;
using Shiliu.Oral.Sdk.Abstractions.Exceptions;
using Shiliu.Oral.Sdk.Abstractions.Enums;
using Shiliu.Oral.Sdk.Abstractions.Models;
using Shiliu.Oral.Sdk.Abstractions.Services;
using Shiliu.Oral.Sdk.Infrastructure.Http;
using SerilogLog = Serilog.Log;

namespace Shiliu.Oral.Sdk.Infrastructure.Services
{
    /// <summary>
    /// AI conversation service implementation.
    /// Adapted from the original AiTalkClient — all WPF/UI dependencies removed.
    /// </summary>
    public class AiTalkService : HttpApiClientBase, IAiTalkService
    {
        public AiTalkService(IHttpClientFactory httpClientFactory, ILogger<AiTalkService> logger, ICurrentLanguageProvider currentLanguageProvider = null)
            : base(httpClientFactory, "AiTalkClient", logger, currentLanguageProvider)
        {
        }

        public async Task<SceneResult> GetOneLevelScenesAsync(CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/scenes/oneList", "", ct);
            var result = ParseResponse<SceneResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取一级场景列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Scene[]> GetTwoLevelScenesAsync(int oneSceneId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/scenes/twoList", JsonSerializer.Serialize(new { oneScenesId = oneSceneId }), ct);
            var result = ParseResponse<Scene[]>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取二级场景列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Scene> GetSceneDetailAsync(int sceneId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/scenes/detail", JsonSerializer.Serialize(new { scenesId = sceneId }), ct);
            var result = ParseResponse<Scene>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取场景详情失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Page<List<TaskResult>>> GetTaskListAsync(int pageNum, int pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/list", JsonSerializer.Serialize(new { pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<TaskResult>>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取练习列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<List<TaskRecord>> GetTaskRecordListAsync(string speakTaskId, int pageNum, int pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/recordList", JsonSerializer.Serialize(new { speakTaskId, pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<TaskRecord>>>(content);
            if (result.Success) return result.Value?.Data ?? new List<TaskRecord>();
            throw new ShiliuOralException($"获取对话记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<UserInfoResult> GetSpeakUserInfoAsync(CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/user/getSpeakUserInfo", "", ct);
            var result = ParseResponse<UserInfoResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取用户信息失败: {result.Message}", category: ShiliuOralErrorCategory.Authentication);
        }

        public async Task<string> StartTaskAsync(int sceneId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/start", JsonSerializer.Serialize(new { sceneId, taskType = 1 }), ct);
            var result = ParseResponse<string>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"开始对话任务失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<RecommendSpeakResult> GetRecommendSpeakAsync(int sceneId, string speakTaskId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/recommendSpeak", JsonSerializer.Serialize(new { sceneId, speakTaskId }), ct);
            var result = ParseResponse<RecommendSpeakResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取推荐提示失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<TextToAudioResult> TextToAudioAsync(string text, int voiceToneId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/textToAudio", JsonSerializer.Serialize(new { text, voiceToneId }), ct);
            var result = ParseResponse<TextToAudioResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"文本转语音失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<List<VoiceTone>> GetVoiceTonesAsync(CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/voiceTone/list", "", ct);
            var result = ParseResponse<List<VoiceTone>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取音色列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<List<VoiceTone>> GetVoiceTonesListByLangAsync(string language, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/voiceTone/listByLang", JsonSerializer.Serialize(new { language }), ct);
            var result = ParseResponse<List<VoiceTone>>(content);
            if (result.Success) return result.Value ?? new List<VoiceTone>();
            throw new ShiliuOralException($"按语言获取音色列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<List<SpeakLanguage>> GetLanguageListAsync(CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/language/lang", "", ct);
            var result = ParseResponse<List<SpeakLanguage>>(content);
            if (result.Success) return result.Value ?? new List<SpeakLanguage>();
            throw new ShiliuOralException($"获取语言列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        private static readonly JsonSerializerOptions _camelCaseOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<string> SaveRecordAsync(string speakTaskId, TaskRecord taskRecord, CancellationToken ct = default)
        {
            var body = JsonSerializer.Serialize(new { speakAddTaskRecord = taskRecord, speakTaskId }, _camelCaseOptions);
            var content = await PostAsync("/api/speak/task/saveRecord", body, ct);
            var result = ParseResponse<string>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"保存对话记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<bool> UpdateRecordAsync(TaskRecord taskRecord, CancellationToken ct = default)
        {
            var body = JsonSerializer.Serialize(taskRecord, _camelCaseOptions);
            SerilogLog.Information("UpdateRecord 请求体: {Body}", body);
            var content = await PostAsync("/api/speak/task/updateRecord", body, ct);
            var result = ParseResponse<bool>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"更新对话记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<bool> StopTaskAsync(long speakTaskId, int voiceToneId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/stop", JsonSerializer.Serialize(new { speakTaskId, voiceToneId }), ct);
            var resultObject = JsonSerializer.Deserialize<JsonElement>(content);
            bool success = resultObject.GetProperty("success").GetBoolean();
            if (success) return true;
            var message = resultObject.GetProperty("msg").GetString();
            throw new ShiliuOralException($"结束对话任务失败: {message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<UploadTokenResult> GetUploadTokenAsync(string fileName, int? convert, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/common/uploadTokenV2", JsonSerializer.Serialize(new { fileName, convert }), ct);
            var result = ParseResponse<UploadTokenResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取上传Token失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<TaskDetail> GetTaskDetailAsync(long speakTaskId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/detail/v2", JsonSerializer.Serialize(new { speakTaskId }), ct);
            SerilogLog.Information("GetTaskDetail 原始响应: {Content}", content);
            var result = ParseResponse<TaskDetail>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取任务详情失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<UserSingleReviewResult> UserSingleReviewAsync(string id, string taskId, string audioPath, string requestJson, CancellationToken ct = default)
        {
            var requestBody = JsonSerializer.Serialize(new { id, taskId, audioPath, requestJson });
            SerilogLog.Information("UserSingleReview 请求体: {Body}", requestBody);
            var content = await PostAsync("/api/speak/task/correct/v2", requestBody, ct);
            SerilogLog.Information("UserSingleReview 响应: {Content}", content);
            var result = ParseResponse<UserSingleReviewResult>(content);
            if (result.Success)
            {
                if (result.Value != null)
                {
                    // OriginalResult 由 ParseResponse 从 value.originalResult 反序列化得到（原始评测JSON字符串）
                    // 若为空则从原始响应中提取，确保不携带外层 code/msg/value/success 包装
                    if (string.IsNullOrEmpty(result.Value.OriginalResult))
                    {
                        try
                        {
                            var raw = JsonSerializer.Deserialize<JsonElement>(content);
                            if (raw.TryGetProperty("value", out var valueElem) &&
                                valueElem.TryGetProperty("originalResult", out var origElem))
                            {
                                result.Value.OriginalResult = origElem.GetString();
                            }
                        }
                        catch { }
                    }
                    result.Value.ScoreSystem ??= 5;
                }
                return result.Value;
            }
            throw new ShiliuOralException($"单条评测失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Page<List<WordPhraseItem>>> GetWordPhraseListAsync(int sceneId, int type, int pageNum, int pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/practice/practiceList", JsonSerializer.Serialize(new { sceneId, type, pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<WordPhraseItem>>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取单词短语列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Page<List<PhraseListItem>>> GetPhraseListAsync(int sceneId, int pageNum, int pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/practice/problemList", JsonSerializer.Serialize(new { sceneId, pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<PhraseListItem>>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取短语练习列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task SubmitFeedbackAsync(string feedback, CancellationToken ct = default)
        {
            await PostAsync("/api/speak/userFeedback/add", JsonSerializer.Serialize(new { feedback }), ct);
        }

        public async Task<bool> VerifyBoxSerialNumbersAsync(string serialNumber, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/third/box/verifyBoxSerialNumbers", JsonSerializer.Serialize(new { sourceId = 11806, serialNumber }), ct);
            var result = ParseResponse<bool>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"设备验证失败: {result.Message}", category: ShiliuOralErrorCategory.Authentication);
        }
    }
}

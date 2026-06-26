using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shiliu.Oral.Sdk.Abstractions.Models;

namespace Shiliu.Oral.Sdk.Abstractions.Services
{
    /// <summary>
    /// AI conversation service for scene-based dialogue practice.
    /// Also owns translation APIs per the original AiTalkClient design.
    /// </summary>
    public interface IAiTalkService
    {
        /// <summary>Get top-level scene categories.</summary>
        Task<SceneResult> GetOneLevelScenesAsync(CancellationToken ct = default);

        /// <summary>Get sub-scenes for a parent scene.</summary>
        Task<Scene[]> GetTwoLevelScenesAsync(int oneSceneId, CancellationToken ct = default);

        /// <summary>Get scene detail by ID.</summary>
        Task<Scene> GetSceneDetailAsync(int sceneId, CancellationToken ct = default);

        /// <summary>Get user's practice task list (paginated).</summary>
        Task<Page<List<TaskResult>>> GetTaskListAsync(int pageNum, int pageSize, CancellationToken ct = default);

        /// <summary>Get conversation records for a task.</summary>
        Task<List<TaskRecord>> GetTaskRecordListAsync(string speakTaskId, int pageNum, int pageSize, CancellationToken ct = default);

        /// <summary>Get current user's spoken-English profile.</summary>
        Task<UserInfoResult> GetSpeakUserInfoAsync(CancellationToken ct = default);

        /// <summary>Start a new conversation task for a scene.</summary>
        Task<string> StartTaskAsync(int sceneId, CancellationToken ct = default);

        /// <summary>Get recommended speaking prompts for a task.</summary>
        Task<RecommendSpeakResult> GetRecommendSpeakAsync(int sceneId, string speakTaskId, CancellationToken ct = default);

        /// <summary>Convert text to speech audio.</summary>
        Task<TextToAudioResult> TextToAudioAsync(string text, int voiceToneId, CancellationToken ct = default);

        /// <summary>Get available voice tone list.</summary>
        Task<List<VoiceTone>> GetVoiceTonesAsync(CancellationToken ct = default);

        /// <summary>Save a conversation record.</summary>
        Task<string> SaveRecordAsync(string speakTaskId, TaskRecord taskRecord, CancellationToken ct = default);

        /// <summary>Update a conversation record.</summary>
        Task<bool> UpdateRecordAsync(TaskRecord taskRecord, CancellationToken ct = default);

        /// <summary>Stop/end a conversation task.</summary>
        Task<bool> StopTaskAsync(long speakTaskId, int voiceToneId, CancellationToken ct = default);

        /// <summary>Get upload token for cloud storage.</summary>
        Task<UploadTokenResult> GetUploadTokenAsync(string fileName, int? convert, CancellationToken ct = default);

        /// <summary>Get task detail and evaluation summary.</summary>
        Task<TaskDetail> GetTaskDetailAsync(long speakTaskId, CancellationToken ct = default);

        /// <summary>Submit single user review/evaluation.</summary>
        Task<UserSingleReviewResult> UserSingleReviewAsync(string id, string taskId, string audioPath, string requestJson, CancellationToken ct = default);

        /// <summary>Get word/phrase practice list for a scene.</summary>
        Task<Page<List<WordPhraseItem>>> GetWordPhraseListAsync(int sceneId, int type, int pageNum, int pageSize, CancellationToken ct = default);

        /// <summary>Get phrase practice problem list.</summary>
        Task<Page<List<PhraseListItem>>> GetPhraseListAsync(int sceneId, int pageNum, int pageSize, CancellationToken ct = default);

        /// <summary>Submit user feedback.</summary>
        Task SubmitFeedbackAsync(string feedback, CancellationToken ct = default);

        /// <summary>Verify device serial number (hardware binding).</summary>
        Task<bool> VerifyBoxSerialNumbersAsync(string serialNumber, CancellationToken ct = default);
    }
}

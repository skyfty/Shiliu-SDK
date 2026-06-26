namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    public class SceneResult
    {
        public Scene[] OneScenes { get; set; }
        public FreeDialogue[] FreeDialogue { get; set; }
    }

    public class Scene
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Icon { get; set; }
        public int? Sort { get; set; }
        public string ProloguePrompt { get; set; }
        public string UserInstructionPrompt { get; set; }
        public string EnUserInstructionPrompt { get; set; }
        public int? CharacterId { get; set; }
        public int Depth { get; set; }
    }

    public class FreeDialogue
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public object Icon { get; set; }
        public int Sort { get; set; }
        public string ProloguePrompt { get; set; }
        public string UserInstructionPrompt { get; set; }
        public string EnUserInstructionPrompt { get; set; }
        public int CharacterId { get; set; }
        public int Depth { get; set; }
    }
}

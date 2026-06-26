namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Spoken-language option returned by AiTalk scene APIs.
    /// </summary>
    public class SpeakLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsDefault { get; set; }
    }
}
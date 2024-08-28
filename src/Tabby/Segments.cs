namespace AICoderVS.Tabby
{
    public class Segments
    {
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Filepath { get; set; }
        public string GitUrl { get; set; }
        public Declaration[] Declarations { get; set; }
        public Snippet[] RelevantSnippetsFromChangedFiles { get; set; }
        public string Clipboard { get; set; }
    }
}
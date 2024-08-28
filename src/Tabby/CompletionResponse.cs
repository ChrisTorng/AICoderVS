namespace AICoderVS.Tabby
{
    public class CompletionResponse
    {
        public string Id { get; set; }
        public Choice[] Choices { get; set; }
        public DebugData DebugData { get; set; }
    }
}
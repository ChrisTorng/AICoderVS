namespace AICoderVS.Tabby
{
    public class CompletionRequest
    {
        public string Language { get; set; }
        public Segments Segments { get; set; }
        public string User { get; set; }
        public DebugOptions DebugOptions { get; set; }
        public float? Temperature { get; set; }
        public long? Seed { get; set; }
    }
}
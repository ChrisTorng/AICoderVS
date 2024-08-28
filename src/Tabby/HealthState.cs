namespace AICoderVS.Tabby
{
    public class HealthState
    {
        public string Model { get; set; }
        public string ChatModel { get; set; }
        public string ChatDevice { get; set; }
        public string Device { get; set; }
        public string Arch { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCount { get; set; }
        public string[] CudaDevices { get; set; }
        public Version Version { get; set; }
        public bool? Webserver { get; set; }
    }
}
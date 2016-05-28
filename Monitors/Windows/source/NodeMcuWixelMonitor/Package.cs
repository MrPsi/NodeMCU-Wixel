namespace NodeMcuWixelMonitor
{
    public class Package
    {
        public int TransmissionId { get; set; }
        public string TransmitterId { get; set; }
        public int RawValue { get; set; }
        public int FilteredValue { get; set; }
        public int BatteryLife { get; set; }
        public int ReceivedSignalStrength { get; set; }
        public int CaptureDateTime { get; set; }
        public int Uploaded { get; set; }
        public int UploadAttempts { get; set; }
        public int UploaderBatteryLife { get; set; }
        public int RelativeTime { get; set; }
    }
}
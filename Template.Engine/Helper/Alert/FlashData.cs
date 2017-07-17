namespace Template.Engine.Helper.Alert
{
    public class FlashData
    {
        public FlashData(MessageType MessageType_ = MessageType.Information, string Message_ = "",
            bool Confirmation_ = false)
        {
            MessageType = MessageType_;
            Message = Message_;
            Confirmation = Confirmation_;
        }

        public MessageType MessageType { get; set; }
        public string Message { get; set; }
        public bool Confirmation { get; set; }
    }
}
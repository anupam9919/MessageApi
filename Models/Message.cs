namespace MessageApi.Models
{
    public class Message
    {
        public int MsgId { get; set; }
        public string MessageText { get; set; }
        public string UserMob { get; set; }
        public string DemandNumber { get; set; }
        public string Project { get; set; }
        public string Location { get; set; }
    }
    public class MessageResponse
    {
        public string MessageText { get; set; }
        public DateTime? MsgDate { get; set; }
        public string? MsgStatus { get; set; }
    }
}

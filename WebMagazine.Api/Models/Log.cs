namespace WebMagazine.Api.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
        public string Exception { get; set; }
        public string User { get; set; }
    }
}

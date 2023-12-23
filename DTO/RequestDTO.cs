namespace VezetaApi.DTO
{
    public class RequestDTO
    {
        public int Requests { get; set; }
        public int PendingRequests { get; set; }
        public int completedRequests { get; set; }
        public int CancelledRequests { get; set; }
    }
}

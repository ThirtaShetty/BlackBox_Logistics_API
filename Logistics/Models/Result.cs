namespace Logistics.Models
{
    public class Result()
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public int? StatusCode { get; set;}
    }
}
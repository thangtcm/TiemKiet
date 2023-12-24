namespace TiemKiet.Models.ViewModel
{
    public class StatusResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T? Result { get; set; }
    }
}

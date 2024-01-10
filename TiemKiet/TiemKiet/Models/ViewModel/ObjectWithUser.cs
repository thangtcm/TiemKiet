namespace TiemKiet.Models.ViewModel
{
    public class ObjectWithUser<T>
    {
        public UserInfoVM User { get; set; }
        public ICollection<T> ListValue { get; set; }
        public T Value { get; set; }
    }
}

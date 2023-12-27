using TiemKiet.Models;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface INotifyFCMService
    {
        public Task SendToUser(string title, string messageBody, long userId);
        public Task SendToGroup(string title, string messageBody);
    }
}

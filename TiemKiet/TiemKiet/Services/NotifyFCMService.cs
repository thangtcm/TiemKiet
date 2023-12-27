using Firebase.Auth;
using FirebaseAdmin.Messaging;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class NotifyFCMService : INotifyFCMService
    {
        public IUnitOfWork _unitOfWork;
        public NotifyFCMService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendToGroup(string title, string messageBody)
        {
            var userNoti = (await _unitOfWork.UserRoleRepository.GetAllAsync()).Select(ur => ur.UserId)
                    .Distinct()
                    .ToList();
            var listToken = await _unitOfWork.UserTokenRepository.GetAllAsync(x => userNoti.Contains(x.Id));
            var message = new MulticastMessage()
            {
                Tokens = listToken.Select(x => x.UserToken).ToList(),
                Notification = new Notification
                {
                    Title = title,
                    Body = messageBody,
                },
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendMulticastAsync(message);
        }

        public async Task SendToUser(string title, string messageBody, long userId)
        {
            var listToken = await _unitOfWork.UserTokenRepository.GetAllAsync(x => x.Id == userId);
            var message = new MulticastMessage()
            {
                Tokens = listToken.Select(x => x.UserToken).ToList(),
                Notification = new Notification
                {
                    Title = title,
                    Body = messageBody,
                },
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendMulticastAsync(message);
        }
    }
}

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
            var listToken = await _unitOfWork.UserTokenRepository.GetAllAsync(x => userNoti.Contains(x.UserId!.Value));
            if(listToken.Count > 0)
            {
                var message = new MulticastMessage()
                {
                    Tokens = listToken.Select(x => x.UserToken).ToList(),
                    Notification = new Notification
                    {
                        Title = title,
                        Body = messageBody,
                    },
                    Apns = new ApnsConfig()
                    {
                        Headers = new Dictionary<string, string>()
                        {
                            { "apns-collapse-id", "solo_changed_administrator"},
                            { "content-available", "1"},
                            { "apns-priority", "10" },
                        },
                        Aps = new Aps()
                        {
                            Sound = "default"
                        }
                    },
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification()
                        {
                            DefaultSound = true,
                        }
                    }
                };

                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendMulticastAsync(message);
            }
            
        }

        public async Task SendToUser(string title, string messageBody, long userId)
        {
            var listToken = await _unitOfWork.UserTokenRepository.GetAllAsync(x => x.UserId == userId);
            if(listToken.Count > 0)
            {
                var message = new MulticastMessage()
                {
                    Tokens = listToken.Select(x => x.UserToken).ToList(),
                    Notification = new Notification
                    {
                        Title = title,
                        Body = messageBody,
                    },
                    Apns = new ApnsConfig()
                    {
                        Headers = new Dictionary<string, string>()
                        {
                            { "apns-collapse-id", "solo_changed_administrator"},
                            { "content-available", "1"},
                            { "apns-priority", "10" },
                        },
                        Aps = new Aps()
                        {
                            Sound = "default"
                        }
                    },
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification()
                        {
                            DefaultSound = true,
                        }
                    }
                };
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendMulticastAsync(message);
            }
        }
    }
}

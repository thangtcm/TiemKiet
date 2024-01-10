using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TiemKiet.Models;

namespace TiemKiet.Controllers
{
    public class FirebaseConfigController : Controller
    {
        private readonly FirebaseConfigVM _firebaseConfig;

        public FirebaseConfigController(IOptions<FirebaseConfigVM> firebaseConfig)
        {
            _firebaseConfig = firebaseConfig.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_firebaseConfig);
        }
    }
}

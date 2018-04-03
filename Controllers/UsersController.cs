using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using VoiceOnlineStats.Models;
using VoiceOnlineStats.Services;

namespace VoiceOnlineStats.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public IDBService dbService;
        public UsersController(IDBService dbService)
        {
            this.dbService = dbService;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            using (var db = new LiteDatabase($"filename={dbService.DBSource}; journal=false;"))
            {
                var col = db.GetCollection<UserModel>("Users");
                var result = col.FindAll().OrderByDescending(x => x.VoiceOnlineTime).Select(x => new
                {
                    Id = x.Id.ToString(),
                    x.IsConnected,
                    x.LastConnectedTime,
                    x.LastSessionTime,
                    x.LastUpdateTime,
                    x.Username,
                    x.VoiceOnlineTime
                });
                return result;
            }
        }

        [HttpGet("{voiceOnlineCount}")]
        public int GetCurrentVoiceOnlineCount([FromQuery]string type)
        {
            using (var db = new LiteDatabase($"filename={dbService.DBSource}; journal=false;"))
            {
                var col = db.GetCollection<UserModel>("Users");
                IEnumerable<UserModel> result = null;
                switch (type)
                {
                    case "current":
                        result = col.Find(x => x.IsConnected);
                        break;
                    case "max":
                        result = col.Find(x => x.VoiceOnlineTime != TimeSpan.Zero);
                        break;
                }
                return (result != null) ? result.Count() : 0;
            }
        }

        [HttpDelete("{removeall}")]
        public IActionResult RemoveAllUsers([FromBody]string psw)
        {
            if (psw.ToLower() != "support")
                return new UnauthorizedResult();
            if (System.IO.File.Exists(dbService.DBSource))
            {
                System.IO.File.Copy(dbService.DBSource, $"{dbService.DBSource}.backup_{DateTime.Now.ToString("MMddyyyy_HHmmss")}");
            }
            using (var db = new LiteDatabase($"filename={dbService.DBSource}; journal=false;"))
            {
                var col = db.GetCollection<UserModel>("Users");
                var users = col.FindAll().Select(x=> {
                    x.VoiceOnlineTime = TimeSpan.Zero; 
                    x.LastConnectedTime = DateTimeOffset.Now;
                    x.LastUpdateTime = DateTimeOffset.Now;
                    x.LastSessionTime = null;
                    x.Hours10Prize = false;
                    x.Hours20Prize = false;
                    x.Hours40Prize = false;
                    x.Hours60Prize = false;
                    x.Hours90Prize = false;
                    x.Hours120Prize = false;
                    x.Hours150Prize = false;
                    return x;});
                col.Update(users);
            }
            return new OkResult();
        }
    }
}

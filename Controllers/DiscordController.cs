using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using VoiceOnlineStats.Models;

namespace VoiceOnlineStats.Controllers
{
    [Produces("application/json")]
    [Route("api/Discord")]
    public class DiscordController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromQuery]string password)
        {
            if (password != "4815162342")
                return new UnauthorizedResult();
            using (var db = new LiteDatabase($"filename=auth.db; journal=false;"))
            {
                var col = db.GetCollection<DiscordUserModel>("DiscordUsers");
                col.EnsureIndex(x => x.Id);
                var result = col.FindAll();
                return Json(result.ToList());
            }
        }

        [HttpPost("{authorize}")]
        public IActionResult SaveUserToken([FromBody]DiscordUserModel model)
        {
            if(model == null)
                return new BadRequestResult();
            try
            {
                using (var db = new LiteDatabase($"filename=auth.db; journal=false;"))
                {
                    var col = db.GetCollection<DiscordUserModel>("DiscordUsers");
                    col.EnsureIndex(x => x.Id);
                    col.Insert(model);
                }
                return Ok();
            }
            catch(Exception ex)
            {
                Log.Logger.Error($"Exception occured: {ex.GetType()}: {ex.Message}");
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoiceOnlineStats.Models
{
    public class DiscordUserModel
    {
        public ulong Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}

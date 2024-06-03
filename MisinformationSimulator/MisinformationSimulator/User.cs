using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisinformationSimulator
{
    internal class User
    {
        public double believeMisinfoChance;
        public double repostMisinfoChance;
        public double believeFriendMisinfoChance;
        public double repostFriendMisinfoChance;
        public double downtimeLength;
        public double currentDowntime;
        public List<User> friends = new();

        public bool believesMisinfo = false;
        public bool hasRepostedMisinfo = false;
        public bool hasSeenMisinfo = false;

        public User(double believeMisinfoChance = 0.01, double repostMisinfoChance = 0.01, double believeFriendMisinfoChance = 0.02, double repostFriendMisinfoChance = 0.02, double downtimeLength = 24)
        {
            this.believeFriendMisinfoChance = believeFriendMisinfoChance;
            this.repostFriendMisinfoChance = repostFriendMisinfoChance;
            this.believeMisinfoChance = believeMisinfoChance;
            this.repostMisinfoChance = repostMisinfoChance;
            this.downtimeLength = downtimeLength;
            Random random = new Random();
            currentDowntime = (downtimeLength + 1) * random.NextDouble() - 1;
        }
    }
}

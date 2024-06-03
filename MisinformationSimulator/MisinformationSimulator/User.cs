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
        public double seeFriendPostChanceMulti;
        public double averagePostsRead;
        public List<User> friends = new();

        public bool believesMisinfo = false;
        public bool hasRepostedMisinfo = false;
        public bool hasSeenMisinfo = false;

        public User(double believeMisinfoChance = 0.1, double repostMisinfoChance = 0.1, double believeFriendMisinfoChance = 0.2, double repostFriendMisinfoChance = 0.2, double seeFriendPostChanceMulti = 500, double averagePostsRead = 100, double downtimeLength = 24)
        {
            this.averagePostsRead = averagePostsRead;
            this.seeFriendPostChanceMulti = seeFriendPostChanceMulti;
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

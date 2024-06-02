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
        public double downtimeLength;
        public double currentDowntime;
        public List<User> friends = new();

        public bool believesMisinfo = false;
        public bool hasRepostedMisinfo = false;
        public bool hasSeenMisinfo = false;

        public User(double gullibleChance = 0.01, double repostMisinfoChance = 0.01, double downtimeLength = 24)
        {
            this.believeMisinfoChance = gullibleChance;
            this.repostMisinfoChance = repostMisinfoChance;
            this.downtimeLength = downtimeLength;
            Random random = new Random();
            currentDowntime = (downtimeLength + 1) * random.NextDouble() - 1;
        }
    }
}

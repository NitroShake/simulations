using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisinformationSimulator
{
    internal class Post
    {
        public bool isMisinfo;
        public double lifeSpan;
        public double timeAlive = 0;
        public User poster;

        public Post(User poster, bool isMisinfo, double lifeSpan = 24)
        {
            this.isMisinfo = isMisinfo;
            this.poster = poster;
            this.lifeSpan = lifeSpan;
        }
    }
}

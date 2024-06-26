﻿using System;
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

        public PostCategory category;
        public int likes;
        public bool isDebunk;

        public Post(User poster, bool isMisinfo, bool isDebunk, PostCategory cat, double lifeSpan = 24)
        {
            category = cat;
            likes = 0;
            if (isMisinfo && isDebunk)
            {
                throw new Exception("it can't be both. what are you doing");
            }
            this.isDebunk = isDebunk;
            this.isMisinfo = isMisinfo;
            this.poster = poster;
            this.lifeSpan = lifeSpan;
        }
    }
}

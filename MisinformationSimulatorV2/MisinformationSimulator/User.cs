﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisinformationSimulator
{
    internal class User
    {
        public PostCategory[] preferredCategories;
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

        public double likeChance;
        public double likeFriendMulti;
        public double likeCategoryMulti;

        public double believeDebunkChance;
        public double believeDebunkWhenMisinformedMulti;
        public double believeMisinformationWhenDebunkedMulti;

        public User(double believeMisinfoChance = 0.1, double repostMisinfoChance = 0.1, double believeFriendMisinfoChance = 0.2, double repostFriendMisinfoChance = 0.2, 
            double seeFriendPostChanceMulti = 500, double averagePostsRead = 100, PostCategory[]? preferredCategories = null, int numPreferredCategories = 3, 
            double likeChance = 0.1, double likeFriendMulti = 2, double likeCategoryMulti = 2, double believeDebunkChance = 0.8, double believeDebunkWhenMisinformedMulti = 0.5,
            double believeMisinformationWhenDebunkedMulti = 0.33, double downtimeLength = 24)
        {
            this.averagePostsRead = averagePostsRead;
            this.likeChance = likeChance;
            this.likeFriendMulti = likeFriendMulti;
            this.likeCategoryMulti = likeCategoryMulti;
            this.believeDebunkChance = believeDebunkChance;
            this.believeDebunkWhenMisinformedMulti = believeDebunkWhenMisinformedMulti;
            this.believeMisinformationWhenDebunkedMulti = believeMisinformationWhenDebunkedMulti;

            //init preferred categories
            Random random = new Random();
            if (preferredCategories == null) 
            { 
                this.preferredCategories = new PostCategory[numPreferredCategories];
                Array cats = Enum.GetValues(typeof(PostCategory));
                for (int i = 0; i < numPreferredCategories; i++)
                {
                    this.preferredCategories[i] = (PostCategory)random.Next(0, cats.Length);
                }
            }
            else
            {
                this.preferredCategories = preferredCategories;
            }


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
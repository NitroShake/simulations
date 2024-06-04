using MisinformationSimulator;
using System.Diagnostics;
using System.Reflection.Metadata;

List<User> users = new List<User>();
List<Post> posts = new List<Post>();

int userCount = 10000;
int friendCountPerUser = 50;

Random random = new Random();
void simulate(float deltaHours, bool readPosts = true, bool output = true)
{
    Stopwatch sw = Stopwatch.StartNew();
    double seenMisinfoStat = 0;
    double believeMisinfoStat = 0;
    double repostedMisinfoStat = 0;
    double postsSeenStat = 0;
    double usersOnlineStat = 0;
    double seenDebunkStat = 0;
    double believeDebunkStat = 0;
    double postedDebunkStat = 0;

    double totalLikes = posts.Sum(post => post.likes);

    for (int i = 0; i < users.Count; i++)
    {
        User user = users[i];
        //if user is online
        if (user.currentDowntime <= 0)
        {
            usersOnlineStat++;

            bool willPostMisinfo = false;
            bool willPostDebunk = false;

            PostCategory? categoryToPost = null;

            //simulate reading posts
            if (readPosts)
            {
                for (int j = 0; j < posts.Count; j++)
                {
                    Post post = posts[j];

                    double readPostChance = user.averagePostsRead * (post.likes + 1) / totalLikes;
                    double likeChance = user.likeChance;

                    bool postIsFromFriend = user.friends.Contains(post.poster);

                    if (postIsFromFriend)
                    {
                        readPostChance *= user.seeFriendPostChanceMulti;
                        likeChance *= user.likeFriendMulti;
                    }

                    if (user.preferredCategories.Contains(post.category))
                    {
                        readPostChance *= user.prefersCategoryMulti;
                        likeChance *= user.likeCategoryMulti;
                    }

                    if (random.NextDouble() < readPostChance)
                    {
                        postsSeenStat++;

                        if (post.isMisinfo)
                        {
                            user.hasSeenMisinfo = true;
                            double believeMisinfoChance;
                            double repostMisinfoChance;

                            if (postIsFromFriend)
                            {
                                believeMisinfoChance = user.believeFriendMisinfoChance;
                                repostMisinfoChance = user.repostFriendMisinfoChance;
                            }
                            else
                            {
                                believeMisinfoChance = user.believeMisinfoChance;
                                repostMisinfoChance = user.repostMisinfoChance;
                            }

                            if (user.believesDebunk)
                            {
                                believeMisinfoChance *= user.believeMisinformationWhenDebunkedMulti;
                                repostMisinfoChance *= user.repostMisinformationWhenDebunkedMulti;
                            }

                            if (random.NextDouble() < believeMisinfoChance)
                            {
                                user.believesMisinfo = true;
                                user.believesDebunk = false;
                            }
                            else
                            {
                                likeChance = 0;
                            }

                            if (!user.hasRepostedMisinfo && random.NextDouble() < repostMisinfoChance)
                            {
                                willPostDebunk = false;
                                willPostMisinfo = true;
                                categoryToPost = post.category;
                            }

                            if (!user.believesMisinfo && !willPostMisinfo && random.NextDouble() < user.postsDebunkInResponseToMisinfoChance)
                            {
                                if (postIsFromFriend && random.NextDouble() < user.postsDebunkInResponseToFriendMisinfoChance)
                                {
                                    willPostDebunk = true;
                                    categoryToPost = post.category;
                                    willPostMisinfo = false;
                                }
                                else if (!postIsFromFriend && random.NextDouble() < user.postsDebunkInResponseToMisinfoChance)
                                {
                                    willPostDebunk = true;
                                    categoryToPost = post.category;
                                    willPostMisinfo = false;
                                }
                            }
                        }
                        else if (post.isDebunk)
                        {
                            user.hasSeenDebunk = true;
                            double believeDebunkChance = user.believeDebunkChance;
                            double repostDebunkChance = user.repostDebunkChance;

                            if (user.believesMisinfo)
                            {
                                believeDebunkChance *= user.believeDebunkWhenMisinformedMulti;
                                repostDebunkChance *= user.repostDebunkWhenMisinformedMulti;
                            }

                            if (random.NextDouble() < believeDebunkChance)
                            {
                                user.believesDebunk = true;
                                user.believesMisinfo = false;
                            }
                            else
                            {
                                likeChance = 0;
                            }

                            if (random.NextDouble() < repostDebunkChance)
                            {
                                willPostDebunk = true;
                                categoryToPost = post.category;
                                willPostMisinfo = false;
                            }
                        }

                        if (random.NextDouble() < likeChance)
                        {
                            post.likes++;
                            totalLikes++;
                        }
                    }
                }
            }

            //simulate creating post
            if (willPostMisinfo)
            {
                posts.Add(new Post(user, true, false, (PostCategory)categoryToPost));
                user.hasRepostedMisinfo = true;
            }
            else if (willPostDebunk)
            {
                posts.Add(new Post(user, false, true, (PostCategory)categoryToPost));
                user.hasPostedDebunk = true;
            }
            else
            {
                posts.Add(new Post(user, false, false, user.preferredCategories[random.Next(0, user.preferredCategories.Length)]));
            }

            //make user go offline
            user.currentDowntime += user.downtimeLength;
        }
        else
        {
            //user gets closer to coming online
            user.currentDowntime -= deltaHours;
        }

        if (user.hasRepostedMisinfo)
        {
            repostedMisinfoStat++;
        }

        if (user.believesMisinfo)
        {
            believeMisinfoStat++;
        }

        if (user.hasSeenMisinfo)
        {
            seenMisinfoStat++;
        }

        if (user.believesDebunk)
        {
            believeDebunkStat++;
        }

        if (user.hasPostedDebunk)
        {
            postedDebunkStat++;
        }

        if (user.hasSeenDebunk)
        {
            seenDebunkStat++;
        }
    }

    //remove posts that have lived too long
    for (int i = 0; i < posts.Count; i++)
    {
        posts[i].timeAlive += deltaHours;
        if (posts[i].timeAlive > posts[i].lifeSpan)
        {
            posts.RemoveAt(i);
            i--;
        }
    }

    sw.Stop();
    if (output)
    {
        Console.WriteLine($"believe misinfo: {believeMisinfoStat / users.Count() * 100}%");
        Console.WriteLine($"seen misinfo: {seenMisinfoStat / users.Count() * 100}%");
        Console.WriteLine($"reposted misinfo: {repostedMisinfoStat / users.Count() * 100}%");
        Console.WriteLine($"believe debunk: {believeDebunkStat / users.Count() * 100}%");
        Console.WriteLine($"seen debunk: {seenDebunkStat / users.Count() * 100}%");
        Console.WriteLine($"posted debunk: {postedDebunkStat / users.Count() * 100}%");
        Console.WriteLine($"posts seen per user: {postsSeenStat / usersOnlineStat}");
        Console.WriteLine($"users online: {usersOnlineStat}");
        Console.WriteLine($"current posts: {posts.Count()}");
        Console.WriteLine($"time taken to run (s): {(double)sw.ElapsedTicks / 10000000}");
    }
}


Console.WriteLine("Setting up users...");
//init list of users
for (int i = 0; i < userCount; i++)
{
    users.Add(new User(0.1, 0.1));
}

//add friends to users
for (int i = 0; i < userCount; i++)
{
    for (int j = 0; j < friendCountPerUser; j++)
    {
        int friendIndex = 0;
        do
        {
            friendIndex = random.Next(0, userCount);
        } 
        while (friendIndex == i || users[i].friends.Contains(users[friendIndex]));

        // users[i].addFriend(users[friendIndex]);
        users[i].friends.Add(users[friendIndex]);
        users[friendIndex].friends.Add(users[i]);
    }
}

Console.WriteLine("Setting up posts... (this may take a while)");
int setupHours = 24;
//simulate one post lifetime cycle to populate post list
for (int i = 0; i < setupHours; i++)
{
    Console.WriteLine($"Running setup hour {i}/{setupHours}");
    simulate(1, true, false);
}


//add misinfo post
posts.Add(new Post(users[0], true, false, PostCategory.OTHER));

//establish which hours debunks are guaranteed to post
int[] debunkIndexes = { 24 };

for (int i = 0; i < 48; i++)
{
    Console.WriteLine();
    if (debunkIndexes.Contains(i))
    {
        Console.WriteLine("Adding in debunk post");
        posts.Add(new Post(users[1], false, true, PostCategory.OTHER));
    }
    Console.WriteLine($"Running Hour {i}...");
    simulate(1);
}
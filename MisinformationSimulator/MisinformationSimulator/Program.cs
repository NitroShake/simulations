using MisinformationSimulator;
using System.Diagnostics;
using System.Reflection.Metadata;

List<User> users = new List<User>();
List<Post> posts = new List<Post>();

int userCount = 100000;
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

    for (int i = 0; i < users.Count; i++)
    {
        User user = users[i];
        //if user is online
        if (user.currentDowntime <= 0)
        {
            usersOnlineStat++;

            bool willPostMisinfo = false;

            //simulate reading posts
            if (readPosts)
            {
                double baseReadPostChance = user.averagePostsRead / posts.Count;
                for (int j = 0; j < posts.Count; j++)
                {
                    Post post = posts[j];

                    double readPostChance = baseReadPostChance;
                    bool postIsFromFriend = user.friends.Contains(post.poster);

                    if (postIsFromFriend)
                    {
                        readPostChance *= user.seeFriendPostChanceMulti;
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

                            if (random.NextDouble() < believeMisinfoChance)
                            {
                                user.believesMisinfo = true;
                            }

                            if (!user.hasRepostedMisinfo && random.NextDouble() < repostMisinfoChance)
                            {
                                willPostMisinfo = true;
                            }
                        }
                    }
                }
            }

            //simulate creating post
            if (willPostMisinfo)
            {
                posts.Add(new Post(user, true));
                user.hasRepostedMisinfo = true;
            }
            else
            {
                posts.Add(new Post(user, false));
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
        Console.WriteLine($"believe: {believeMisinfoStat / users.Count() * 100}%");
        Console.WriteLine($"seen: {seenMisinfoStat / users.Count() * 100}%");
        Console.WriteLine($"reposted: {repostedMisinfoStat / users.Count() * 100}%");
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
        while (friendIndex == i && users[i].friends.Contains(users[friendIndex]));

        users[i].friends.Add(users[friendIndex]);
        users[friendIndex].friends.Add(users[i]);
    }
}

Console.WriteLine("Setting up posts...");
//simulate one post lifetime cycle to populate post list
for (int i = 0; i < 24; i++)
{
    double highestDowntime = -999, lowestDowntime = 999;

    for (int j = 0; j < users.Count; j++)
    {
        if (users[j].currentDowntime > highestDowntime)
        {
            highestDowntime = users[j].currentDowntime;
        }

        if (users[j].currentDowntime < lowestDowntime)
        {
            lowestDowntime = users[j].currentDowntime;
        }
    }
    simulate(1, false, false);
}
posts.Add(new Post(users[0], true));

for (int i = 0; i < 24; i++)
{
    double highestDowntime = -999, lowestDowntime = 999;

    for (int j = 0; j < users.Count; j++)
    {
        if (users[j].currentDowntime > highestDowntime)
        {
            highestDowntime = users[j].currentDowntime;
        }

        if (users[j].currentDowntime < lowestDowntime)
        {
            lowestDowntime = users[j].currentDowntime;
        }
    }
    Console.WriteLine();
    Console.WriteLine($"Hour {i}");
    simulate(1);
}
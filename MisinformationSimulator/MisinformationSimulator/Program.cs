using MisinformationSimulator;
using System.Reflection.Metadata;

List<User> users = new List<User>();
List<Post> posts = new List<Post>();

int userCount = 300000;
int friendCountPerUser = 10;

Random random = new Random();
void simulate(float deltaHours, bool readPosts = true, bool output = true)
{
    double seenMisinfoStat = 0;
    double believeMisinfoStat = 0;
    double repostedMisinfoStat = 0;
    double postsSeenStat = 0;
    double usersOnlineStat = 0;

    for (int i = 0; i < users.Count; i++)
    {
        //if user is online
        if (users[i].currentDowntime <= 0)
        {
            usersOnlineStat++;

            bool willPostMisinfo = false;

            //simulate reading posts
            if (readPosts)
            {
                double baseReadPostChance = 100d / posts.Count;
                for (int j = 0; j < posts.Count; j++)
                {
                    if (random.NextDouble() < baseReadPostChance)
                    {
                        postsSeenStat++;

                        if (posts[j].isMisinfo)
                        {
                            users[i].hasSeenMisinfo = true;
                            if (random.NextDouble() < users[i].believeMisinfoChance)
                            {
                                users[i].believesMisinfo = true;
                            }

                            if (!users[i].hasRepostedMisinfo && random.NextDouble() < users[i].repostMisinfoChance)
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
                posts.Add(new Post(users[i], true));
                users[i].hasRepostedMisinfo = true;
            }
            else
            {
                posts.Add(new Post(users[i], false));
            }

            //make user go offline
            users[i].currentDowntime += users[i].downtimeLength;
        }
        else
        {
            //user gets closer to coming online
            users[i].currentDowntime -= deltaHours;
        }

        if (users[i].hasRepostedMisinfo)
        {
            repostedMisinfoStat++;
        }

        if (users[i].believesMisinfo)
        {
            believeMisinfoStat++;
        }

        if (users[i].hasSeenMisinfo)
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

    if (output)
    {
        Console.WriteLine($"believe: {believeMisinfoStat / users.Count()}");
        Console.WriteLine($"seen: {seenMisinfoStat / users.Count()}");
        Console.WriteLine($"reposted: {repostedMisinfoStat / users.Count()}");
        Console.WriteLine($"posts seen per user: {postsSeenStat / usersOnlineStat}");
        Console.WriteLine($"users online: {usersOnlineStat}");
        Console.WriteLine($"current posts: {posts.Count()}");
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
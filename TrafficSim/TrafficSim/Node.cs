using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    internal class Node
    {
        public List<Road> roads = new();
        public Vector3 position;
        double timeToTraverse = 2;

    }
}

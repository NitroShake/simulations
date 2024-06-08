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
        string label;

        public Node(Vector3 pos, string label = "", double timeToTraverse = 2) 
        {
            this.label = label;
            this.position = pos;
            this.timeToTraverse = timeToTraverse;
        }
    }
}

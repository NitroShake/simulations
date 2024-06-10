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
        public double timeToTraverse = 1.5;
        string label;

        public Node(Vector3 pos, string label = "", double timeToTraverse = 2) 
        {
            this.label = label;
            this.position = pos;
            this.timeToTraverse = timeToTraverse;
        }

        public Road getRoadToNode(Node node)
        {
            Road result = null;
            foreach (Road road in roads)
            {
                if (road.getOpposingNode(this) != node)
                {
                    result = road;
                }
            }
            if (result == null)
            {
                throw new Exception("YOU SUCK. UNINSTALL RIGHT NOW");
            }
            return result;
        }
    }
}

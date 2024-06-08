using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    internal class Road
    {
        public double cost;
        double speedLimit;
        Node node1;
        Node node2;

        public Node getOpposingNode(Node node)
        {
            if (node == node1)
            {
                return node2;
            }
            else if (node == node2)
            {
                return node1;
            }
            else
            {
                throw new ArgumentException("HEY THIS ISN'T A CORRECT NODE. FIX IT");
            }

        }

        public Road(Node node1, Node node2, double speedLimit = 60)
        {
            this.node1 = node1;
            node1.roads.Add(this);
            this.node2 = node2;
            node2.roads.Add(this);
            cost = Vector3.Distance(node1.position, node2.position) / speedLimit;
        }
    }
}

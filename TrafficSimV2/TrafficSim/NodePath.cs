using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    internal class NodePath
    {
        public Node node;
        public List<Road> roadsToNode;
        public double cost;

        public NodePath(Node node, List<Road> roadsToNode, double cost)
        {
            this.node = node;
            this.roadsToNode = roadsToNode;
            this.cost = cost;
        }

        public double getTotalCostToNode(Node node)
        {
            return cost + Vector3.Distance(this.node.position, node.position);
        }
    }
}

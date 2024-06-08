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
        public List<Node> pathToNode;
        public double cost;

        public NodePath(Node node, List<Node> pathToNode, double cost)
        {
            this.node = node;
            this.pathToNode = pathToNode;
            this.cost = cost;
        }

        public double getTotalCostToNode(Node node)
        {
            return cost + Vector3.Distance(this.node.position, node.position);
        }
    }
}

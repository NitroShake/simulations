using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    internal class Road
    {
        double cost;
        public double maxSpeed;
        public double slowestSpeedDrivenByCars;
        Node node1;
        public bool canAccessNode1 = true;
        Node node2;
        public bool canAccessNode2 = true;
        public int numberOfCarsJammedToNode1;
        public int numberOfCarsJammedToNode2;

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

        public bool canAccessNode(Node node)
        {
            if (node == node1)
            {
                return canAccessNode1;
            }
            else if (node == node2)
            {
                return canAccessNode2;
            }
            else
            {
                throw new ArgumentException("HEY THIS ISN'T A CORRECT NODE. FIX IT");
            }
        }

        public Road(Node node1, int jammedCarsOnWayToNode1, Node node2, int jammedCarsOnWayToNode2, double maxSpeed = 26, Node blockedNode = null)
        {
            numberOfCarsJammedToNode1 = jammedCarsOnWayToNode1;
            numberOfCarsJammedToNode2 = jammedCarsOnWayToNode2;
            this.node1 = node1;
            node1.roads.Add(this);
            this.node2 = node2;
            node2.roads.Add(this);
            this.maxSpeed = maxSpeed;
            cost = Vector3.Distance(node1.position, node2.position);
            if (blockedNode != null)
            {
                if (blockedNode == node1)
                {
                    canAccessNode1 = false;
                }
                else if (blockedNode == node2)
                {
                    canAccessNode2 = false;
                }
                else
                {
                    throw new Exception("YOU ARE SO BAAAAAD");
                }
            }
        }

        public double getCost(Node node)
        {
            int jammed;
            if (node == node1)
            {
                jammed = numberOfCarsJammedToNode1;
            }
            else if (node == node2)
            {
                jammed = numberOfCarsJammedToNode2;
            }
            else
            {
                throw new Exception("wha");
            }
            return cost / maxSpeed + (node.timeToTraverse * jammed);
        }
    }
}

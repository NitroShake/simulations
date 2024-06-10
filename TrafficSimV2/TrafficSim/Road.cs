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
        public int roadId;
        public double baseCost;
        public double maxSpeed;
        public List<Car> carsOnRoad = new();
        public double slowestSpeedDrivenByCars;
        Node node1;
        Node node2;
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



        public Road(int roadId, Node node1, int jammedCarsOnWayToNode1, Node node2, int jammedCarsOnWayToNode2, double maxSpeed = 26)
        {
            this.roadId = roadId;
            numberOfCarsJammedToNode1 = jammedCarsOnWayToNode1;
            numberOfCarsJammedToNode2 = jammedCarsOnWayToNode2;
            this.node1 = node1;
            node1.roads.Add(this);
            this.node2 = node2;
            node2.roads.Add(this);
            this.maxSpeed = maxSpeed;
            baseCost = Vector3.Distance(node1.position, node2.position);
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
            return baseCost / slowestSpeedDrivenByCars + (node.timeToTraverse * jammed);
        }
    }
}

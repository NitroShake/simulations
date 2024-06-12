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
        public bool canAccessNode1 = true;
        Node node2;
        public bool canAccessNode2 = true;

        public List<List<Car>> carsSnapshots;

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



        public Road(int roadId, Node node1, Node node2, double maxSpeed = 26, Node blockedNode = null)
        {
            this.roadId = roadId;
            this.node1 = node1;
            node1.roads.Add(this);
            this.node2 = node2;
            node2.roads.Add(this);
            this.maxSpeed = maxSpeed;
            baseCost = Vector3.Distance(node1.position, node2.position);
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

        public double getCostUsingSnapshots(Node node, int index)
        {
            double slowestSpeedDrivenByCars = double.MaxValue;
            double jammed = 0;

            foreach (Car car in carsSnapshots[index])
            {
                if (car.targetNodeSnapshots[index] == node) 
                {
                    if (slowestSpeedDrivenByCars > car.speedMulti * maxSpeed)
                    {
                        slowestSpeedDrivenByCars = car.speedMulti * maxSpeed;
                    }
                    if (car.stateSnapshots[index] == Car.State.WaitingInLine)
                    {
                        jammed++;
                    }                    
                }
            }
            return baseCost / slowestSpeedDrivenByCars + (node.timeToTraverse * jammed);
        }

        public void snapshot()
        {
            carsSnapshots.Add(new(carsOnRoad));
        }
    }
}

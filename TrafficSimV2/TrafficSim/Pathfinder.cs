using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    static class Pathfinder
    {
        static NodePath getLowestCostPath(List<NodePath> paths, Node endNode)
        {
            NodePath lowestCostPath = null;
            double cost = double.MaxValue;
            foreach (NodePath path in paths)
            {
                double pathCost = path.getTotalCostToNode(endNode);
                if (pathCost < cost)
                {
                    cost = pathCost;
                    lowestCostPath = path;
                }
            }
            return lowestCostPath;
        }

        public static NodePath getFastestPath(Node startNode, Node endNode, Func<Road, Node, double> costFunction)
        {
            List<Node> closedNodes = new List<Node>
            {
                startNode
            };
            List<NodePath> potentialPaths = new List<NodePath>
            {
                new(startNode, new(), 0)
            };


            NodePath pathToNode = null;

            while (potentialPaths.Count > 0)
            {
                NodePath fastestPath = getLowestCostPath(potentialPaths, endNode);
                Node node = fastestPath.node;
                foreach (Road road in node.roads)
                {
                    Node nextNode = road.getOpposingNode(node);
                    if (road.getOpposingNode(node) == endNode)
                    {
                        List<Road> newPath = new(fastestPath.roadsToNode);
                        newPath.Add(road);
                        double newCost = fastestPath.cost + costFunction(road, nextNode);
                        pathToNode = new(nextNode, newPath, newCost);
                        //empty potential paths to break loop
                        potentialPaths = new List<NodePath>();
                    }
                    else if (pathToNode == null && !closedNodes.Contains(nextNode))
                    {
                        List<Road> newPath = new(fastestPath.roadsToNode);
                        newPath.Add(road);
                        double newCost = fastestPath.cost + costFunction(road, nextNode);
                        potentialPaths.Add(new(nextNode, newPath, newCost));
                        closedNodes.Add(nextNode);
                    }
                }
                potentialPaths.Remove(fastestPath);
            }
            return pathToNode;
        }
        public static NodePath complexGetFastestPath(Node startNode, Node endNode, List<Car> cars, List<Road> roads, double costPerIndex)
        {
            List<Node> closedNodes = new List<Node>
            {
                startNode
            };
            List<NodePath> potentialPaths = new List<NodePath>
            {
                new(startNode, new(), 0)
            };

            NodePath pathToNode = null;

            while (potentialPaths.Count > 0)
            {
                NodePath fastestPath = getLowestCostPath(potentialPaths, endNode);
                Node node = fastestPath.node;

                int largestCost = 0;

                foreach (Road road in node.roads)
                {
                    Node nextNode = road.getOpposingNode(node);
                    if (road.getOpposingNode(node) == endNode)
                    {
                        List<Road> newPath = new(fastestPath.roadsToNode);
                        newPath.Add(road);
                        double newCost = fastestPath.cost + road.getCostUsingSnapshots(nextNode, (int)fastestPath.cost);
                        if (largestCost < newCost)
                        {
                            largestCost = (int)newCost;
                        }
                        pathToNode = new(nextNode, newPath, newCost);
                        //empty potential paths to break loop
                        potentialPaths = new List<NodePath>();
                    }
                    else if (pathToNode == null && !closedNodes.Contains(nextNode))
                    {
                        List<Road> newPath = new(fastestPath.roadsToNode);
                        newPath.Add(road);
                        double newCost = fastestPath.cost + road.getCostUsingSnapshots(nextNode, (int)fastestPath.cost);
                        potentialPaths.Add(new(nextNode, newPath, newCost));
                        closedNodes.Add(nextNode);
                    }
                }

                int index = (int)(costPerIndex * largestCost);
                while (cars[0].progressSnapshots.Count < index)
                {
                    foreach (Car car in cars)
                    {
                        car.update(costPerIndex);
                    }
                    foreach (Road road in roads)
                    {
                        road.snapshot();
                    }
                }

                potentialPaths.Remove(fastestPath);
            }


            return pathToNode;
        }
    }
}

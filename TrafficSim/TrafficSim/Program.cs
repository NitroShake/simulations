using System.IO;
using System.Numerics;
using TrafficSim;

NodePath getLowestCostPath(List<NodePath> paths, Node endNode)
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

NodePath getFastestPath(Node startNode, Node endNode)
{
    List<Node> closedNodes = new List<Node>();
    List<NodePath> potentialPaths = new List<NodePath>();

    NodePath pathToNode = null;

    while (potentialPaths.Count > 0)
    {
        NodePath fastestPath = getLowestCostPath(potentialPaths, endNode);
        Node node = fastestPath.node;
        foreach (Road road in node.roads)
        {
            Node nextNode = road.getOpposingNode(node);
            if (pathToNode == null && !closedNodes.Contains(nextNode))
            {
                List<Node> newPath = new();
                newPath = (List<Node>)newPath.Concat(fastestPath.pathToNode);
                newPath.Add(nextNode);
                double newCost = fastestPath.cost + road.cost + Vector3.Distance(node.position, endNode.position);
                potentialPaths.Add(new(node, newPath, newCost));
                closedNodes.Add(node);
            }
            else if (road.getOpposingNode(node) == endNode)
            {
                List<Node> newPath = new();
                newPath = (List<Node>)newPath.Concat(fastestPath.pathToNode);
                newPath.Add(endNode);
                double newCost = fastestPath.cost + road.cost + Vector3.Distance(node.position, endNode.position);
                pathToNode = new(node, newPath, newCost);
                //empty potential paths to break loop
                potentialPaths = new List<NodePath>();
            }

        }
    }
    return pathToNode;
}



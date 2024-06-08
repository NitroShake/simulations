using System.IO;
using System.Numerics;
using TrafficSim;

NodePath getLowestCostPath(List<NodePath> paths)
{
    NodePath lowestCostPath = null;
    double cost = double.MaxValue;
    foreach (NodePath path in paths)
    {
        if (path.cost < cost)
        {
            cost = path.cost;
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
        NodePath fastestPath = getLowestCostPath(potentialPaths);
        Node node = fastestPath.node;
        foreach (Road road in node.roads)
        {
            if (pathToNode == null && !closedNodes.Contains(road.getOpposingNode(node)))
            {
                List<Node> newPath = new();
                newPath = (List<Node>)newPath.Concat(fastestPath.pathToNode);
                newPath.Add(node);
                double newCost = fastestPath.cost + road.cost + Vector3.Distance(node.position, endNode.position);
                potentialPaths.Add(new(node, newPath, newCost));
            }
            else if (road.getOpposingNode(node) == endNode)
            {
                List<Node> newPath = new();
                newPath = (List<Node>)newPath.Concat(fastestPath.pathToNode);
                newPath.Add(node);
                double newCost = fastestPath.cost + road.cost + Vector3.Distance(node.position, endNode.position);
                pathToNode = new(node, newPath, newCost);
                //empty potential paths to break loop
                potentialPaths = new List<NodePath>();
            }
        }
    }
    return pathToNode;
}



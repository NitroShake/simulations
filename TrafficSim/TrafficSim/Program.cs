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
    List<Node> closedNodes = new List<Node>
    {
        startNode
    };
    List<NodePath> potentialPaths = new List<NodePath>
    {
        new(startNode, new Node[] {startNode}.ToList(), 0)
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
                List<Node> newPath = new(fastestPath.pathToNode);
                newPath.Add(endNode);
                double newCost = fastestPath.cost + road.cost;
                pathToNode = new(nextNode, newPath, newCost);
                //empty potential paths to break loop
                potentialPaths = new List<NodePath>();
            }
            else if (pathToNode == null && !closedNodes.Contains(nextNode))
            {
                List<Node> newPath = new(fastestPath.pathToNode);
                newPath.Add(nextNode);
                double newCost = fastestPath.cost + road.cost;
                potentialPaths.Add(new(nextNode, newPath, newCost));
                closedNodes.Add(nextNode);
            }
        }
        potentialPaths.Remove(fastestPath);
    }
    return pathToNode;
}

Node[] nodes =
{ 
    new(new(590, 0, -140), "0"),
    new(new(470,0, -40), "1"),
    new(new(600, 0, 80), "2"),
    new(new(220, 0, 0), "3"),
    new(new(410, 0, 200), "4"),
    new(new(600, 0, 310), "5"),
    new(new(280, 0, 230), "6"),
    new(new(360, 0, 330), "7"),
    new(new(30, 0, 50), "8"),
    new(new(150, 0, 230), "9"),
    new(new(360, 0, 500), "10"),
    new(new(620, 0, 540), "11"),
    new(new(-10, 0, 320), "12"),
    new(new(150, 0, 400), "13"),
    new(new(40, 0, 545), "14")
};

Road[] roads =
{
    new(nodes[0], nodes[1]),
    new(nodes[1], nodes[2]),
    new(nodes[0], nodes[2]),
    new(nodes[2], nodes[5]),
    new(nodes[5], nodes[11]),
    new(nodes[11], nodes[10]),
    new(nodes[10], nodes[5]),
    new(nodes[10], nodes[7]),
    new(nodes[7], nodes[4]),
    new(nodes[4], nodes[2]),
    new(nodes[4], nodes[6]),
    new(nodes[6], nodes[9]),
    new(nodes[9], nodes[13]),
    new(nodes[13], nodes[10]),
    new(nodes[10], nodes[14]),
    new(nodes[14], nodes[13]),
    new(nodes[14], nodes[12]),
    new(nodes[12], nodes[9]),
    new(nodes[12], nodes[8]),
    new(nodes[8], nodes[3]),
    new(nodes[3], nodes[9]),
    new(nodes[3], nodes[1]),
};

Node[] path = getFastestPath(nodes[0], nodes[13]).pathToNode.ToArray();
Console.WriteLine(""""""""aaaaaamnong us/1"""""""");
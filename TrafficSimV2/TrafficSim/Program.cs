using System.IO;
using System.Numerics;
using TrafficSim;

Node[] nodes =
{ 
    new(new(570, 0, -80), "0"),
    new(new(440,0, 0), "1"),
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
    new(new(40, 0, 545), "14"),
    new(new(40, 0, 545), "15")
};

List<Road> roads =
new Road[] {
    new(0, nodes[0], nodes[1]),
    new(0, nodes[1], nodes[3]),
    new(0, nodes[3], nodes[8]),
    new(0, nodes[8], nodes[12]),
    new(0, nodes[12], nodes[14]),
    new(0, nodes[14], nodes[10]),
    new(0, nodes[10], nodes[11]),
    new(0, nodes[11], nodes[5]),
    new(0, nodes[5], nodes[2]),
    new(0, nodes[2], nodes[0]),

    new(1, nodes[14], nodes[13]),
    new(1, nodes[13], nodes[9]),
    new(1, nodes[9], nodes[3]),

    new(2, nodes[9], nodes[6]),

    new(3, nodes[7], nodes[10]),

    new(4, nodes[15], nodes[5]),

    new(5, nodes[7], nodes[6], blockedNode: nodes[7]),
    new(5, nodes[6], nodes[4], blockedNode: nodes[6]),
    new(5, nodes[4], nodes[15], blockedNode: nodes[4]),
    new(5, nodes[15], nodes[7], blockedNode: nodes[15]),
}.ToList();

//Road[] roaaaaaaaads = Pathfinder.getFastestPath(nodes[0], nodes[13], (road, nextNode) => road.getCost(nextNode)).roadsToNode.ToArray();

//NodePath[] roaaaaaaaads = Pathfinder.complexGetFastestPath(nodes[0], nodes[13], new(), roads, 1);
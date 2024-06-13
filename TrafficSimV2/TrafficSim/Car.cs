using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Formats.Asn1;

namespace TrafficSim
{
    internal class Car
    {
        public enum State
        {
            Driving,
            WaitingInLine,
            WaitingForTurn,
            Turning,
            Arrived
        }

        NodePath path;
        int roadIndex = 0;
        double progressAlongRoad = 0;
        Node targetNode;
        public double speedMulti;
        double turnProgress = 0;
        State state;

        public List<double> progressSnapshots = new();
        public List<State> stateSnapshots = new();
        public List<Node> targetNodeSnapshots = new();
        public List<int> roadIndexSnapshots = new();

        public Car(Node startNode, Node endNode, double speedMulti = 1)
        {
            path = Pathfinder.getFastestPath(startNode, endNode, (road, nextNode) => road.baseCost);
            this.speedMulti = speedMulti;
            state = State.Driving;
            targetNode = path.roadsToNode[0].getOpposingNode(startNode);

            progressSnapshots.Add(0);
            stateSnapshots.Add(state);
            targetNodeSnapshots.Add(targetNode);
            roadIndexSnapshots.Add(roadIndex);
        }

        Car carInFront = null;
        public void update(double deltaTime)
        {
            Road road = path.roadsToNode[roadIndex];

            if (state == State.Driving)
            {
                double speed = speedMulti * road.maxSpeed;
                bool shouldStop = false;
                foreach (Car car in road.carsOnRoad)
                {
                    double distance = progressAlongRoad - car.progressAlongRoad;
                    if (distance < 5 && distance > 5)
                    {
                        speed = car.speedMulti * road.maxSpeed;
                        if (car.state == State.WaitingInLine || car.state == State.WaitingForTurn)
                        {
                            shouldStop = true;
                            car.state = State.WaitingInLine;
                            carInFront = car;
                        }
                    }
                }

                if (!shouldStop) 
                { 
                    progressAlongRoad += speed * deltaTime;
                    if (progressAlongRoad >= road.baseCost)
                    {
                        if (roadIndex + 1 == path.roadsToNode.Count) 
                        {
                            state = State.Arrived;
                        }
                        //if next road is a turn
                        else if (path.roadsToNode[roadIndex + 1].roadId != road.roadId)
                        {
                            state = State.WaitingForTurn;
                        }
                        else
                        {
                            advanceToNextRoad();
                        }
                    }
                }
            }


            if (state == State.WaitingInLine) 
            {
                if (carInFront.state != State.WaitingInLine && carInFront.state != State.WaitingForTurn)
                {
                    state = State.Driving;
                    carInFront = null;
                }
            }


            if (state == State.WaitingForTurn)
            {
                Vector3 currentDirV3 = targetNode.position - road.getOpposingNode(targetNode).position;
                Vector2 currentDirection = new Vector2(currentDirV3.X, currentDirV3.Z);
                Vector3 newDirV3 = path.roadsToNode[roadIndex + 1].getOpposingNode(targetNode).position - targetNode.position;
                Vector2 newDirection = new Vector2(newDirV3.X, newDirV3.Z);
                double dot = currentDirection.X * newDirection.X + currentDirection.Y * newDirection.Y;
                double det = currentDirection.X * newDirection.Y - currentDirection.Y * newDirection.X;
                double angle = Math.Atan2(det, dot);
                bool isTurningRight = angle < 0;

                Console.WriteLine(angle);
                bool canTurn = true;
                foreach(Road connectedRoad in targetNode.roads)
                {
                    if (road != connectedRoad)
                    {
                        foreach (Car car in connectedRoad.carsOnRoad)
                        {
                            if ((isTurningRight && car.roadIndex != roadIndex + 1) || (!isTurningRight && car.roadIndex == roadIndex + 1))
                            {
                                if (car.targetNode != targetNode)
                                {
                                    if (car.progressAlongRoad < 5)
                                    {
                                        canTurn = false;
                                    }
                                }
                                else
                                {
                                    if (car.path.roadsToNode[car.roadIndex].baseCost - car.progressAlongRoad < 10)
                                    {
                                        canTurn = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (canTurn)
                {
                    state = State.Turning;
                }
            }


            if (state == State.Turning)
            {
                turnProgress += deltaTime;
                if (turnProgress >= targetNode.timeToTraverse)
                {
                    advanceToNextRoad();
                }
            }

            progressSnapshots.Add(progressAlongRoad);
            stateSnapshots.Add(state);
            targetNodeSnapshots.Add(targetNode);
            roadIndexSnapshots.Add(roadIndex);
        }

        void advanceToNextRoad()
        {
            path.roadsToNode[roadIndex].carsOnRoad.Remove(this);
            roadIndex++;
            targetNode = path.roadsToNode[roadIndex].getOpposingNode(targetNode);
            path.roadsToNode[roadIndex].carsOnRoad.Add(this);
        }
    }
}
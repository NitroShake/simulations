using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TrafficSim
{
    internal class Car
    {
        enum State
        {
            Driving,
            WaitingInLine,
            WaitingForTurn,
            Turning,
            Arrived
        }

        NodePath path;
        int roadIndex = 0;
        double progressAlongRoad;
        Node targetNode;
        double speedMulti = 50;
        double turnProgress = 0;
        State state;
        
        void update(int deltaTime)
        {
            Road road = path.roadsToNode[roadIndex];

            if (state == State.Driving)
            {
                double speed = speedMulti * road.maxSpeed;
                foreach (Car car in road.carsOnRoad)
                {
                    double distance = car.progressAlongRoad;
                    if (distance < 10 && distance > -10)
                    {
                        speed = car.speedMulti * road.maxSpeed;
                    }
                }


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
        }

        void advanceToNextRoad()
        {
            roadIndex++;
            targetNode = path.roadsToNode[roadIndex].getOpposingNode(targetNode);
        }
    }
}
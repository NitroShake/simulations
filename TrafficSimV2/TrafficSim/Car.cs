using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    internal class Car
    {
        enum State
        {
            Driving,
            WaitingInLine,
            WaitingForTurn,
            Turning
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
                    //if next road is a turn
                    if (path.roadsToNode[roadIndex + 1].roadId != road.roadId)
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
                bool canTurn = true;
                foreach(Road connectedRoad in targetNode.roads)
                {
                    if (road != connectedRoad)
                    {
                        foreach (Car car in connectedRoad.carsOnRoad)
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
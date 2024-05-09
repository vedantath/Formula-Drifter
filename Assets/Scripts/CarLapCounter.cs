using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class CarLapCounter : MonoBehaviour
{
    int passedCheckPointNum = 0;
    float timeAtLastPassedCheckPoint = 0;
    int numberofPassedCheckPoints = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 2;
    bool isRaceCompleted = false;
    int carPosition = 0;

    //events
    public event Action<CarLapCounter> OnPassCheckpoint;

    public void SetCarPosition(int position) { carPosition = position; }

    public int GetNumberOfCheckpointsPassed() { return numberofPassedCheckPoints; }

    public float GetTimeAtLastCheckPoint() { return timeAtLastPassedCheckPoint; }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("CheckPoint"))
        {
            if(isRaceCompleted)
                return;

            Checkpoint checkPoint = collider2D.GetComponent<Checkpoint>();
        
            if(passedCheckPointNum+1 == checkPoint.checkPointNum)
            {
                passedCheckPointNum = checkPoint.checkPointNum;
                numberofPassedCheckPoints++;
                timeAtLastPassedCheckPoint = Time.time;

                if(checkPoint.isFinishLine)
                {
                    passedCheckPointNum = 0;
                    lapsCompleted++;

                    if(lapsCompleted >= lapsToComplete)
                        isRaceCompleted = true;
                }

                //Invoke the passed checkpoint event
                OnPassCheckpoint?.Invoke(this);
            }
        }
    }
}

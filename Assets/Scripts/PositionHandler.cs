using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    public CarAI kartAgent;
    
    // Start is called before the first frame update
    void Awake()
    {
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();
    
        foreach (CarLapCounter lapCounters in carLapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;
        }

        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
    }

    void Start()
    {
        //Ask leaderboard handler to update list
        leaderboardUIHandler.UpdateList(carLapCounters);
    }

    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        Debug.Log($"Event: Car {carLapCounter.gameObject.name} passed a checkpoint");

        if(kartAgent!= null)
        {
            kartAgent.AddReward(2f);
            Debug.Log("+2 reward");
        }

        //sort the cars pos. first based on how many checkpoints passed (more=better). Then sort on time (shorter=better)
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckPoint()).ToList();
    
        //Get the cars positon
        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        carLapCounter.SetCarPosition(carPosition);
    
        leaderboardUIHandler.UpdateList(carLapCounters);

        //20:50
    }


}

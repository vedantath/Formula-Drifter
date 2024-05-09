using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{

    public CarAI kartAgent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Barrier"))
        {
            Debug.Log("Hit Barrier");
            kartAgent.AddReward(-1f);
            kartAgent.EndEpisode();
        }
    }
}

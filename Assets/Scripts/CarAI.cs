using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAI : Agent
{
  public CheckpointManager checkpointManager;
  public BarrierManager barrierManager;
  private CarController carController;

  /*void Awake() s
  {
    carController = GetComponent<CarController>();
    checkpointManager = GetComponent<CheckpointManager>();
  }*/

  public override void Initialize()
  {
    carController = GetComponent<CarController>();
    //checkpointManager = GetComponent<CheckpointManager>();
  }


  //called every time it times-out or reach goal
  public override void OnEpisodeBegin()
  {
    //base.OnEpisodeBegin();
    checkpointManager.ResetCheckpoints();
    carController.Respawn();
  }



  #region Edit this region!

  //Collecting extra Information that isn't picked up by the RaycastSensors
  public override void CollectObservations(VectorSensor sensor)
  {
    //Vector between Car and next Checkpoint
    Vector2 diff = checkpointManager.nextCheckPointToReach.transform.position - transform.position;
    sensor.AddObservation(diff/20f); //normalize

    AddReward(0.001f); //Promote faster driving

  }

  //Processing the actions received
  public override void OnActionReceived(ActionBuffers actions)
  {
    var input = actions.ContinuousActions;

    carController.ApplyEngineForce(input[1]);
    carController.ApplySteering(input[0]);
  }
  
  //For manual testing with human input, the actionsOut defined here will be sent to OnActionRecieved
  public override void Heuristic(in ActionBuffers actionsOut)
  {
    var action = actionsOut.ContinuousActions;

    action[0] = Input.GetAxis("Debug Horizontal"); //steering
    action[1] = Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;  //acceleration

  }
    
  #endregion
}

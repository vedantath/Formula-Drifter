using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using TMPro;
using System;
using Fusion;


public class CarController : MonoBehaviour //NetworkBehavior
{

    public SpawnPointManager _spawnPointManager;

    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 10;

    public CarAI kartAgent;

    //local vars
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;
    //public TextMeshProUGUI speedometerText;

    //Components
    Rigidbody2D carRigidbody2D;

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate() //offline car
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    //frame-rate indep. for physics calculation but networked
    /*public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            steeringInput = data.direction.x;
            accelerationInput = data.direction.y;
        }

        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }*/

    void ApplyEngineForce()
    {
        //Calc how much "forward" velocity
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        //Limit w/ max speed
        if(velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        if(velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        if(carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        //apply drag
        if(accelerationInput == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else {carRigidbody2D.drag = 0; }

        //create force for engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        //apply foce and push car forward
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    public void ApplyEngineForce(float input)
    {
        accelerationInput = input;

        //Calc how much "forward" velocity
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        //Limit w/ max speed
        if(velocityVsUp > maxSpeed && input > 0)
            return;

        if(velocityVsUp < -maxSpeed * 0.5f && input < 0)
            return;

        if(carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && input > 0)
            return;

        //apply drag
        if(input == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else {carRigidbody2D.drag = 0; }

        //create force for engine
        Vector2 engineForceVector = transform.up * input * accelerationFactor;

        //apply foce and push car forward
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        float minSpeedBeforeAllowTurnFactor = (carRigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurnFactor = Mathf.Clamp01(minSpeedBeforeAllowTurnFactor);

        //update rot angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurnFactor;

        //apply steering by rotating car obj
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    //override ai input
    public void ApplySteering(float input)
    {
        float minSpeedBeforeAllowTurnFactor = (carRigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurnFactor = Mathf.Clamp01(minSpeedBeforeAllowTurnFactor);

        //update rot angle based on input
        rotationAngle -= input * turnFactor * minSpeedBeforeAllowTurnFactor;

        //apply steering by rotating car obj
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    float getLateralVelocity() { return Vector2.Dot(transform.right, carRigidbody2D.velocity); }

    public bool isTireDrifting(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = getLateralVelocity();
        isBraking = false;

        if(accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if(Mathf.Abs(getLateralVelocity()) > 4.0f)
            return true;
        
        return false;

    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = getLateralVelocity();
        isBraking = false;

        if(accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if(Math.Abs(getLateralVelocity()) > 4.0f)
            return true;

        return false;
    }

    public void Respawn()
    {
      Vector2 pos = _spawnPointManager.SelectRandomSpawnpoint();
      carRigidbody2D.MovePosition(pos);
      transform.position = pos - new Vector2(0, 0);
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody2D.velocity.magnitude;
    }


    float calculuateSpeed() { return carRigidbody2D.velocity.magnitude*2.237f; }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(kartAgent!=null)
        {
            Debug.Log("Fail->Respawn");
            kartAgent.AddReward(-1f);
            kartAgent.EndEpisode();
        }
    }


}

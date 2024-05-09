using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    CarController carController;
    public int playerNum;

    Vector2 inputVector = Vector2.zero;

    void Awake()
    {
        carController = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        inputVector = Vector2.zero;

        switch(playerNum)
        {
            case 1:
                inputVector.x = Input.GetAxis("Horizontal_P1");
                inputVector.y = Input.GetAxis("Vertical_P1");
                break;
            
            case 2:
                inputVector.x = Input.GetAxis("Horizontal_P2");
                inputVector.y = Input.GetAxis("Vertical_P2");
                break;

            /*case 3:
                inputVector.x = Input.GetAxis("Horizontal_P3");
                inputVector.y = Input.GetAxis("Vertical_P3");
                break;

            case 4:
                inputVector.x = Input.GetAxis("Horizontal_P4");
                inputVector.y = Input.GetAxis("Vertical_P4");
                break;*/
        }

       /* if(playerNum == 1) //Red Car uses WASD
        {
            inputVector.x = Input.GetAxis("Horizontal");
            inputVector.y = Input.GetAxis("Vertical");
        }

        if(playerNum == 2)  //Blue car uses arrow keys
        {
            inputVector.x = Input.GetAxis("Debug Horizontal");
            inputVector.y = Input.GetAxis("Debug Vertical");
        }*/

        
        carController.SetInputVector(inputVector);
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.direction = inputVector;

        return networkInputData;
    }
}

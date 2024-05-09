using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.UI;

public class SelectCarUIHandler : MonoBehaviour
{
    [Header("Car prefab")]
    public GameObject carPrefab;

    [Header("Spawn on")]
    public Transform spawnOnTransform;

   // public Button LButton;
   // public Button RButton;

    bool isChangingCar = false;
    CarUIHandler carUIHandler = null;
    


    // Start is called before the first frame update
    void Start()
    {
//        LButton.onClick.AddListener(TaskOnLeftClick);
        //RButton.onClick.AddListener(TaskOnRightClick);
        StartCoroutine(SpawnCarCO(true));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow) && !isChangingCar)
        {
            StartCoroutine(SpawnCarCO(true));
        }
        if(Input.GetKey(KeyCode.RightArrow) && !isChangingCar)
        {
            
        }
    }

    //Button OnClick Listeners
   /* void TaskOnLeftClick(){
		StartCoroutine(SpawnCarCO(true));
	}
    void TaskOnRightClick(){
		StartCoroutine(SpawnCarCO(true));
	}*/

    IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide) //Coroutine
    {
        isChangingCar = true;

        if(carUIHandler != null)
           carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);

        GameObject instantiatedCar = Instantiate(carPrefab, spawnOnTransform);
        carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();
        carUIHandler.StartCarEntranceAnimation(isCarAppearingOnRightSide);

        yield return new WaitForSeconds(0.2f);

        isChangingCar = false;
    }
}

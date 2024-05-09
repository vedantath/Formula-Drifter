using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUIHandler : MonoBehaviour
{
    Animator animator = null;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }

    public void StartCarEntranceAnimation(bool isAppearingOnRightSide)
    {
        if(isAppearingOnRightSide)
            animator.Play("Car Appear From Right");
        else
            animator.Play("Car Appear From Left");
    }

    public void StartCarExitAnimation(bool isExitingOnRightSide)
    {
        if(isExitingOnRightSide)
            animator.Play("Car Leave To Right");
        else
            animator.Play("Car Leave To Left");
    }

    //Events
    public void OnCarExitAnimationCompleted()
    {
        Destroy(gameObject);
    }

}

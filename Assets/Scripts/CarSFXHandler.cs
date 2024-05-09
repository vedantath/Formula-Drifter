using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSFXHandler : MonoBehaviour
{
   // [Header("Mixers")]
    //public AudioMixer audioMixer;

    [Header("Audio sources")]
    public AudioSource tireScreechingAudioSource;
    public AudioSource engineAudioSource;
    public AudioSource crashAudioSource;

    CarController carController;

    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
       //audioMixer.SetFloat("SFXVolume", 0.5f);     
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTireScreechSFX();
    }

    void UpdateEngineSFX()
    {
        float velocityMagnitude = carController.GetVelocityMagnitude();

        //Increase engine volume as car goes faster
        float desiredEngineVolume = velocityMagnitude*.05f;
        
        //Keep min level (engine idle)
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);
        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime*10);

        desiredEnginePitch = velocityMagnitude*0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 1.5f);  //max default (2f)
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime*1.5f);
    }

    void UpdateTireScreechSFX()
    {
        if(carController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if(isBraking)
            {
                tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 1.0f, Time.deltaTime*10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime*10);
            }
            else
            {
                tireScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        //Fade out SFX if not screeching
        else
            tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 0, Time.deltaTime*10);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        float relativeVelocity = collision2D.relativeVelocity.magnitude;
        float volume = relativeVelocity*0.1f;

        crashAudioSource.pitch = Random.Range(0.95f, 1.05f);
        crashAudioSource.volume = volume;

        if(!crashAudioSource.isPlaying)
            crashAudioSource.Play();
    }
}

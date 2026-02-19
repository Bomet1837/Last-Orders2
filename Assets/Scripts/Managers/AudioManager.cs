using System;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject emitterOrigin; 
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public StudioEventEmitter ChangeEmitterEvent(EventReference fmodEvent)
    {
        StudioEventEmitter emitter = emitterOrigin.GetComponent<StudioEventEmitter>();
        emitter.Stop();
        
        StudioEventEmitter newEmitter = emitterOrigin.AddComponent<StudioEventEmitter>();
        newEmitter.EventPlayTrigger = EmitterGameEvent.ObjectStart;
        newEmitter.EventReference = fmodEvent;
        
        Destroy(emitter);

        return newEmitter;
    }
}

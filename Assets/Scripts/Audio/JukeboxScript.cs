using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using Random = UnityEngine.Random;

public class JukeboxScript : MonoBehaviour, IInteractable
{
    public EventReference[] jbMusicRefs;
    public StudioEventEmitter emitter;

  void Awake()
  {
      emitter.EventReference = jbMusicRefs[Random.Range(0, jbMusicRefs.Length)];
  }

  public void Interact()
    {
        if (emitter.IsPlaying())
        {
            int i = Random.Range(0, jbMusicRefs.Length);
            
            emitter = AudioManager.Instance.ChangeEmitterEvent(jbMusicRefs[i]);
        }
        else
        {
            emitter.Play();
        }
    }
    
    
    
}

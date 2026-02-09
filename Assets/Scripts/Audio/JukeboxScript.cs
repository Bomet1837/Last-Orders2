using System.Collections;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class JukeboxScript : MonoBehaviour, ICanInteract
{
    public EventReference[] jbMusicRefs;
    public StudioEventEmitter emitter;

  void Awake()
  {
      emitter.EventReference = jbMusicRefs[Random.Range(0, jbMusicRefs.Length)];
  }

  public void Interact(RaycastHit hit)
    {
        if (emitter.IsPlaying())
        {
            emitter.AllowFadeout = true;
            emitter.EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            
            emitter.EventReference = jbMusicRefs[Random.Range(0, jbMusicRefs.Length)];
            emitter.Play();
            
        }
        else
        {
            
            emitter.Play();
        }
    }
    
    
    
}

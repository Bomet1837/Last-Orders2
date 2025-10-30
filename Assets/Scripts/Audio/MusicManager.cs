using UnityEngine;
using FMOD;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    public EventReference musicEvent;

    private FMOD.Studio.EventInstance musicInstance;

    private void Start()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }
}

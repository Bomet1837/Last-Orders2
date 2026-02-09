using UnityEngine;
using FMOD;
using FMODUnity;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SFXManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance SFXinst_footsteps;
    private float footstepDelay = 0.5f;
    private float footstepTimer = 0f;

    private void Awake()
    {
        SFXinst_footsteps = RuntimeManager.CreateInstance("event:/EventsMain/SFX/SFX_Footsteps");
    }

    private void Update()
    {
        PlayFootsteps();
    }

    private void PlayUI()
    {
        
    }

    private void PlayFootsteps()
    {
        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && PlayerManager.Grounded)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepDelay)
            {
                SFXinst_footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/EventsMain/SFX/SFX_Footsteps");
                SFXinst_footsteps.setVolume(10);
                SFXinst_footsteps.start();
                UnityEngine.Debug.Log("Playing Footstep Sound");
                SFXinst_footsteps.release();
                footstepTimer = 0f;
            }

        }
        else
        {
            footstepTimer = 0f;
        }
    }
}

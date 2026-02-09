using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class UI_FX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData enterPed)
    {
        RuntimeManager.PlayOneShot("event:/EventsMain/SFX/UI/sfx_guiselect");
    }
    
    public void OnPointerDown(PointerEventData downPed)
    {
        RuntimeManager.PlayOneShot("event:/EventsMain/SFX/UI/sfx_guiconfirm");
    }
    
}

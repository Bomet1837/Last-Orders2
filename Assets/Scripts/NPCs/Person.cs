using System;
using Cinemachine;
using TMPro;
using UnityEngine;

public class Person : MonoBehaviour, IInteractable
{
    public GameObject cam;
    public CharacterScript dialogue;
    public Transform stoolPoint;
    public bool navigatingToBar;
    public bool navigatingToStoolNavPoint;
    public bool navigatingToStool;

    int _index;
    
    
    public void SetStool(Transform stool)
    {
        stoolPoint = stool;
        navigatingToBar = true;
    }
    
    void Awake()
    {
        //cam = transform.GetChild(0).gameObject;
    }

    void Update()
    {

        if (navigatingToBar)
        {
            Transform currentCornerNavPoint = SpawnManager.instance.cornerNavPointsArray[_index];
        
            MoveTo(currentCornerNavPoint.position);

            Vector3 xyPosition = transform.position;
            xyPosition.y = 0;
            
            Vector3 xyTargetPosition = currentCornerNavPoint.position;
            xyTargetPosition.y = 0;

            Vector3 direction = stoolPoint.GetChild(0).GetChild(0).position - transform.position;
            
            Ray ray = new Ray(transform.position, direction.normalized);
            
            Debug.DrawRay(transform.position,direction.normalized * Vector3.Distance(transform.position, stoolPoint.position));
            
            if (Vector3.Distance(xyPosition,xyTargetPosition) > 0.05f) return;
            
            //Check if we hit anything, cause if we do we need to go to the next index, otherwise we may proceed to the stool
            if (Physics.Raycast(ray,out RaycastHit hit, Vector3.Distance(transform.position, stoolPoint.GetChild(0).GetChild(0).position)))
            {
                Debug.Log(hit.transform.gameObject.name);
                _index++;
            }
            else
            {
                navigatingToStoolNavPoint = true;
                navigatingToBar = false;
            }
        }

        if (navigatingToStoolNavPoint)
        {
            Transform stoolNavPoint = stoolPoint.GetChild(0).GetChild(0);
            
            MoveTo(stoolNavPoint.position);
            
            Vector3 xyPosition = transform.position;
            xyPosition.y = 0;
            
            Vector3 xyTargetPosition = stoolNavPoint.position;
            xyTargetPosition.y = 0;

            if (Vector3.Distance(xyPosition, xyTargetPosition) > 0.05f) return;
            
            navigatingToStoolNavPoint = false;
            navigatingToStool = true;   
        }

        if (navigatingToStool)
        {
            MoveTo(stoolPoint.position);
        }
    }

    void MoveTo(Vector3 destination)
    {
        destination.y = 0;
        
        Vector3 position = transform.position;
        position.y = 0;
        
        Vector3 changeVector = (destination - position).normalized;
        

        float distance = Vector3.Distance(position, destination);
        
        changeVector *= Time.deltaTime;
        
        if (changeVector.magnitude > distance)
        {
            transform.position = destination;
            return;
        }

        transform.position += changeVector;
    }
    
    public void Interact()
    {
        if(!PlayerManager.InBar) return;
        
        PlayerManager.LastInteractedPerson = this;
        PlayerManager.CharacterController.enabled = false;

        DialogueManager.Instance.currentCharacterScript = dialogue;
        DialogueManager.Instance.ShowText();
        
        cam.SetActive(true);
        UIManager.Instance.customerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
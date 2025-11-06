using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Person : MonoBehaviour, IInteractable
{
    public GameObject cam;
    public CharacterScript dialogue;
    public Stool stool;
    public bool navigatingToBar;
    public bool navigatingToStoolNavPoint;
    public bool navigatingToStool;
    public bool drinkOrdered;
    public string endText;
    public Drink Drink;
    
    int _index;
    
    
    public void SetStool(Stool stool)
    {
        stool.occupied = true;
        this.stool = stool;
        navigatingToBar = true;
    }
    
    void Awake()
    {
        cam = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        SetRandomDrink();
        if(dialogue == null) endText = DialogueManager.Instance.DialogueDict[UseRandomText()];
    }

    void SetRandomDrink()
    {
        int randomInt = Mathf.RoundToInt(Random.Range(0, DrinkManager.Recipes.Length));
        Drink = new Drink(DrinkManager.Recipes[randomInt].name);
    }

    string UseRandomText()
    {
        return $"generic_order_closed_{Mathf.RoundToInt(Random.Range(0,9))}";
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

            Vector3 direction = stool.transform.GetChild(0).GetChild(0).position - transform.position;
            
            Ray ray = new Ray(transform.position, direction.normalized);
            
            Debug.DrawRay(transform.position,direction.normalized * Vector3.Distance(transform.position, stool.transform.position));
            
            if (Vector3.Distance(xyPosition,xyTargetPosition) > 0.05f) return;
            
            //Check if we hit anything, cause if we do we need to go to the next index, otherwise we may proceed to the stool
            if (Physics.Raycast(ray,out RaycastHit hit, Vector3.Distance(transform.position, stool.transform.GetChild(0).GetChild(0).position)))
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
            Transform stoolNavPoint = stool.transform.GetChild(0).GetChild(0);
            
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
            MoveTo(stool.transform.position);
        }
    }

    void MoveTo(Vector3 destination)
    {
        destination.y = 0;
        
        Vector3 position = transform.position;
        position.y = 0;
        
        Vector3 changeVector = (destination - position).normalized;
        

        float distance = Vector3.Distance(position, destination);
        float yPosition = transform.position.y;
        
        changeVector *= Time.deltaTime;
        
        if (changeVector.magnitude > distance)
        {
            destination.y = yPosition;
            transform.position = destination;
            return;
        }

        transform.position += changeVector;
    }
    
    public void Interact()
    {
        if(!PlayerManager.InBar) return;

        if (drinkOrdered)
        {
            DialogueManager.Instance.ForceSetText(endText);
        }

        PlayerManager.LastInteractedPerson = this;
        PlayerManager.CharacterController.enabled = false;
        
        if (dialogue == null)
        {
            DialogueManager.Instance.ForceSetCharacter("Phoebe");
            DialogueManager.Instance.ForceSetText(endText);
            
            cam.SetActive(true);
            UIManager.Instance.customerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        DialogueManager.Instance.currentCharacterScript = dialogue;
        DialogueManager.Instance.ShowText();
        
        cam.SetActive(true);
        UIManager.Instance.customerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
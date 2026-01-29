using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Person : MonoBehaviour, IInteractable
{
    public GameObject cam;
    public CharacterScript dialogue;
    public Stool stool;
    public bool animationEnabled = true;
    public Drink Drink;
    public string characterName;
    public int dialogueIndex;

    public Vector3 spawnOffset;


    NavMeshAgent _navMeshAgent;
    Animator _animator;
    float _currentSpeed;
    Vector3 _lastPosition;
    int _index;
    readonly string[] _randomNames = {"Ava", "Liam", "Noah", "Emma", "Olivia", "Sophia", "Isabella", "Mason", "Ethan", "Logan", "Harper", "Elijah", "Amelia", "James", "Benjamin", "Lucas", "Charlotte", "Henry", "Alexander", "Mia" };
    
    public void SetStool(Stool stool)
    {
        stool.occupied = true;
        this.stool = stool;
    }
    
    void Awake()
    {
        cam = transform.GetChild(0).gameObject;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
    }

    void Start()
    {
        SetRandomDrink();
        DialogueManager.Instance.CharacterList.Add(gameObject.GetInstanceID() ,this);
        if (characterName == "") SetRandomName();
        else DialogueManager.Instance.Characters.Add(characterName.ToLower(), this);
        _animator = GetComponentInChildren<Animator>();
        if (_animator == null) animationEnabled = false;
    }

    void SetRandomName()
    {
        characterName = _randomNames[Mathf.RoundToInt(Random.Range(0, _randomNames.Length - 1))];
    }
    
    void SetRandomDrink()
    {
        int randomInt = Mathf.RoundToInt(Random.Range(0, DrinkManager.Recipes.Length));
        Drink = new Drink(DrinkManager.Recipes[randomInt].name);
    }

    public void SwapToCamera()
    {
        cam.SetActive(false);
        cam.SetActive(true);
    }
    
    void Update()
    {
        if (animationEnabled)
        {
            _animator.SetFloat("Speed",  1);
            if (_navMeshAgent.isStopped)
            {
                _animator.SetFloat("Speed", 0);
                GetComponent<NavMeshObstacle>().enabled = true;
            }
        }
        
        RotateTowardsDestination(transform.position + _navMeshAgent.velocity);
     
        Transform stoolNavPoint = stool.transform.GetChild(0);

        _navMeshAgent.SetDestination(stoolNavPoint.position);

        if (Vector3.Distance(transform.position, stool.transform.GetChild(0).position) < 0.05f)
        {
            if (animationEnabled)
            {
                _animator.SetFloat("Speed", 0);
                _animator.SetBool("Sitting", true);
            }
            RotateTowardsBar();
        }
        
        _lastPosition = transform.position;
    }


    void RotateTowardsDestination(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0; // keep rotation flat

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    void RotateTowardsBar()
    {
        Vector3 direction = -stool.transform.up;
        direction.y = 0; 

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }
    
    void MoveTo(Vector3 destination)
    {
        Vector3 position = transform.position;
        
        Vector3 changeVector = (destination - position).normalized;
        if(animationEnabled) _animator.SetFloat("Speed", 1);

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
        //if(!PlayerManager.InBar) return;

        PlayerManager.LastInteractedPerson = this;
        PlayerManager.FirstPersonController.enabled = false;

        DialogueManager.Instance.currentCharacterScript = dialogue;
        DialogueManager.Instance.ShowText();
        
        cam.SetActive(true);
        UIManager.Instance.customerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
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
    public PersonType mutantType = PersonType.Human; 
    public Vector3 spawnOffset;


    public NavMeshAgent navMeshAgent;
    public Animator animator;


    IState _currentState;
    float _currentSpeed;
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
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }

    void Start()
    {
        SetRandomDrink();
        
        if (characterName == "") SetRandomName();
        else DialogueManager.Instance.Characters.Add(characterName.ToLower(), this);
        
        animator = GetComponentInChildren<Animator>();
        if (animator == null) animationEnabled = false;
        
        SwitchStates(new MoveToState(stool.transform.GetChild(0).position, new Sitting()));
    }   

    void SetRandomName()
    {
        characterName = _randomNames[Mathf.RoundToInt(Random.Range(0, _randomNames.Length - 1))];
    }
    
    void SetRandomDrink()
    {
        int randomInt = Mathf.RoundToInt(Random.Range(0, DrinkManager.Recipes.Length));
        CocktailRecipe recipe = DrinkManager.Recipes[randomInt];
        Drink = new Drink(recipe.name, recipe.effects );
    }

    public void SwapToCamera()
    {
        cam.SetActive(false);
        cam.SetActive(true);
    }
    
    void Update()
    {
        _currentState.Update(this);
    }

    public void kill()
    {
        DialogueManager.Instance.Characters.Remove(characterName.ToLower());
        Destroy(gameObject);
    }
    
    public void SwitchStates(IState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    public void RotateTowardsDestination(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0; // keep rotation flat

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    public void RotateTowardsBar()
    {
        Vector3 direction = -stool.transform.up;
        direction.y = 0; 

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }
    
    public void Interact()
    {
        //if(!PlayerManager.InBar) return;

        PlayerManager.LastInteractedPerson = this;
        PlayerManager.FirstPersonController.enabled = false;
        PlayerManager.PlayerLook.enabled = false;

        DialogueManager.Instance.currentCharacterScript = dialogue;
        DialogueManager.Instance.ShowText();
        
        cam.SetActive(true);
        UIManager.Instance.customerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}

public enum PersonType {
    Human,
    Radiopath,
    Chugger,
    Spiller,
    Seer,
    Leaver
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using Debug = FMOD.Debug;

public class FMODParamManager : MonoBehaviour
{
    FMOD.Studio.System audSys;
    public static FMODParamManager Instance;
    
    [SerializeField] private Person[] _npcs;
    
    private float BusynessPercentageCalc(int current, int max)
    {
        float busynessAdded = (float)current / max * 8f;
        return busynessAdded;
    }
    
    private void Awake()
    {
        audSys = FMODUnity.RuntimeManager.StudioSystem;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        audSys.setParameterByName("Busyness", 0f);
    }

    private void Update()
    {
        _npcs = FindObjectsByType<Person>(FindObjectsSortMode.None);
        List<Person> npcs = _npcs.ToList();

        float busynessTotal = 0f;
        
        foreach (var npc in npcs)
        {
            float busyness = BusynessPercentageCalc(1, 10);
            
            // incrementing value's making music sound too flat, need to convert to while statement or coroutine for fading musical elements possibly
            busynessTotal += busyness;
        }
        
        audSys.setParameterByName("Busyness", busynessTotal);
        

        
    }
    
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    List<Stool> _stools = new List<Stool>();
    
    [SerializeField] GameObject stools;
    [SerializeField] SpawnGuide spawnGuide;
    [SerializeField] GameObject[] tableStools;

    public Transform[] cornerNavPointsArray;
    public static SpawnManager instance;

    bool _spawned;

    bool _specialSpawned;
    SpawnWave _currentSpawnWave;
    Dictionary<TimeSpan, SpecialSpawnWave> _specialSpawns = new Dictionary<TimeSpan, SpecialSpawnWave>();
    Queue<SpecialSpawn> _specialSpawnQueue = new Queue<SpecialSpawn>();

    void Awake()
    {
        if(instance != null) Debug.LogError("Multiple spawn managers! There should only be one!");
        instance = this;
        
        GetCornerNavPoints();
        GetStools();

        foreach (SpecialSpawnWave spawn in spawnGuide.specialSpawns)
        {
            TimeSpan min = new TimeSpan(spawn.startHour,spawn.startMinute,0);
            TimeSpan max = new TimeSpan(spawn.endHour,spawn.endMinute,0);
            TimeSpan randomRange = GetRandomRange(min, max);
            
            //Make sure we don't accidentally overwrite another character spawning by just generating a new random range;
            while (_specialSpawns.ContainsKey(randomRange))
            {
                randomRange = GetRandomRange(min, max);
            }
            
            Debug.Log(randomRange.ToString());
            
            _specialSpawns.Add(randomRange, spawn);
        }
    }

    void Start()
    {
        TimeManager.OnMinuteChange += () => _specialSpawned = false;
        TimeManager.OnMinuteChange += SetSpawnwave;
        SetSpawnwave();
    }

    void GetCornerNavPoints()
    {
        List<Transform> cornerNavPoints = new List<Transform>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            cornerNavPoints.Add(transform.GetChild(i));
        }
        
        cornerNavPointsArray = cornerNavPoints.ToArray();
    }

    void GetStools()
    {
        for (int i = 0; i < stools.transform.childCount; i++)
        {
            _stools.Add(stools.transform.GetChild(i).GetComponent<Stool>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan currentTime = new TimeSpan(TimeManager.Instance.currentTime.Hours, TimeManager.Instance.currentTime.Minutes, 0);
        
        
        //Special spawn logic
        if (_specialSpawns.ContainsKey(currentTime))
        {
            if (_specialSpawned) return;
            _specialSpawned = true;
            
            Debug.Log(_specialSpawns.Count);
            foreach (SpecialSpawn specialSpawn in _specialSpawns[currentTime].characters)
            {
                _specialSpawnQueue.Enqueue(specialSpawn);
            }
        }

        
        if (_specialSpawnQueue.Count > 0)
        {
            SpecialSpawn characterToSpawn = _specialSpawnQueue.Peek();
            Spawn(characterToSpawn.Character, characterToSpawn.GoToBooth);
            _specialSpawnQueue.Dequeue();
        }
        
        
        //Generic spawn logic
        int remainder = TimeManager.Instance.currentTime.Minutes % _currentSpawnWave.genericSpawnInterval;
        
        if (remainder == 0)
        {
            if(_spawned) return;
            if(spawnGuide.genericSpawns.Length == 0) return;
            _spawned = true;

            int randomInt = Mathf.RoundToInt(Random.Range(0, spawnGuide.genericSpawns.Length));
            Spawn(spawnGuide.genericSpawns[randomInt], false);
        }
        else _spawned = false;
    }

    void Spawn(GameObject objectToSpawn, bool goToBooth)
    {
        //transform.position + offset, transform.rotation
        GameObject spawnedPerson = Instantiate(objectToSpawn);
        Person person = spawnedPerson.GetComponent<Person>();

        spawnedPerson.transform.position = transform.position + person.spawnOffset;
        spawnedPerson.transform.rotation = transform.rotation;
        
        if(goToBooth) person.SetStool(GetUnoccupiedTableStool());
        else person.SetStool(GetUnoccupiedStool());
    }

    //Called on minute change
    void SetSpawnwave()
    {
        foreach (SpawnWave spawnWave in spawnGuide.spawnWaves)
        {
            if (TimeManager.Instance.currentTime.Minutes == spawnWave.startMinute) _currentSpawnWave = spawnWave;
        }
    }

    TimeSpan GetRandomRange(TimeSpan min, TimeSpan max)
    {
        //Unity doesn't do random longs :D
        System.Random sysRand = new System.Random();
        double randomNumber = sysRand.NextDouble();

        long randomTicks = min.Ticks + (long)(randomNumber * (max.Ticks - min.Ticks));
        TimeSpan randomizedSpan = TimeSpan.FromTicks(randomTicks);

        randomizedSpan = new TimeSpan(randomizedSpan.Hours, randomizedSpan.Minutes, 0);
        
        return randomizedSpan;
    }

    Stool GetUnoccupiedTableStool()
    {
        foreach (GameObject stool in tableStools)
        {
            Stool stoolScript = stool.GetComponent<Stool>();
            if (!stoolScript.occupied) return stoolScript;
        }
        
        Debug.LogError("No free table stools!");
        return null;
    }
    
    Stool GetUnoccupiedStool()
    {
        foreach (Stool stool in _stools)
        {
            if (!stool.occupied) return stool;
        }
        
        Debug.LogError("No free stools!");
        SceneManager.LoadScene(0);
        return null;
    }
}

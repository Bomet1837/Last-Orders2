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

    public Transform[] cornerNavPointsArray;
    public static SpawnManager instance;

    bool _spawned;
    bool _specialSpawned;
    
    SpawnWave _currentSpawnWave;
    Dictionary<TimeSpan, GameObject[]> _specialSpawns = new Dictionary<TimeSpan, GameObject[]>();

    void Awake()
    {
        if(instance != null) Debug.LogError("Multiple spawn managers! There should only be one!");
        instance = this;
        
        GetCornerNavPoints();
        GetStools();

        foreach (SpecialSpawn spawn in spawnGuide.specialSpawns)
        {
            TimeSpan min = new TimeSpan(spawn.startHour,spawn.startMinute,0);
            TimeSpan max = new TimeSpan(spawn.endHour,spawn.endMinute,0);
            TimeSpan randomRange = GetRandomRange(min, max);

            
            //Make sure we don't accidentally overwrite another character spaaning by just generating a new random range;
            while (_specialSpawns.ContainsKey(randomRange))
            {
                randomRange = GetRandomRange(min, max);
            }

            TimeSpan truncatedTimespan = new TimeSpan(randomRange.Hours, randomRange.Minutes, 0);
            Debug.Log(truncatedTimespan.ToString());
            
            _specialSpawns.Add(truncatedTimespan, spawn.characters);
        }
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
        //This needs to be put in a function that only checks every in game minute change rather than every frame
        foreach (SpawnWave spawnWave in spawnGuide.spawnWaves)
        {
            if (TimeManager.Instance.currentTime.Minutes == spawnWave.startMinute) _currentSpawnWave = spawnWave;
        }

        TimeSpan currentTime = new TimeSpan(TimeManager.Instance.currentTime.Hours, TimeManager.Instance.currentTime.Minutes, 0);
        
        if (_specialSpawns.ContainsKey(currentTime))
        {
            if (_specialSpawned) return;
            _specialSpawned = true;

            foreach (GameObject specialSpawn in _specialSpawns[currentTime])
            {
                Spawn(specialSpawn);
            }
        }
        else _specialSpawned = false;
        
        int remainder = TimeManager.Instance.currentTime.Minutes % _currentSpawnWave.genericSpawnInterval;
        
        if (remainder == 0)
        {
            if(_spawned) return;
            _spawned = true;

            int randomInt = Mathf.RoundToInt(Random.Range(0, spawnGuide.genericSpawns.Length));
            Spawn(spawnGuide.genericSpawns[randomInt]);
        }
        else _spawned = false;
    }

    void Spawn(GameObject objectToSpawn)
    {
        //transform.position + offset, transform.rotation
        GameObject spawnedPerson = Instantiate(objectToSpawn);
        Person person = spawnedPerson.GetComponent<Person>();

        spawnedPerson.transform.position = transform.position + person.spawnOffset;
        spawnedPerson.transform.rotation = transform.rotation;
        
        person.SetStool(GetUnoccupiedStool());
    }

    TimeSpan GetRandomRange(TimeSpan min, TimeSpan max)
    {
        //Unity doesn't do random longs :D
        System.Random sysRand = new System.Random();
        double randomNumber = sysRand.NextDouble();

        long randomTicks = min.Ticks + (long)(randomNumber * (max.Ticks - min.Ticks));
        return TimeSpan.FromTicks(randomTicks);
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

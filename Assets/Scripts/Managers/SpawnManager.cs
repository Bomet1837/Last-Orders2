using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    List<GameObject> _stools = new List<GameObject>();
    
    [SerializeField] GameObject objectToBeSpawned;
    [SerializeField] GameObject stools;

    public Transform[] cornerNavPointsArray;
    public static SpawnManager instance;

    void Awake()
    {
        if(instance != null) Debug.LogError("Multiple spawn managers! There should only be one!");
        instance = this;
        
        GetCornerNavPoints();
        GetStools();
        
        Spawn();
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
            _stools.Add(stools.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        GameObject spawnedPerson = Instantiate(objectToBeSpawned, transform.position, transform.rotation);
        spawnedPerson.GetComponent<Person>().SetStool(_stools[12].transform);
    }
}

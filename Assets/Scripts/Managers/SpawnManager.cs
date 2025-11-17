using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    List<Stool> _stools = new List<Stool>();
    
    [SerializeField] GameObject objectToBeSpawned;
    [SerializeField] GameObject stools;

    public Transform[] cornerNavPointsArray;
    public Vector3 offset;
    public static SpawnManager instance;

    bool spawned;

    void Awake()
    {
        if(instance != null) Debug.LogError("Multiple spawn managers! There should only be one!");
        instance = this;
        
        GetCornerNavPoints();
        GetStools();
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
        if (TimeManager.Instance.currentTime.Minutes == 30 || TimeManager.Instance.currentTime.Minutes == 0)
        {
            if(spawned) return;
            spawned = true;
            Spawn();
        }
        else spawned = false;
    }

    void Spawn()
    {
        GameObject spawnedPerson = Instantiate(objectToBeSpawned, transform.position + offset, transform.rotation);
        spawnedPerson.GetComponent<Person>().SetStool(GetUnoccupiedStool());
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

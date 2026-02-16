using UnityEngine;

[CreateAssetMenu(fileName = "SpawnGuide", menuName = "Scriptable Objects/SpawnGuide")]
public class SpawnGuide : ScriptableObject
{
    public GameObject[] genericSpawns;
    public SpawnWave[] spawnWaves;
    public SpecialSpawnWave[] specialSpawns;
}

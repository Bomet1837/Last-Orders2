using UnityEngine;

[System.Serializable]
public class SpecialSpawn
{
   public int startHour;
   public int startMinute;

   public int endHour;
   public int endMinute;
   public bool goToBooth;
   
   public GameObject[] characters;
}

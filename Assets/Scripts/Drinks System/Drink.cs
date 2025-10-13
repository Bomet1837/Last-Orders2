using UnityEngine;

[System.Serializable]
public class Drink : IDrinkable
{
    public string name;
    
    public void Consume()
    {
        throw new System.NotImplementedException();
    }
}
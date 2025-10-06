using UnityEngine;

[System.Serializable]
public class Drink : IDrinkable
{
    protected string name;
    
    public void Consume()
    {
        throw new System.NotImplementedException();
    }
}
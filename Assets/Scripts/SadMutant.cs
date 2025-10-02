using UnityEngine;

public class SadMutant : Mutant
{
    void Start()
    {
        Drink(new UnhappyDrink());
    }

    public override void Drink<UnhappyDrink>(UnhappyDrink drink)
    {
        Debug.Log("Wow I hate life!");
    }
}

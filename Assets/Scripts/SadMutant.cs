using System;
using UnityEngine;

public class SadMutant : Mutant
{

    public override void Drink<TUnhappyDrink>(TUnhappyDrink drink)
    {
        if (drink.name == "Despreso") SetText("Wow I hate life!");
        else SetText("Mmm delicious!");
    }
}

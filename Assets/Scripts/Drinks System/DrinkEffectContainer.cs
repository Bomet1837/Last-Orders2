using UnityEngine;

public abstract class DrinkEffectContainer : ScriptableObject ,IDrinkEffect
{
    public abstract void Activate(Person drinker);
}

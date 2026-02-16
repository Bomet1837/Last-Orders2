using System;
using UnityEngine;


//I fucking hate unities stupid serialization system man, why can't I just drag and drop classes and have them instantiate? Sincerely, jack at 2 am
[CreateAssetMenu(menuName = "DrinkEffects", fileName = "Leaver Effect")]
public class DrinkLeaverEffect : DrinkEffectContainer
{
    public Vector3 wayOut;
    
    public override void Activate(Person drinker)
    {
        if(drinker.mutantType != PersonType.Leaver) return;
        drinker.SwitchStates(new MoveToState(wayOut, true));
    }
}

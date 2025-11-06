using System;
using UnityEngine;

public class DrinkManager : MonoBehaviour
{

    [SerializeField] CocktailRecipe[] allRecipes;
    public static CocktailRecipe[] Recipes;

    void Awake()
    {
        Recipes = allRecipes;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Header("Cocktail Recipes")]
    [SerializeField] private CocktailRecipe[] allRecipes; // recipe book
    private List<IngredientData> addedIngredients = new List<IngredientData>();
    //keep track of added ingredients


    // Called when an ingredient is added to the shaker
    public void AddIngredient(IngredientData ingredient)
    {
        // Avoid duplicates
        if (!addedIngredients.Contains(ingredient))
        {
            addedIngredients.Add(ingredient);
            Debug.Log($"[Shaker] Added {ingredient.ingredientName}");
        }
        else
        {
            Debug.Log($"[Shaker] {ingredient.ingredientName} already added.");
        }

       
    }

    // Checks if wombo combo of ingredients matches any recipe
    public void ShakeAndCheckCocktail()
    {
        Debug.Log("[Shaker] Shaking..."); //imagine shaking animation here

        foreach (var recipe in allRecipes)
        {
            //check for a match
            if (MatchesRecipe(recipe))
            {
                Debug.Log($" You made a {recipe.cocktailName}!");
                addedIngredients.Clear(); //empty shaker
                return; // exit after finding a match
            }
        }

        Debug.Log("No known recipe. You made a shit drink");
        addedIngredients.Clear();
    }


    // Compares the shaker's current ingredients with a recipe
    private bool MatchesRecipe(CocktailRecipe recipe)
    {
        
       if (recipe.requiredIngredients.Length != addedIngredients.Count)
           return false;

        foreach (var req in recipe.requiredIngredients)
        {
            if (!addedIngredients.Contains(req.ingredient))
                return false;
        }

        return true;
    }
}

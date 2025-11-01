using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Header("Cocktail Recipes")]
    [SerializeField] private CocktailRecipe[] allRecipes; // recipe book
    private List<IngredientData> _addedIngredients = new List<IngredientData>();
    //keep track of added ingredients


    // Called when an ingredient is added to the shaker
    public void AddIngredient(IngredientData ingredient)
    {
        // Avoid duplicates
        if (!_addedIngredients.Contains(ingredient))
        {
            _addedIngredients.Add(ingredient);
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

        foreach (CocktailRecipe recipe in allRecipes)
        {
            //check for a match
            if (MatchesRecipe(recipe))
            {
                Debug.Log($" You made a {recipe.cocktailName}!");
                _addedIngredients.Clear(); //empty shaker
                return; // exit after finding a match
            }
        }

        Debug.Log("No known recipe. You made a shit drink");
        _addedIngredients.Clear();
    }


    // Compares the shaker's current ingredients with a recipe
    // Might want to look at this if we have performance problems.
    private bool MatchesRecipe(CocktailRecipe recipe)
    { 
        if (recipe.requiredIngredients.Length != _addedIngredients.Count)
           return false;
        
        foreach (IngredientRequirement req in recipe.requiredIngredients)
        {
            if (!_addedIngredients.Contains(req.ingredient))
                return false;
        }

        return true;
    }
}

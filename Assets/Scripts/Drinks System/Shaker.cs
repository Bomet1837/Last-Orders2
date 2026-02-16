using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FMODUnity;

public class Shaker : MonoBehaviour, IInteractable
{
    public StudioEventEmitter pour, actionFail, liquidDrip; 
    
    [Header("Cocktail Recipes")]
    private List<IngredientData> _addedIngredients = new List<IngredientData>();
    //keep track of added ingredients

    float _maxTime = 2;
    float _currentTime;

    TMP_Text _text;

    void Start()
    {
        _text = transform.GetChild(1).GetComponent<TMP_Text>();
        _text.enabled = false;
    }

    void Update()
    {
        if (!_text.enabled) return;
        _currentTime += Time.deltaTime;
        
        if (_currentTime < _maxTime) return;
        _text.enabled = false;
        _currentTime = 0f;
    }

    // Called when an ingredient is added to the shaker
    public void AddIngredient(IngredientData ingredient)
    {
        // Avoid duplicates
        if (!_addedIngredients.Contains(ingredient))
        {
            _addedIngredients.Add(ingredient);
            _text.SetText($"Added {ingredient.ingredientName}");
            pour.Play();
        }
        else
        {
            _text.SetText($"{ingredient.ingredientName} already added.");
            actionFail.Play();
        }
        
        _text.enabled = true;
        _currentTime = 0f;
    }

    // Checks if wombo combo of ingredients matches any recipe
    public void ShakeAndCheckCocktail()
    {
        Debug.Log("[Shaker] Shaking..."); //imagine shaking animation here
        liquidDrip.Play();
        

        foreach (CocktailRecipe recipe in DrinkManager.Recipes)
        {
            //check for a match
            if (MatchesRecipe(recipe))
            {
                _text.SetText($" You made a {recipe.cocktailName}. Now serve it!");
                _text.enabled = true;
                _currentTime = 0f;

                PlayerManager.currentDrink = new Drink(recipe.cocktailName);
                
                _addedIngredients.Clear(); //empty shaker
                return; // exit after finding a match
            }
        }

        _text.SetText("No Recipe!");
        actionFail.Play();
        _text.enabled = true;
        _currentTime = 0f;
        
        _addedIngredients.Clear();
    }


    // Compares the shaker's current ingredients with a recipe
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
    public void Interact()
    {
        ShakeAndCheckCocktail();
    }
}

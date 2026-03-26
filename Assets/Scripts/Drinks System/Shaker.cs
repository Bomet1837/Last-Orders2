using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class Shaker : MonoBehaviour, IInteractable
{
    public static Shaker Current;

    public Transform spawnPoint;
    public GameObject failedDrinkPrefab;
    public GameObject defaultDrink;
    public Vector3 drinkSpawnOfsset;

    List<IngredientData> _addedIngredients = new List<IngredientData>();
    CocktailRecipe pendingRecipe;
    bool waiting;

    TMP_Text text;

    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void AddIngredient(IngredientData ingredient)
    {
        if (!_addedIngredients.Contains(ingredient))
        {
            _addedIngredients.Add(ingredient);
            Debug.Log("Added " + ingredient.ingredientName);
        }
    }

    public void ShakeAndCheckCocktail()
    {
        if (waiting) return;

        Debug.Log("Starting minigame");

        pendingRecipe = FindRecipe();
        Current = this;
        waiting = true;

        FishingMinigame_Shaker.Instance.StartMinigame();
        Debug.Log("[Shaker] ShakeAndCheckCocktail called");
    }

    CocktailRecipe FindRecipe()
    {
        foreach (CocktailRecipe recipe in DrinkManager.Recipes)
        {
            if (Matches(recipe))
                return recipe;
        }
        return null;
    }

    bool Matches(CocktailRecipe recipe)
    {
        if (recipe.requiredIngredients.Length != _addedIngredients.Count)
            return false;

        foreach (var req in recipe.requiredIngredients)
            if (!_addedIngredients.Contains(req.ingredient))
                return false;

        return true;
    }

    public void OnMinigameWin()
    {
        if (pendingRecipe != null)
        {
            GameObject drink = Instantiate(defaultDrink, spawnPoint.position, defaultDrink.transform.rotation);
            DrinkObject drinkScript = drink.GetComponent<DrinkObject>();
            drinkScript.contains = pendingRecipe.cocktailName;
            
            drink.transform.position += drinkSpawnOfsset;
            Debug.Log("Correct drink spawned");
        }
        else
        {
            Instantiate(failedDrinkPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Failed drink spawned");
        }

        ResetShaker();
    }

    public void OnMinigameFail()
    {
        Instantiate(failedDrinkPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Failed drink spawned");

        ResetShaker();
    }

    void ResetShaker()
    {
        waiting = false;
        pendingRecipe = null;
        _addedIngredients.Clear();
    }
    public void Interact()
    {
        ShakeAndCheckCocktail();
    }
}
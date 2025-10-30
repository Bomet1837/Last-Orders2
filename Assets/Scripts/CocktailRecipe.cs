using UnityEngine;

[System.Serializable]
public class IngredientRequirement
{
    public IngredientData ingredient; // Ingredient reference
}

[CreateAssetMenu(fileName = "NewCocktail", menuName = "Cocktail/Recipe")]

public class CocktailRecipe : ScriptableObject
{
    public string cocktailName; // e.g. "Margarita"
    public IngredientRequirement[] requiredIngredients; // the list of ingredients
}

using TMPro;
using UnityEngine;

public class Notepad : MonoBehaviour
{
    public GameObject TextList;
    public GameObject textPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateText();
    }

    void GenerateText()
    {
        bool first = true;
        
        foreach (CocktailRecipe recipe in DrinkManager.Recipes)
        {
            string recipeText = recipe.cocktailName + " = ";
            foreach (IngredientRequirement ingredientRequirement in recipe.requiredIngredients)
            {
                if (!first) recipeText += " + ";
                IngredientData ingredientData = ingredientRequirement.ingredient;
                recipeText += ingredientData.ingredientName;
                first = false;
            }

            first = true;

            GameObject textObject = Instantiate(textPrefab, TextList.transform);
            textObject.GetComponent<TMP_Text>().SetText(recipeText);
        }
    }
}

using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private IngredientData ingredientData;

    public IngredientData Ingredient => ingredientData;
}

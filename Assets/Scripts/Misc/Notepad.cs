using TMPro;
using UnityEngine;

public class Notepad : MonoBehaviour, ICanUse,ICanInteract , IPickupable, IDropable
{
    public GameObject Origin { get; set; }
    public ObjectPlaceholder ObjectPlaceholder { get; set; }
    public Transform inspectPoint;
    public Transform holdPoint;
    private bool showing;

    [SerializeField] private TMP_Text text;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectPlaceholder = GetComponent<ObjectPlaceholder>();
        GenerateText();
    }

    public void Interact(RaycastHit hit)
    {
        if (hit.transform.gameObject == Origin)
        {
            PlayerManager.FirstPersonController.Drop();
            OnDrop();
            return;
        }
    }

    void GenerateText()
    {
        bool first = true;
        string recipesText = "";
        
        foreach (CocktailRecipe recipe in DrinkManager.Recipes)
        {
            recipesText += recipe.cocktailName + " = ";
            foreach (IngredientRequirement ingredientRequirement in recipe.requiredIngredients)
            {
                if (!first) recipesText += " + ";
                IngredientData ingredientData = ingredientRequirement.ingredient;
                recipesText += ingredientData.ingredientName;
                first = false;
            }

            first = true;
            recipesText += "\n";
        }
        
        text.SetText(recipesText);
    }

    public void OnPickup()
    {
        ObjectPlaceholder.SetPlaceholder();
    }

    public void OnDrop()
    {
        Origin.GetComponent<ObjectPlaceholder>().UnSetPlaceholder();
        Destroy(gameObject);
    }

    public void Use()
    {
        showing = !showing;
        if (showing)
        {
            transform.SetParent(inspectPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(-90,0,0);
        }
        else
        {
            transform.SetParent(holdPoint);
            transform.localRotation = Quaternion.Euler(0,0,0);   
            transform.localPosition = Vector3.zero;
        }
    }
}

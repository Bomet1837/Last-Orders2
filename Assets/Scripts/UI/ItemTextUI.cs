using TMPro;
using UnityEngine;

public class ItemTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText; // assign in inspector
    [SerializeField] private CanvasGroup canvasGroup; // to fade in/out

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Show(string message)
    {
        itemText.text = message;
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}

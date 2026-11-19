using TMPro;
using UnityEngine;

public class InteractionPromptController : MonoBehaviour
{
    [SerializeField]
    private GameObject promptPanel;

    [SerializeField]
    private TMP_Text promptText;

    private void Start()
    {
        HidePrompt();
    }

    public void ShowPrompt(string message)
    {
        if (promptPanel == null ||
            promptText == null)
        {
            return;
        }

        promptText.text =
            $"[ E ]  {message.ToUpperInvariant()}";

        promptPanel.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }
}
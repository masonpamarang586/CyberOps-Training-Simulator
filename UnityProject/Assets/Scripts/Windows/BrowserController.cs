using TMPro;
using UnityEngine;

public class BrowserController : MonoBehaviour
{
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private TMP_Text resultsText;
    [SerializeField] private GameObject helpPanel;

    private ReconSearchSimulator searchSimulator;

    private void Awake()
    {
        searchSimulator =
            new ReconSearchSimulator();
    }

    private void Start()
    {
        resultsText.text =
            "Enter a search query to begin reconnaissance.";

        searchInput.onSubmit.AddListener(
            HandleSearch
        );
        helpPanel.SetActive(false);
    }

    public void Search()
    {
        HandleSearch(searchInput.text);
    }

    private void HandleSearch(string query)
    {
        resultsText.text =
            searchSimulator.Search(query);

        searchInput.ActivateInputField();
        searchInput.Select();
    }

    public void ToggleHelp()
    {
        helpPanel.SetActive(
            !helpPanel.activeSelf
        );
    }

    private void OnDestroy()
    {
        searchInput.onSubmit.RemoveListener(
            HandleSearch
        );
    }
}
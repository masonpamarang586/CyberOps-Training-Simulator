using TMPro;
using UnityEngine;

public class BrowserController : MonoBehaviour
{
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private TMP_Text resultsText;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject exposedDocumentButton;
    [SerializeField] private GameObject documentPanel;
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private GameObject missionCompletePanel;
    [SerializeField] private TMP_Text missionCompleteText;
    [SerializeField] private string documentObjectiveTarget = "exposed_network_document";
    [SerializeField]
    private ReconSearchProfile searchProfile =
    ReconSearchProfile.PublicExposure;
    private ReconSearchSimulator searchSimulator;

    private void Awake()
    {
        searchSimulator =
            new ReconSearchSimulator(searchProfile);
    }

    private void Start()
    {
        resultsText.text =
            "Enter a search query to begin reconnaissance.";

        searchInput.onSubmit.AddListener(
            HandleSearch
        );
        helpPanel.SetActive(false);
        exposedDocumentButton.SetActive(false);
        documentPanel.SetActive(false);

        missionCompletePanel.SetActive(false);

        if (missionManager != null)
        {
            missionManager.OnMissionCompleted += ShowMissionComplete;
        }
    }

    public void Search()
    {
        HandleSearch(searchInput.text);
    }

    private void HandleSearch(string query)
    {
        string command =
            query.Trim().ToLowerInvariant();

        if (command == "hint")
        {
            resultsText.text =
                missionManager != null
                    ? missionManager.RequestHint()
                    : "Hint system unavailable.";

            searchInput.text = "";
            searchInput.ActivateInputField();
            searchInput.Select();

            return;
        }

        if (command == "score")
        {
            resultsText.text =
                missionManager != null
                    ? missionManager.GetScoreSummary()
                    : "Score system unavailable.";

            searchInput.text = "";
            searchInput.ActivateInputField();
            searchInput.Select();

            return;
        }

        resultsText.text =
            searchSimulator.Search(query);

        exposedDocumentButton.SetActive(
            searchSimulator.RevealsExposedDocument(query)
        );

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

        if (missionManager != null)
        {
            missionManager.OnMissionCompleted -= ShowMissionComplete;
        }
    }

    public void OpenExposedDocument()
    {
        documentPanel.SetActive(true);
    }

    public void CloseDocument()
    {
        documentPanel.SetActive(false);

        if (missionManager != null)
        {
            missionManager.CompleteExternalObjective(
                documentObjectiveTarget
            );
        }
    }

    private void ShowMissionComplete()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        missionCompletePanel.SetActive(true);

        if (missionManager != null)
        {
            missionCompleteText.text =
                missionManager.GetMissionName() +
                " completed.\n\n" +
                missionManager.GetScoreSummary();
        }
    }

    public void CloseMissionComplete()
    {
        missionCompletePanel.SetActive(false);
    }
}
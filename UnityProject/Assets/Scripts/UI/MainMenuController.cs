using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject scenarioSelectPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        scenarioSelectPanel.SetActive(false);
    }

    public void ShowScenarioSelect()
    {
        mainMenuPanel.SetActive(false);
        scenarioSelectPanel.SetActive(true);
    }

    public void LoadSSHScenario()
    {
        SceneManager.LoadScene(
            "Prototype_Workstation"
        );
    }

    public void LoadPasswordRecoveryScenario()
    {
        SceneManager.LoadScene(
            "Scenario_PasswordRecovery"
        );
    }

    public void LoadPublicExposureReconScenario()
    {
        SceneManager.LoadScene(
            "PublicExposureRecon"
        );
    }

    public void LoadExposedServiceAccountScenario()
    {
        SceneManager.LoadScene(
            "Scenario_ExposedServiceAccount"
        );
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
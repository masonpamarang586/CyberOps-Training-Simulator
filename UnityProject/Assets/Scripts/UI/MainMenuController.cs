using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
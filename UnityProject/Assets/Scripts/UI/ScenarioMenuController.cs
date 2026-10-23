using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScenarioMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenuContent;
    [SerializeField] private GameObject objectivesPanel;

    public bool IsMenuOpen => pausePanel.activeSelf;
    public bool IsObjectivesOpen =>
        objectivesPanel != null &&
        objectivesPanel.activeSelf;

    public event Action MenuClosed;

    private void Start()
    {
        pausePanel.SetActive(false);

        if (pauseMenuContent != null)
        {
            pauseMenuContent.SetActive(true);
        }

        if (objectivesPanel != null)
        {
            objectivesPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void OpenMenu()
    {
        if (IsMenuOpen)
        {
            return;
        }

        pausePanel.SetActive(true);
        pauseMenuContent.SetActive(true);
        objectivesPanel.SetActive(false);

        Time.timeScale = 0f;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ResumeGame()
    {
        if (!IsMenuOpen)
        {
            return;
        }

        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        MenuClosed?.Invoke();
    }

    public void ShowObjectives()
    {
        pauseMenuContent.SetActive(false);
        objectivesPanel.SetActive(true);
    }

    public void ReturnToPauseMenu()
    {
        objectivesPanel.SetActive(false);
        pauseMenuContent.SetActive(true);
    }

    public void HandleEscape()
    {
        if (IsObjectivesOpen)
        {
            ReturnToPauseMenu();
            return;
        }

        ResumeGame();
    }

    public void RestartScenario()
    {
        Time.timeScale = 1f;

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.name
        );
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            "MainMenu"
        );
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
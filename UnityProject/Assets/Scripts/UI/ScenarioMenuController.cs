using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Readme;

public class ScenarioMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public bool IsMenuOpen => pausePanel.activeSelf;

    public event Action MenuClosed;

    private void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenMenu()
    {
        if (IsMenuOpen)
        {
            return;
        }

        pausePanel.SetActive(true);
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
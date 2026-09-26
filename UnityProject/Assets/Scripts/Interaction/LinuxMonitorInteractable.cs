using UnityEngine;

public class LinuxMonitorInteractable : MonoBehaviour
{
    [SerializeField] private GameObject terminalPanel;
    [SerializeField] private TerminalController terminalController;

    public bool IsFocused { get; private set; }

    private void Start()
    {
        terminalPanel.SetActive(false);
    }

    public void EnterFocus()
    {
        IsFocused = true;

        terminalPanel.SetActive(true);
        terminalController.FocusInput();
    }

    public void ExitFocus()
    {
        IsFocused = false;

        terminalPanel.SetActive(false);
    }
}

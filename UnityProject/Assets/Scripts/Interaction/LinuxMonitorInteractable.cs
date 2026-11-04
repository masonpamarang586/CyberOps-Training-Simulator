using UnityEngine;

public class LinuxMonitorInteractable : MonitorInteractable
{
    [SerializeField] private GameObject terminalPanel;
    [SerializeField] private TerminalController terminalController;

    private void Start()
    {
        terminalPanel.SetActive(false);
    }

    public override void EnterFocus()
    {
        IsFocused = true;

        terminalPanel.SetActive(true);
        terminalController.FocusInput();
    }

    public override void ExitFocus()
    {
        IsFocused = false;

        terminalPanel.SetActive(false);
    }
}
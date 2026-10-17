using UnityEngine;

public class WindowsMonitorInteractable : MonitorInteractable
{
    [SerializeField] private GameObject windowsPanel;

    private void Start()
    {
        windowsPanel.SetActive(false);
    }

    public override void EnterFocus()
    {
        IsFocused = true;
        windowsPanel.SetActive(true);
    }

    public override void ExitFocus()
    {
        IsFocused = false;
        windowsPanel.SetActive(false);
    }
}
using UnityEngine;

public class PhoneInteractable : DeskInteractable
{
    [SerializeField] private GameObject phoneMessagePanel;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private PhoneNotificationController notificationController;
    private void Start()
    {
        phoneMessagePanel.SetActive(false);
    }

    public override void EnterFocus()
    {
        IsFocused = true;

        phoneMessagePanel.SetActive(true);

        if (phoneController != null)
        {
            phoneController.RefreshMessage();
        }

        if (notificationController != null)
        {
            notificationController.MarkAsRead();
        }
    }

    public override void ExitFocus()
    {
        IsFocused = false;
        phoneMessagePanel.SetActive(false);
    }
}
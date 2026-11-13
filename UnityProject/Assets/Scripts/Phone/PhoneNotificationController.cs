using UnityEngine;

public class PhoneNotificationController : MonoBehaviour
{
    [SerializeField] private Renderer phoneScreenRenderer;
    [SerializeField] private GameObject notificationLight;

    [SerializeField]
    private Color screenOffColor =
        new Color(0.02f, 0.02f, 0.02f);

    [SerializeField]
    private Color screenOnColor =
        new Color(0.25f, 0.65f, 1f);

    [SerializeField] private float notificationDelay = 1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip notificationBuzz;

    private bool hasUnreadMessage;

    private void Start()
    {
        SetNotificationState(false);

        Invoke(
            nameof(ReceiveNotification),
            notificationDelay
        );
    }

    public void ReceiveNotification()
    {
        hasUnreadMessage = true;
        SetNotificationState(true);

        if (audioSource != null && notificationBuzz != null)
        {
            audioSource.PlayOneShot(notificationBuzz);
        }
    }

    public void MarkAsRead()
    {
        hasUnreadMessage = false;
        SetNotificationState(false);
    }

    private void SetNotificationState(bool active)
    {
        if (phoneScreenRenderer != null)
        {
            phoneScreenRenderer.material.color =
                active
                    ? screenOnColor
                    : screenOffColor;
        }

        if (notificationLight != null)
        {
            notificationLight.SetActive(active);
        }
    }
}
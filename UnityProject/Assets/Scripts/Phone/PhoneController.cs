using TMPro;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    [SerializeField] private MissionManager missionManager;

    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text missionTitleText;
    [SerializeField] private TMP_Text messageText;

    private void Start()
    {
        RefreshMessage();
    }

    public void RefreshMessage()
    {
        if (missionManager == null)
        {
            return;
        }

        senderText.text = "UNKNOWN";

        missionTitleText.text =
            missionManager.GetMissionName();

        messageText.text =
            missionManager.GetBriefing();
    }
}
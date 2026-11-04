using TMPro;
using UnityEngine;

public class ScenarioObjectivesController : MonoBehaviour
{
    [SerializeField]
    private MissionManager missionManager;

    [SerializeField]
    private TMP_Text missionTitleText;

    [SerializeField]
    private TMP_Text objectivesText;

    private void Start()
    {
        Refresh();

        if (missionManager != null)
        {
            missionManager.OnObjectiveCompleted += Refresh;
            missionManager.OnMissionCompleted += Refresh;
        }
    }

    private void OnDestroy()
    {
        if (missionManager != null)
        {
            missionManager.OnObjectiveCompleted -= Refresh;
            missionManager.OnMissionCompleted -= Refresh;
        }
    }

    public void Refresh()
    {
        if (missionManager == null)
        {
            return;
        }

        missionTitleText.text =
            missionManager.GetMissionName();

        objectivesText.text =
            missionManager.GetObjectiveChecklist();
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private string missionName;
    [SerializeField]
    private List<MissionObjective> objectives =
        new List<MissionObjective>();

    private bool missionComplete;

    public bool IsMissionComplete => missionComplete;

    public event Action OnMissionCompleted;

    public void EvaluateObjectives(
        CommandProcessor commandProcessor,
        TerminalController terminalController)
    {
        if (missionComplete)
        {
            return;
        }

        foreach (MissionObjective objective in objectives)
        {
            bool justCompleted =
                objective.Check(commandProcessor);

            if (justCompleted)
            {
                terminalController.AppendSystemMessage(
                    $"[OBJECTIVE COMPLETE] {objective.description}"
                );
            }
        }

        if (AllObjectivesComplete())
        {
            CompleteMission(terminalController);
        }
    }

    private bool AllObjectivesComplete()
    {
        if (objectives.Count == 0)
        {
            return false;
        }

        foreach (MissionObjective objective in objectives)
        {
            if (!objective.isComplete)
            {
                return false;
            }
        }

        return true;
    }

    private void CompleteMission(
        TerminalController terminalController)
    {
        missionComplete = true;

        terminalController.AppendSystemMessage(
            $"*** MISSION COMPLETE: {missionName} ***"
        );

        OnMissionCompleted?.Invoke();
    }
}
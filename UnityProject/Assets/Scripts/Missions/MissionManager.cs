using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private string missionName;
    [TextArea(3, 6)]
    [SerializeField] private string briefing;
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

    public string GetMissionBriefing()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine($"=== MISSION: {missionName} ===");

        if (!string.IsNullOrWhiteSpace(briefing))
        {
            builder.AppendLine(briefing);
        }

        builder.AppendLine();
        builder.AppendLine("Objectives:");

        foreach (MissionObjective objective in objectives)
        {
            builder.AppendLine($"- {objective.description}");
        }

        return builder.ToString().TrimEnd();
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
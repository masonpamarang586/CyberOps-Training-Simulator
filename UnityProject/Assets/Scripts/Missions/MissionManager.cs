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

    [SerializeField] private int startingScore = 100;

    private int currentScore;

    public int CurrentScore => currentScore;

    public bool IsMissionComplete => missionComplete;

    public event Action OnMissionCompleted;

    private void Awake()
    {
        currentScore = startingScore;
    }

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

        builder.AppendLine();
        builder.AppendLine("Type 'hint' for assistance.");

        return builder.ToString().TrimEnd();
    }

    public void RequestHint(TerminalController terminalController)
    {
        if (missionComplete)
        {
            terminalController.AppendSystemMessage(
                "[HINT] Mission already complete."
            );

            return;
        }

        MissionObjective activeObjective = null;

        foreach (MissionObjective objective in objectives)
        {
            if (!objective.isComplete)
            {
                activeObjective = objective;
                break;
            }
        }

        if (activeObjective == null)
        {
            terminalController.AppendSystemMessage(
                "[HINT] No active objective."
            );

            return;
        }

        string hint = activeObjective.GetNextHint(out int tier);

        if (hint == null)
        {
            terminalController.AppendSystemMessage(
                "[HINT] No additional hints available for this objective."
            );

            return;
        }

        int penalty = GetHintPenalty(tier);

        currentScore = Mathf.Max(
            0,
            currentScore - penalty
        );

        terminalController.AppendSystemMessage(
            $"[HINT {tier}/3] {hint}"
        );

        terminalController.AppendSystemMessage(
            $"[HINT PENALTY] -{penalty} | Score: {currentScore}"
        );
    }

    private int GetHintPenalty(int tier)
    {
        switch (tier)
        {
            case 1:
                return 5;

            case 2:
                return 10;

            case 3:
                return 20;

            default:
                return 0;
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

        terminalController.AppendSystemMessage(
            $"Mission Score: {currentScore}/{startingScore}"
        );

        OnMissionCompleted?.Invoke();
    }
}
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
    [SerializeField] private ScoreManager scoreManager;
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

        builder.AppendLine();
        builder.AppendLine("Type 'hint' for assistance.");
        builder.AppendLine("Type 'score' to view your current score.");

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

        int penalty = 0;

        if (scoreManager != null)
        {
            penalty = scoreManager.ApplyHintPenalty(tier);
        }

        terminalController.AppendSystemMessage(
            $"[HINT {tier}/3] {hint}"
        );

        if (scoreManager != null)
        {
            terminalController.AppendSystemMessage(
                $"[HINT PENALTY] -{penalty} | Score: {scoreManager.CurrentScore}"
            );
        }
    }

    public string GetScoreSummary()
    {
        if (scoreManager == null)
        {
            return "Score system unavailable.";
        }

        return scoreManager.GetScoreSummary();
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

        if (scoreManager != null)
        {
            terminalController.AppendSystemMessage(
                scoreManager.GetScoreSummary()
            );
        }

        OnMissionCompleted?.Invoke();
    }
}
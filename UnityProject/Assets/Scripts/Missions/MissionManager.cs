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
    public event Action OnObjectiveCompleted;
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

                OnObjectiveCompleted?.Invoke();
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

    public string RequestHint()
    {
        if (missionComplete)
        {
            return "[HINT] Mission already complete.";
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
            return "[HINT] No active objective.";
        }

        string hint =
            activeObjective.GetNextHint(out int tier);

        if (hint == null)
        {
            return
                "[HINT] No additional hints available for this objective.";
        }

        int penalty = 0;

        if (scoreManager != null)
        {
            penalty =
                scoreManager.ApplyHintPenalty(tier);
        }

        string message =
            $"[HINT {tier}/3] {hint}";

        if (scoreManager != null)
        {
            message +=
                $"\n[HINT PENALTY] -{penalty}" +
                $" | Score: {scoreManager.CurrentScore}";
        }

        return message;
    }

    public void RequestHint(
    TerminalController terminalController)
    {
        terminalController.AppendSystemMessage(
            RequestHint()
        );
    }

    public string GetScoreSummary()
    {
        if (scoreManager == null)
        {
            return "Score system unavailable.";
        }

        return scoreManager.GetScoreSummary();
    }

    public string GetMissionName()
    {
        return missionName;
    }

    public string GetObjectiveChecklist()
    {
        StringBuilder builder =
            new StringBuilder();

        foreach (MissionObjective objective in objectives)
        {
            string marker =
                objective.isComplete
                    ? "[X]"
                    : "[ ]";

            builder.AppendLine(
                $"{marker} {objective.description}"
            );
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

    private void CompleteMission(TerminalController terminalController = null)
    {
        missionComplete = true;

        if (terminalController != null)
        {
            terminalController.AppendSystemMessage(
                $"*** MISSION COMPLETE: {missionName} ***"
            );

            if (scoreManager != null)
            {
                terminalController.AppendSystemMessage(
                    scoreManager.GetScoreSummary()
                );
            }
        }

        OnMissionCompleted?.Invoke();
    }

    public bool CompleteExternalObjective(string target)
    {
        if (missionComplete)
        {
            return false;
        }

        foreach (MissionObjective objective in objectives)
        {
            if (objective.type != MissionObjectiveType.ExternalEvent)
            {
                continue;
            }

            if (objective.isComplete)
            {
                continue;
            }

            if (!string.Equals(
                objective.target,
                target,
                StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            objective.isComplete = true;

            OnObjectiveCompleted?.Invoke();

            if (AllObjectivesComplete())
            {
                CompleteMission();
            }

            return true;
        }

        return false;
    }
}
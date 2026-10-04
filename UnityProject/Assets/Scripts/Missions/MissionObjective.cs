using System;
using UnityEngine;

public enum MissionObjectiveType
{
    ReachPath,
    ReadFile
}

[Serializable]
public class MissionObjective
{
    public string description;
    public MissionObjectiveType type;
    public string target;
    [Header("Hints")]

    [TextArea(2, 4)]
    public string conceptualHint;

    [TextArea(2, 4)]
    public string guidedHint;

    [TextArea(2, 4)]
    public string directHint;

    [HideInInspector]
    public int hintsUsed;

    [HideInInspector]
    public bool isComplete;

    public bool Check(CommandProcessor commandProcessor)
    {
        if (isComplete)
        {
            return false;
        }

        bool completed = false;

        switch (type)
        {
            case MissionObjectiveType.ReachPath:
                completed =
                    commandProcessor.GetCurrentPath() == target;
                break;

            case MissionObjectiveType.ReadFile:
                completed =
                    commandProcessor.HasReadFile(target);
                break;
        }

        if (completed)
        {
            isComplete = true;
            return true;
        }

        return false;
    }

    public string GetNextHint(out int tier)
    {
        tier = hintsUsed + 1;

        switch (hintsUsed)
        {
            case 0:
                hintsUsed++;
                return conceptualHint;

            case 1:
                hintsUsed++;
                return guidedHint;

            case 2:
                hintsUsed++;
                return directHint;

            default:
                tier = 0;
                return null;
        }
    }
}
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
}
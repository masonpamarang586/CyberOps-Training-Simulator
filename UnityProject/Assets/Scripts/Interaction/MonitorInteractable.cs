using UnityEngine;

public abstract class MonitorInteractable : MonoBehaviour
{
    public bool IsFocused { get; protected set; }

    public abstract void EnterFocus();
    public abstract void ExitFocus();
}
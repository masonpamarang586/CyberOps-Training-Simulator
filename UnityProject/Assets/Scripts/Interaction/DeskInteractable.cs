using UnityEngine;

public abstract class DeskInteractable : MonoBehaviour
{
    public bool IsFocused { get; protected set; }

    public abstract void EnterFocus();
    public abstract void ExitFocus();
}
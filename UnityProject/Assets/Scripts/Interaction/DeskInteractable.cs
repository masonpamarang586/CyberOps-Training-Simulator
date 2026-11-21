using UnityEngine;

public abstract class DeskInteractable : MonoBehaviour
{
    [SerializeField]
    private string interactionLabel = "Interact";

    public bool IsFocused { get; protected set; }

    public string InteractionLabel =>
        interactionLabel;

    public abstract void EnterFocus();
    public abstract void ExitFocus();
}
using TMPro;
using UnityEngine;

public class TerminalController : MonoBehaviour 
{
    [SerializeField] private TMP_Text terminalOutput;
    [SerializeField] private TMP_InputField terminalInput;

    private void Start() 
    {
        terminalInput.onSubmit.AddListener(HandleCommand);
        terminalInput.ActivateInputField();
    }

    private void HandleCommand(string command) 
    {
        command = command.Trim();
        if (string.IsNullOrEmpty(command))
        {
            RefocusInput();
            return;
        }

        terminalOutput.text +=
            $"\nuser@target:~$ {command}" +
            $"\nYou entered: {command}";
        terminalInput.text = "";
        RefocusInput();
    }

    private void RefocusInput()
    {
        terminalInput.ActivateInputField();
        terminalInput.Select();
    }

    private void OnDestroy()
    {
        terminalInput.onSubmit.RemoveListener(HandleCommand);
    }
}

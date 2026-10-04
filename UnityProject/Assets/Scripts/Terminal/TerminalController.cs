using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerminalController : MonoBehaviour
{
    [SerializeField] private TMP_Text terminalOutput;
    [SerializeField] private TMP_InputField terminalInput;
    [SerializeField] private ScrollRect terminalScroll;
    [SerializeField] private MissionManager missionManager;

    private CommandProcessor commandProcessor;

    private void Start()
    {
        commandProcessor = new CommandProcessor();

        string startupText = "CyberOps Linux Simulator";

        if (missionManager != null)
        {
            startupText +=
                "\n\n" +
                missionManager.GetMissionBriefing();
        }

        terminalOutput.text =
            startupText +
            "\n\n" +
            commandProcessor.GetPrompt();

        terminalInput.onSubmit.AddListener(HandleCommand);

        terminalInput.ActivateInputField();
    }

    private IEnumerator ScrollToBottom()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();
        terminalScroll.verticalNormalizedPosition = 0f;
    }

    private void HandleCommand(string input)
    {
        string command = input.Trim();

        if (command.ToLower() == "hint")
        {
            if (missionManager != null)
            {
                missionManager.RequestHint(this);
            }
            else
            {
                AppendSystemMessage(
                    "[HINT] No active mission."
                );
            }

            terminalInput.text = "";

            StartCoroutine(ScrollToBottom());
            RefocusInput();

            return;
        }

        if (string.IsNullOrEmpty(command))
        {
            RefocusInput();
            return;
        }

        terminalOutput.text += $"\n{commandProcessor.GetPrompt()} {command}";

        string result = commandProcessor.Execute(command);

        if (!string.IsNullOrEmpty(result))
        {
            terminalOutput.text += $"\n{result}";
        }

        if (missionManager != null)
        {
            missionManager.EvaluateObjectives(
                commandProcessor,
                this
            );
        }

        terminalInput.text = "";

        StartCoroutine(ScrollToBottom());
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

    public void FocusInput()
    {
        StartCoroutine(FocusInputNextFrame());
    }

    private IEnumerator FocusInputNextFrame()
    {
        yield return null;

        terminalInput.Select();
        terminalInput.ActivateInputField();
    }

    public void AppendSystemMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        terminalOutput.text += $"\n{message}";
    }
}
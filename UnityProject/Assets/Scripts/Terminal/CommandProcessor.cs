using System;

public class CommandProcessor
{
    private readonly VirtualFileSystem fileSystem;

    public CommandProcessor()
    {
        fileSystem = new VirtualFileSystem();
    }

    public string Execute(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "";
        }

        string[] parts = input.Trim().Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries
        );

        string command = parts[0].ToLower();

        switch (command)
        {
            case "help":
                return Help();

            case "pwd":
                return fileSystem.GetCurrentPath();

            case "ls":
                return ListDirectory(parts);

            case "cd":
                return ChangeDirectory(parts);

            case "cat":
                return ReadFile(parts);
            case "whoami":
                return fileSystem.GetCurrentUsername();
            default:
                return $"bash: {command}: command not found";
        }
    }

    private string Help()
    {
        return
            "Available commands:\n" +
            "help    Display available commands\n" +
            "pwd     Print current working directory\n" +
            "ls      List directory contents\n" +
            "cd      Change directory\n" +
            "cat     Display file contents\n" +
            "whoami  Display current user";
    }

    private string ChangeDirectory(string[] parts)
    {
        if (parts.Length < 2)
        {
            return "bash: cd: missing operand";
        }

        return fileSystem.ChangeDirectory(parts[1]);
    }

    private string ListDirectory(string[] parts)
    {
        if (parts.Length < 2)
        {
            return fileSystem.ListDirectory();
        }

        return fileSystem.ListDirectory(parts[1]);
    }

    private string ReadFile(string[] parts)
    {
        if (parts.Length < 2)
        {
            return "cat: missing operand";
        }

        return fileSystem.ReadFile(parts[1]);
    }

    public string GetPrompt()
    {
        string username = fileSystem.GetCurrentUsername();
        string path = fileSystem.GetPromptPath();

        return $"{username}@target:{path}$";
    }

    public string GetCurrentPath()
    {
        return fileSystem.GetCurrentPath();
    }

    public bool HasReadFile(string path)
    {
        return fileSystem.HasReadFile(path);
    }
}
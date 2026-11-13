using System;

public class CommandProcessor
{
    private readonly VirtualFileSystem fileSystem;
    private readonly PasswordAttackSimulator passwordAttackSimulator;
    private string lastSearchTerm;
    private string lastSearchPath;

    public CommandProcessor()
    {
        fileSystem = new VirtualFileSystem();
        passwordAttackSimulator =
            new PasswordAttackSimulator();
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
            case "sudo":
                return Sudo(parts);
            case "crack":
                return Crack(parts);
            case "grep":
                return Grep(parts);
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
            "whoami  Display current user\n" +
            "sudo    Execute or inspect elevated permissions\n" +
            "grep    Search a file for matching text\n" +
            "crack   Run simulated password dictionary attack";

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

    private string Sudo(string[] parts)
    {
        if (parts.Length < 2)
        {
            return "sudo: a command is required";
        }

        if (parts[1] == "-l")
        {
            if (fileSystem.IsCurrentUserRoot())
            {
                return
                    "User root may run the following commands on target:\n" +
                    "    (ALL : ALL) ALL";
            }

            return
                "User user may run the following commands on target:\n" +
                "    (root) NOPASSWD: /usr/bin/backup-tool";
        }

        if (parts[1] == "/usr/bin/backup-tool")
        {
            if (fileSystem.IsCurrentUserRoot())
            {
                return "backup-tool: maintenance task completed.";
            }

            string result = fileSystem.EscalateToRoot();

            return
                "backup-tool: running maintenance task with elevated privileges...\n" +
                result;
        }

        return
            $"sudo: {fileSystem.GetCurrentUsername()} is not allowed to execute " +
            $"'{parts[1]}' as root on target";
    }

    public string GetPrompt()
    {
        string username = fileSystem.GetCurrentUsername();
        string path = fileSystem.GetPromptPath();

        string promptSymbol =
            fileSystem.IsCurrentUserRoot() ? "#" : "$";

        return $"{username}@target:{path}{promptSymbol}";
    }

    private string Crack(string[] parts)
    {
        if (parts.Length < 3)
        {
            return
                "Usage: crack <username> <wordlist>";
        }

        string username = parts[1];
        string wordlistPath = parts[2];

        if (!fileSystem.HasReadFile("/etc/shadow"))
        {
            return
                "crack: no credential hashes acquired. " +
                "Inspect /etc/shadow first.";
        }

        string hash =
            fileSystem.GetPasswordHashForUser(username);

        if (string.IsNullOrWhiteSpace(hash))
        {
            return
                $"crack: no hash found for user '{username}'";
        }

        string wordlistContent =
            fileSystem.ReadFile(wordlistPath);

        if (wordlistContent.StartsWith("cat:"))
        {
            return
                $"crack: unable to load wordlist\n" +
                wordlistContent;
        }

        return passwordAttackSimulator.RunDictionaryAttack(
            username,
            hash,
            wordlistContent
        );
    }

    private string Grep(string[] parts)
    {
        if (parts.Length < 3)
        {
            return "Usage: grep <term> <file>";
        }

        string searchTerm = parts[1];
        string filePath = parts[2];

        string content =
            fileSystem.ReadFile(filePath);

        if (content.StartsWith("cat:"))
        {
            return
                $"grep: unable to read '{filePath}'";
        }

        string[] lines =
            content.Split('\n');

        System.Text.StringBuilder matches =
            new System.Text.StringBuilder();

        foreach (string line in lines)
        {
            if (line.Contains(
                searchTerm,
                StringComparison.OrdinalIgnoreCase))
            {
                matches.AppendLine(line);
            }
        }

        lastSearchTerm = searchTerm;
        lastSearchPath = filePath;

        if (matches.Length == 0)
        {
            return "";
        }

        return matches.ToString().TrimEnd();
    }

    public string GetCurrentPath()
    {
        return fileSystem.GetCurrentPath();
    }

    public bool HasReadFile(string path)
    {
        return fileSystem.HasReadFile(path);
    }

    public bool IsCurrentUserRoot()
    {
        return fileSystem.IsCurrentUserRoot();
    }
    public bool HasRecoveredPassword(string username)
    {
        return passwordAttackSimulator.HasRecoveredPassword(username);
    }

    public bool HasSearchedFile(string searchTerm, string filePath)
    {
        return
            string.Equals(
                lastSearchTerm,
                searchTerm,
                StringComparison.OrdinalIgnoreCase) &&
            string.Equals(
                lastSearchPath,
                filePath,
                StringComparison.OrdinalIgnoreCase);
    }
}
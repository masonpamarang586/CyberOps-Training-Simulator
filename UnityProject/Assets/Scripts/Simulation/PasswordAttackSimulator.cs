using System.Collections.Generic;
using System.Text;

public class PasswordAttackSimulator
{
    private readonly Dictionary<string, string> simulatedPasswords =
        new Dictionary<string, string>
        {
            { "$sim$A91C4B7E", "backup2026" }
        };

    private readonly Dictionary<string, string> recoveredPasswords =
        new Dictionary<string, string>();

    public string RunDictionaryAttack(
        string username,
        string hash,
        string wordlistContent)
    {
        if (!simulatedPasswords.TryGetValue(
            hash,
            out string correctPassword))
        {
            return
                $"crack: no recoverable password profile exists for {username}";
        }

        string[] candidates = wordlistContent.Split(
            '\n',
            System.StringSplitOptions.RemoveEmptyEntries
        );

        StringBuilder output = new StringBuilder();

        output.AppendLine(
            $"Starting simulated dictionary attack against {username}..."
        );

        output.AppendLine(
            $"Target hash: {hash}"
        );

        int attempts = 0;

        foreach (string candidateLine in candidates)
        {
            string candidate = candidateLine.Trim();

            if (candidate == "")
            {
                continue;
            }

            attempts++;

            output.AppendLine(
                $"[{attempts}] Trying: {candidate}"
            );

            if (candidate == correctPassword)
            {
                recoveredPasswords[username] = candidate;

                output.AppendLine();
                output.AppendLine(
                    $"PASSWORD RECOVERED: {username}:{candidate}"
                );

                return output.ToString().TrimEnd();
            }
        }

        output.AppendLine();
        output.AppendLine(
            "Password not found in supplied wordlist."
        );

        return output.ToString().TrimEnd();
    }

    public bool HasRecoveredPassword(string username)
    {
        return recoveredPasswords.ContainsKey(username);
    }

    public string GetRecoveredPassword(string username)
    {
        if (recoveredPasswords.TryGetValue(
            username,
            out string password))
        {
            return password;
        }

        return null;
    }
}
public class SimUser
{
    public string Username { get; }
    public bool IsRoot { get; }

    public SimUser(string username, bool isRoot = false)
    {
        Username = username;
        IsRoot = isRoot;
    }
}
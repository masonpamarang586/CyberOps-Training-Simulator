public class SimFile
{
    public string Name { get; }
    public string Content { get; set; }

    public string Owner { get; }
    public PermissionLevel Permission { get; }

    public SimFile(
        string name, 
        string content,
        string owner = "user",
        PermissionLevel permission = PermissionLevel.User)
    {
        Name = name;
        Content = content;
        Owner = owner;
        Permission = permission;
    }
}

using System.Linq;

public class VirtualFileSystem
{
    private SimDirectory root;
    private SimDirectory currentDirectory;

    private SimUser currentUser;

    public VirtualFileSystem()
    {
        BuildFileSystem();
        currentDirectory = root.Directories["home"].Directories["user"];
        currentUser = new SimUser("user");
    }

    private void BuildFileSystem()
    {
        root = new SimDirectory(
            "",
            null,
            "root",
            PermissionLevel.Public
        );

        SimDirectory home = new SimDirectory(
            "home", 
            root,
            "root",
            PermissionLevel.Public
        );
        SimDirectory etc = new SimDirectory(
            "etc", 
            root,
            "root",
            PermissionLevel.Public
        );
        SimDirectory var = new SimDirectory(
            "var", 
            root,
            "root",
            PermissionLevel.Public
        );

        root.AddDirectory(home);
        root.AddDirectory(etc);
        root.AddDirectory(var);

        // /home/user
        SimDirectory user = new SimDirectory(
            "user", 
            home,
            "user",
            PermissionLevel.User
        );
        home.AddDirectory(user);

        SimDirectory documents = new SimDirectory(
            "Documents", 
            user,
            "user",
            PermissionLevel.User
        );
        SimDirectory downloads = new SimDirectory(
            "Downloads", 
            user,
            "user",
            PermissionLevel.User
        );

        user.AddDirectory(documents);
        user.AddDirectory(downloads);

        user.AddFile(
            new SimFile(
                "notes.txt",
                "Personal notes:\nRemember to review server configuration."
            )
        );

        documents.AddFile(
            new SimFile(
                "employee_notes.txt",
                "IT Notes:\nBackup server maintenance scheduled Friday."
            )
        );

        // /etc
        etc.AddFile(
            new SimFile(
                "passwd",
                "root:x:0:0:root:/root:/bin/bash\n" +
                "user:x:1000:1000:user:/home/user:/bin/bash",
                "root",
                PermissionLevel.Public
            )
        );
        etc.AddFile(
            new SimFile(
                "shadow",
                "root:$6$examplehash\nuser:$6$anotherhash",
                "root",
                PermissionLevel.Root
            )
        );

        // /var/log
        SimDirectory log = new SimDirectory(
            "log", 
            var,
            "root",
            PermissionLevel.Public
        );
        var.AddDirectory(log);

        log.AddFile(
            new SimFile(
                "auth.log",
                "Sep 23 14:03 sshd: Accepted password for user"
            )
        );

        // /root
        SimDirectory rootHome = new SimDirectory(
            "root",
            root,
            "root",
            PermissionLevel.Root
        );
        root.AddDirectory(rootHome);

        rootHome.AddFile(
            new SimFile(
                "admin_notes.txt",
                "Confidential administrator notes.",
                "root",
                PermissionLevel.Root
            )
        );
    }

    public string GetCurrentPath()
    {
        if (currentDirectory == root)
        {
            return "/";
        }

        string path = "";
        SimDirectory directory = currentDirectory;

        while (directory != null && directory != root)
        {
            path = "/" + directory.Name + path;
            directory = directory.Parent;
        }

        return path;
    }

    public string GetPromptPath()
    {
        string path = GetCurrentPath();
        string homePath = "/home/user";

        if (path == homePath)
        {
            return "~";
        }

        if (path.StartsWith(homePath + "/"))
        {
            return "~" + path.Substring(homePath.Length);
        }

        return path;
    }

    public string ListCurrentDirectory()
    {
        var directories = currentDirectory.Directories.Keys;
        var files = currentDirectory.Files.Keys;

        return string.Join("    ", directories.Concat(files));
    }

    public string ChangeDirectory(string directoryName)
    {
        if (directoryName == "/")
        {
            currentDirectory = root;
            return "";
        }

        if (directoryName == "..")
        {
            if (currentDirectory.Parent != null)
            {
                currentDirectory = currentDirectory.Parent;
            }

            return "";
        }

        if (currentDirectory.Directories.TryGetValue(
            directoryName,
            out SimDirectory directory))
        {
            if (!CanEnter(directory))
            {
                return $"bash: cd: {directoryName}: Permission denied";
            }

            currentDirectory = directory;
            return "";
        }

        return $"bash: cd: {directoryName}: No such file or directory";
    }

    public string ReadFile(string fileName)
    {
        if (!currentDirectory.Files.TryGetValue(fileName, out SimFile file))
        {
            return $"cat: {fileName}: No such file";
        }

        if (!CanRead(file))
        {
            return $"cat: {fileName}: Permission denied";
        }

        return file.Content;
    }

    private bool CanRead(SimFile file)
    {
        if (currentUser.IsRoot)
        {
            return true;
        }

        switch (file.Permission)
        {
            case PermissionLevel.Public:
                return true;

            case PermissionLevel.User:
                return file.Owner == currentUser.Username;

            case PermissionLevel.Root:
                return false;

            default:
                return false;
        }
    }

    private bool CanEnter(SimDirectory directory)
    {
        if (currentUser.IsRoot)
        {
            return true;
        }

        switch (directory.Permission)
        {
            case PermissionLevel.Public:
                return true;

            case PermissionLevel.User:
                return directory.Owner == currentUser.Username;

            case PermissionLevel.Root:
                return false;

            default:
                return false;
        }
    }
    public string GetCurrentUsername()
    {
        return currentUser.Username;
    }
}
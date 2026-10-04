using System.Collections.Generic;
using System.Linq;

public class VirtualFileSystem
{
    private SimDirectory root;
    private SimDirectory currentDirectory;

    private SimUser currentUser;
    private HashSet<string> readFiles = new HashSet<string>();

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
                "Sep 23 13:58 sshd: Failed password for user from 203.0.113.42 port 51820 ssh2\n" +
                "Sep 23 14:01 sshd: Failed password for user from 203.0.113.42 port 51821 ssh2\n" +
                "Sep 23 14:03 sshd: Accepted password for user from 203.0.113.42 port 51822 ssh2"
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

    public string ListDirectory(string path = null)
    {
        SimDirectory directory;

        if (string.IsNullOrWhiteSpace(path))
        {
            directory = currentDirectory;
        }
        else
        {
            string error = ResolveDirectory(path, out directory);

            if (error != "")
            {
                return $"ls: {path}: {error}";
            }
        }

        var directories = directory.Directories.Keys;
        var files = directory.Files.Keys;

        return string.Join("    ", directories.Concat(files));
    }

    public string ChangeDirectory(string path)
    {
        string error = ResolveDirectory(path, out SimDirectory directory);

        if (error != "")
        {
            return $"bash: cd: {path}: {error}";
        }

        currentDirectory = directory;
        return "";
    }


    public string ReadFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "cat: missing operand";
        }

        int lastSlash = path.LastIndexOf('/');

        string directoryPath;
        string fileName;

        if (lastSlash == -1)
        {
            directoryPath = ".";
            fileName = path;
        }
        else
        {
            directoryPath = path.Substring(0, lastSlash);

            if (directoryPath == "")
            {
                directoryPath = "/";
            }

            fileName = path.Substring(lastSlash + 1);
        }

        string error = ResolveDirectory(
            directoryPath,
            out SimDirectory directory
        );

        if (error != "")
        {
            return $"cat: {path}: {error}";
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return $"cat: {path}: No such file";
        }

        if (!directory.Files.TryGetValue(fileName, out SimFile file))
        {
            return $"cat: {path}: No such file";
        }

        if (!CanRead(file))
        {
            return $"cat: {path}: Permission denied";
        }

        string directoryAbsolutePath = GetDirectoryPath(directory);

        string absoluteFilePath;

        if (directoryAbsolutePath == "/")
        {
            absoluteFilePath = "/" + fileName;
        }
        else
        {
            absoluteFilePath = directoryAbsolutePath + "/" + fileName;
        }

        readFiles.Add(absoluteFilePath);

        return file.Content;
    }

    private string ResolveDirectory(
    string path,
    out SimDirectory resolvedDirectory
)
    {
        resolvedDirectory = null;

        if (string.IsNullOrWhiteSpace(path))
        {
            return "No such file or directory";
        }

        SimDirectory directory;

        // Absolute path
        if (path.StartsWith("/"))
        {
            directory = root;
        }
        // Home shortcut
        else if (path == "~" || path.StartsWith("~/"))
        {
            directory = root.Directories["home"].Directories["user"];

            if (path == "~")
            {
                resolvedDirectory = directory;
                return "";
            }

            path = path.Substring(2);
        }
        // Relative path
        else
        {
            directory = currentDirectory;
        }

        string[] parts = path.Split(
            '/',
            System.StringSplitOptions.RemoveEmptyEntries
        );

        foreach (string part in parts)
        {
            if (part == ".")
            {
                continue;
            }

            if (part == "..")
            {
                if (directory.Parent != null)
                {
                    directory = directory.Parent;
                }

                continue;
            }

            if (!directory.Directories.TryGetValue(
                part,
                out SimDirectory nextDirectory))
            {
                return "No such file or directory";
            }

            if (!CanEnter(nextDirectory))
            {
                return "Permission denied";
            }

            directory = nextDirectory;
        }

        resolvedDirectory = directory;
        return "";
    }
    public bool HasReadFile(string absolutePath)
    {
        return readFiles.Contains(absolutePath);
    }

    private string GetDirectoryPath(SimDirectory directory)
    {
        if (directory == root)
        {
            return "/";
        }

        string path = "";
        SimDirectory current = directory;

        while (current != null && current != root)
        {
            path = "/" + current.Name + path;
            current = current.Parent;
        }

        return path;
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
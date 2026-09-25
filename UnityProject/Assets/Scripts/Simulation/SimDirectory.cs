using UnityEngine;
using System.Collections.Generic;

public class SimDirectory
{
    public string Name { get; }
    public SimDirectory Parent { get; }

    public string Owner { get; }
    public PermissionLevel Permission { get; }

    public Dictionary<string, SimDirectory> Directories { get; }
    public Dictionary<string, SimFile> Files { get; }

    public SimDirectory(
        string name,
        SimDirectory parent = null,
        string owner = "root",
        PermissionLevel permission = PermissionLevel.Public)
    {
        Name = name;
        Parent = parent;
        Owner = owner;
        Permission = permission;

        Directories = new Dictionary<string, SimDirectory>();
        Files = new Dictionary<string, SimFile>();
    }

    public void AddDirectory(SimDirectory directory)
    {
        Directories[directory.Name] = directory;
    }

    public void AddFile(SimFile file)
    {
        Files[file.Name] = file;
    }
}

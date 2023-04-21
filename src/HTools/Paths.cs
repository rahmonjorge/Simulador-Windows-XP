namespace HTools;

using System.Reflection;

/// <summary>
/// Class that handles paths.
/// </summary>
public static class Paths
{
    /// <summary>
    /// Returns the path to the DLL of the originClass.
    /// </summary>
    public static string GetAssemblyPath(Type originClass)
    {
        if (originClass == null) throw new ArgumentNullException("Origin class argument is null.");
        string root = string.Empty;
        Assembly? ass = Assembly.GetAssembly(originClass);
        if (ass != null) root = ass.Location;
        else throw new NullReferenceException("Could not find assembly from origin class: " + originClass.ToString());
        return root;
    }

    /// <summary>
    /// Goes up the path until specified destination.
    /// </summary>
    public static string Climb(string path, string destination)
    {
        string current = Path.GetFileName(path);
        if (current == destination) return path;

        DirectoryInfo? parent = Directory.GetParent(path);
        if (parent != null) return Climb(parent.FullName, destination);
        else throw new NullReferenceException("No more parents to climb.");
    }

    /// <summary>
    /// Tries to find '\src' folder from the DLL location and the project root folder name.
    /// </summary>
    public static string GetSrcPath(Type originClass, string rootFolderName)
    {
        string path = Paths.GetAssemblyPath(originClass); // Get DLL location
        path = Paths.Climb(path, rootFolderName); // Climbs up until project root folder name
        return path += @"\src\"; // Returns the \src full path in the project root folder.
    }

}
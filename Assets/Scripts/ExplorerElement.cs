using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerElement : MonoBehaviour
{
    string path;
    public string Path
    {
        get { return path; }
    }
    bool isDirectory;
    public bool IsDirectory
    {
        get { return isDirectory; }
    }
    private FileExplorer fileExplorer;

    public void Initialize(string filePath, bool directory, FileExplorer explorer)
    {
        path = filePath;
        isDirectory = directory;
        fileExplorer = explorer;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class FileExplorer : MonoBehaviour {

    //make lists for sorting
    public List<DirectoryInfo> dirInfoList = new List<DirectoryInfo>();
    public List<FileInfo> fileInfoList = new List<FileInfo>();
    public ButtonCreater ButtonCreator { get; private set; }
    public string currentPath { get; private set; }
    [SerializeField]
    private Transform driveContent;
    [SerializeField]
    private Transform directoryContent;
    [SerializeField]
    private Transform pathContent;
    [SerializeField]
    private Text searchText;
    [SerializeField]
    private GameObject[] buttonPrefabs;
    [SerializeField]
    private Text deviceButtontext;
    [SerializeField]
    private Scrollbar verticalDirectoryScrollBar;
    
    private void Awake()
    {
        ButtonCreator = GetComponent<ButtonCreater>();
    }

    private void Start()
    {
        // update device button name and create drive buttons in drive view and directory view
        deviceButtontext.text = SystemInfo.deviceName;
        ButtonCreator.CreateDriveButtons(driveContent, this);
        CreateDeviceButtonsInDirectoryView();
        DisplayPath(currentPath);
    }
    
    //create drive buttons in directory view, activated by device button
    public void CreateDeviceButtonsInDirectoryView()
    {
        ClearContent(directoryContent,true);
        ClearContent(pathContent,false);
        ButtonCreator.CreateDeviceButtonInPathView(pathContent, this);
        ButtonCreator.CreateDriveButtons(directoryContent, this);
        currentPath = "";
    }

    // display all directories and files in directory view
    public void DisplayDirectoriesAndFiles(string path)
    {
        currentPath = path;
        DisplayPath(path);
        ClearContent(directoryContent,true);
        // Get subdirectory list and create buttons
        string[] directories = Directory.GetDirectories(path);
        foreach (string directory in directories)
        {
            if (HaveAccess(directory))
            {
                ButtonCreator.CreateDirectoryButton(directoryContent, this, directory);
            }
        }
        // Get file list and create buttons
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            ButtonCreator.CreateFileButton(directoryContent, this, file);
        }
    }

    public void ClearContent(Transform content, bool clearLists)
    {
        //clear lists for sorting
        if (clearLists)
        {
            dirInfoList.Clear();
            fileInfoList.Clear();
        }
        // delete everything in content
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
        //set focus on top of view
       verticalDirectoryScrollBar.value = 1;        
    }

    public void LevelUpDirectory()
    {
        //Check if you are already in top directory (this computer or device...)
        if ( string.IsNullOrEmpty(currentPath)) return;
        //Check if you are in root direction ( C,D,...)
        if (currentPath == Path.GetPathRoot(currentPath))
        {
            CreateDeviceButtonsInDirectoryView();
            return;
        }
        //Remove last "\" so you can use Directory.Getparent
        if (currentPath[currentPath.Length - 1] == '\\')
            currentPath = currentPath.Remove(currentPath.Length - 1, 1);
        //display parent
        DisplayDirectoriesAndFiles(Directory.GetParent(currentPath).FullName);
    }

    public void DisplayPath(string path)
    {
        //destroy all buttons in path before refresh
        ClearContent(pathContent,false);
        //this button always be here
        ButtonCreator.CreateDeviceButtonInPathView(pathContent, this);
        if (string.IsNullOrEmpty(currentPath)) return;

        //Check if you are in root
        if (currentPath == Path.GetPathRoot(currentPath))
        {
            ButtonCreator.CreatePathButton(currentPath, pathContent, this);
            return;
        }
        //Search path if you are not in root:
        Stack<string> parentPaths = new Stack<string>();
        parentPaths.Push(path);
        string dirPath = path;
        //while you are not in root, add parents to stack
        while (dirPath != Path.GetPathRoot(dirPath))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            parentPaths.Push(dirInfo.Parent.FullName);
            dirPath = dirInfo.Parent.FullName;

        }
        //create buttons in top path panel
        ButtonCreator.CreatePathButtons(pathContent, this, parentPaths);
    }

    private void SearchDirectoriesAndFiles(string path,string searchPattern)
    {
        if (path=="")
        {
            //search all drives... it take a while
            foreach (string drive in Directory.GetLogicalDrives())
            {
                SearchDirectoriesAndFiles(drive, searchPattern);
            }
        }
            // search for match in directories
            string[] dirs = Directory.GetDirectories(path, searchPattern);
            foreach (string directory in dirs)
            {
                ButtonCreator.CreateDirectoryButton(directoryContent, this, directory);
            }
            // search for match in files
            string[] files = Directory.GetFiles(path, searchPattern);
            foreach (string file in files)
            {
                ButtonCreator.CreateFileButton(directoryContent, this, file);
            }

            string[] directories = Directory.GetDirectories(path);
            // if you have access, search in child directories
            foreach (string dir in directories)
            {
                if(HaveAccess(dir)) SearchDirectoriesAndFiles(dir, searchPattern);
            }
    }

    public bool HaveAccess(string path)
    {
        try
        {   //check if you have access
            string[] dirs = Directory.GetDirectories(path);
            if (dirs != null)
            {
                return true;
            }
            else return false;
        }
        catch (UnauthorizedAccessException ex)
        {
            Debug.Log(ex);
            return false;
        }
        catch (Exception ex)
        {
            Debug.Log("General exception: " + ex);
            return false;
        }
    }

    // called in search button
    public void DisplaySearching()
    {
        ClearContent(directoryContent,true);
        SearchDirectoriesAndFiles(currentPath, "*" +searchText.text + "*");
    }
}

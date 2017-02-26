using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using System.Security.AccessControl;

public class FileExplorer : MonoBehaviour {

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
    [SerializeField]
    private string currentPath = "";

    private void Start()
    {
        deviceButtontext.text = SystemInfo.deviceName;
        CreateDeviceButtonsInDriversView();
        DisplayPath(currentPath);
    }

    //create drive buttons under the device button, this is done only one, in Start
    private void CreateDeviceButtonsInDriversView()
    {
        foreach (string drive in Directory.GetLogicalDrives())
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefabs[0], driveContent);
            newButton.GetComponent<DriveButton>().InitializeButton(this, drive, drive);
        }
    }

    //create drive buttons in directory view, activated by device button
    public void CreateDeviceButtonsInDirectoryView()
    {
        ClearContent(directoryContent);
        ClearContent(pathContent);
        CreateDeviceButtonInPathView();
        foreach (string drive in Directory.GetLogicalDrives())
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefabs[0], directoryContent);
            newButton.GetComponent<DriveButton>().InitializeButton(this, drive, drive);
            newButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
        }
        currentPath = "";
    }

    public void DisplayDirectoriesAndFiles(string path)
    {
        currentPath = path;
        DisplayPath(path);
        ClearContent(directoryContent);
        try
        {
            // Get subdirectory list and create buttons
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                try
                {   //check if you have access
                    string[] dirs = Directory.GetDirectories(directory);
                    if (dirs != null)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(directory);
                        GameObject newButton = (GameObject)Instantiate(buttonPrefabs[1], directoryContent);
                        newButton.GetComponent<DirectoryButton>().InitializeButton(this, dirInfo.FullName + @"\", dirInfo.Name, dirInfo.CreationTime);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.Log(ex);
                }
                catch (Exception ex)
                {
                    Debug.Log("General exception: " + ex);
                }

            }

            // Get file list and create buttons
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                GameObject newButton = (GameObject)Instantiate(buttonPrefabs[2], directoryContent);
                newButton.GetComponent<FileButton>().InitializeButton(this, fileInfo.FullName + @"\", fileInfo.Name, fileInfo.CreationTime);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Display dirs and files exception: "+ ex);
        }
    }

    private void ClearContent(Transform content)
    {
        // delete everything in directory view
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
        //set focus on top of view
       verticalDirectoryScrollBar.value = 1;        
    }

    public void LevelUpDirectory()
    {
        //Check if you are already in top directory (this computer)
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
        ClearContent(pathContent);
        // device button always be here...
        CreateDeviceButtonInPathView();
        if (string.IsNullOrEmpty(currentPath))return;

        //Check if you are in root
        if (currentPath == Path.GetPathRoot(currentPath))
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefabs[3], pathContent);
            newButton.GetComponent<BaseButtonClass>().InitializeButton(this, currentPath, currentPath);
            return;
        }
        //DIR INFO PRIMO DO TRIDY
        Stack<string> parentPaths = new Stack<string>();
        parentPaths.Push(path);
        string dirPath = path;
        while (dirPath != Path.GetPathRoot(dirPath))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            parentPaths.Push(dirInfo.Parent.FullName);
            dirPath = dirInfo.Parent.FullName;

        }
        //create buttons in top path panel
        foreach (string name in parentPaths)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(name);
            GameObject newButton = (GameObject)Instantiate(buttonPrefabs[3], pathContent);

            //check if dir is root, coz they need different parameters in "constructor"
            if (name != Path.GetPathRoot(name)) newButton.GetComponent<BaseButtonClass>().InitializeButton(this, @dirInfo.FullName + @"\", dirInfo.Name);         
            else newButton.GetComponent<BaseButtonClass>().InitializeButton(this, name, name);
        }
    }

    private void CreateDeviceButtonInPathView()
    {
        GameObject newButton = (GameObject)Instantiate(buttonPrefabs[3], pathContent);
        newButton.GetComponent<Button>().onClick.AddListener(() => CreateDeviceButtonsInDirectoryView());
        newButton.GetComponentInChildren<Text>().text = SystemInfo.deviceName;
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
        
        try
        {
            // Get subdirectory list and create buttons
            string[] dirs = Directory.GetDirectories(path, searchPattern);
            foreach (string directory in dirs)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directory);
                GameObject newButton = (GameObject)Instantiate(buttonPrefabs[1], directoryContent);
                newButton.GetComponent<DirectoryButton>().InitializeButton(this, dirInfo.FullName + @"\", dirInfo.Name, dirInfo.CreationTime);
            }

            string[] files = Directory.GetFiles(path, searchPattern);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                GameObject newButton = (GameObject)Instantiate(buttonPrefabs[2], directoryContent);
                newButton.GetComponent<FileButton>().InitializeButton(this, fileInfo.FullName + @"\", fileInfo.Name, fileInfo.CreationTime);
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories)
            {
                try
                {   //check if you have access
                    string[] dirs5 = Directory.GetDirectories(dir);
                    if (dirs5 != null)
                    {
                        SearchDirectoriesAndFiles(dir, searchPattern);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.Log(ex);
                }
                catch (Exception ex)
                {
                    Debug.Log("General exception: " + ex);
                }
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Debug.Log(ex);
        }
        catch (Exception ex)
        {
            Debug.Log("General exception: " + ex);
        }
    }

    // called with search button
    public void DisplaySearching()
    {
        ClearContent(directoryContent);
        SearchDirectoriesAndFiles(currentPath, "*" +searchText.text + "*");
    }
     
}

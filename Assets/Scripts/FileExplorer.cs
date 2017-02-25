using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class FileExplorer : MonoBehaviour {

    [SerializeField]
    private Transform driveContent;
    [SerializeField]
    private Transform directoryContent;
    [SerializeField]
    private GameObject[] buttonPrefabs;
    [SerializeField]
    private Text deviceButtontext;
    [SerializeField]
    private Scrollbar verticalDirectoryScrollBar;
    [SerializeField]
    private Text pathDisplay;
    [SerializeField]
    private string currentPath = "";

    private void Start()
    {
        deviceButtontext.text = SystemInfo.deviceName;
        CreateDeviceButtonsInDriversView();
        //string[] dirs = Directory.GetDirectories(@"D:\FILMY\", "*",SearchOption.AllDirectories);
    }


    //create drive buttons under the device button
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
        ClearDirectoryContent();
        foreach (string drive in Directory.GetLogicalDrives())
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefabs[0], directoryContent);
            newButton.GetComponent<DriveButton>().InitializeButton(this, drive, drive);
            newButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
        }
        pathDisplay.text = SystemInfo.deviceName;
        currentPath = "";
    }

    public void DisplayDirectoriesAndFiles(string path)
    {
        currentPath = path;
        pathDisplay.text = path;
        ClearDirectoryContent();
        try
        {
            // Get subdirectory list
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directory);
                GameObject newButton = (GameObject)Instantiate(buttonPrefabs[1], directoryContent);
                newButton.GetComponent<DirectoryButton>().InitializeButton(this, dirInfo.FullName + @"\", dirInfo.Name,dirInfo.CreationTime);
            }

            // Get file list
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
            Debug.Log(ex);
        }
    }

    private void ClearDirectoryContent()
    {
        // delete everything in directory view
        foreach (Transform item in directoryContent)
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

        string parentDirectoryPath = Directory.GetParent(currentPath).FullName;
        //display parent
        DisplayDirectoriesAndFiles(parentDirectoryPath);
    }
}

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

    private void Start()
    {
        SetThisDeviceButton();
        CreateDriveButtons();

        //string[] dirs = Directory.GetDirectories(@"D:\FILMY\", "*",SearchOption.AllDirectories);
    }

    private void CreateDriveButtons()
    {
        foreach (string drive in Directory.GetLogicalDrives())
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefabs[0], driveContent);
            newButton.GetComponent<DriveButton>().InitializeButton(this, drive, drive);
        }
    }

    private void SetThisDeviceButton()
    {
        deviceButtontext.text = SystemInfo.deviceName;
    }   

    public void DisplayDirectoriesAndFiles(string path)
    {
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
        foreach (Transform item in directoryContent)
        {
            Destroy(item.gameObject);
        }
       // verticalDirectoryScrollBar.value = 1;
    }
}

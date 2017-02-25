using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileExplorer : MonoBehaviour {

    [SerializeField]
    private Transform driveContent;
    [SerializeField]
    private Transform directoryContent;
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private Text deviceButtontext;
    [SerializeField]
    private Sprite[] buttonSprites;

    private void Start()
    {
        SetThisDeviceButton();
        CreateDriveButtons();

        string[] dirs = Directory.GetDirectories(@"D:\FILMY\", "*",SearchOption.AllDirectories);

        //DirectoryInfo dirInfo = new DirectoryInfo(@"D:\FILMY\");
        //Debug.Log(dirInfo.CreationTime.ToString("d/M/yyyy"));
    }

    private void CreateDriveButtons()
    {
        foreach (string drive in Directory.GetLogicalDrives())
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefab, driveContent);
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.AddComponent<DriveButton>().InitializeButton(this, drive, drive,buttonSprites[0]);
        }
    }

    private void SetThisDeviceButton()
    {
        deviceButtontext.text = SystemInfo.deviceName;
    }

    

    public void DisplayDirectoriesAndFiles(string path)
    {
        string[] dirs = Directory.GetDirectories(path);
    }
}

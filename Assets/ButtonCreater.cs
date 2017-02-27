using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ButtonCreater : MonoBehaviour {

    [SerializeField]
    private GameObject driveButtonPrefab;
    [SerializeField]
    private GameObject directoryButtonPrefab;
    [SerializeField]
    private GameObject fileButtonPrefab;
    [SerializeField]
    private GameObject pathButtonPrefab;

    //DEVICE BUTTON
    //create drive buttons under the device button
    public void CreateDriveButtons(Transform contentParent, FileExplorer fileExplorer)
    {
        foreach (string drive in Directory.GetLogicalDrives())
        {
            GameObject newButton = (GameObject)Instantiate(driveButtonPrefab, contentParent);
            newButton.GetComponent<BaseButtonClass>().InitializeButton(fileExplorer, drive, drive);
        }
    }

    //DIRECTORIES AND FILES BUTTONS
    public void CreateDirectoryButton(Transform contentParent, FileExplorer fileExplorer, string directory)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(directory);
        fileExplorer.dirInfoList.Add(dirInfo);
        GameObject newButton = (GameObject)Instantiate(directoryButtonPrefab, contentParent);
        newButton.GetComponent<DirectoryButton>().InitializeButton(fileExplorer, dirInfo.FullName + @"\", dirInfo.Name, dirInfo.CreationTime);
    }


    public void CreateFileButton(Transform contentParent, FileExplorer fileExplorer, string file)
    {
        FileInfo fileInfo = new FileInfo(file);
        fileExplorer.fileInfoList.Add(fileInfo);
        GameObject newButton = (GameObject)Instantiate(fileButtonPrefab, contentParent);
        newButton.GetComponent<FileButton>().InitializeButton(fileExplorer, fileInfo.FullName + @"\", fileInfo.Name, fileInfo.CreationTime);
    }

    //PATH BUTTONS
    //create first button in path (device)
    public void CreateDeviceButtonInPathView(Transform contentParent, FileExplorer fileExplorer)
    {
        GameObject newButton = (GameObject)Instantiate(pathButtonPrefab, contentParent);
        newButton.GetComponent<Button>().onClick.AddListener(() => fileExplorer.CreateDeviceButtonsInDirectoryView());
        newButton.GetComponentInChildren<Text>().text = SystemInfo.deviceName;
    }

    //create path if you are in root
    public void CreatePathButton(string name, Transform contentParent, FileExplorer fileExplorer)
    {
            GameObject newButton = (GameObject)Instantiate(pathButtonPrefab, contentParent);
            newButton.GetComponent<BaseButtonClass>().InitializeButton(fileExplorer, name, name);
    }
    
    //create path if you are not in root
    public void CreatePathButtons(Transform contentParent, FileExplorer fileExplorer, Stack<string> parentPaths)
    {
        foreach (string name in parentPaths)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(name);
            GameObject newButton = (GameObject)Instantiate(pathButtonPrefab, contentParent);

            //check if dir is root, coz they need different parameters in "constructor"
            if (name != Path.GetPathRoot(name)) newButton.GetComponent<BaseButtonClass>().InitializeButton(fileExplorer, @dirInfo.FullName + @"\", dirInfo.Name);
            else newButton.GetComponent<BaseButtonClass>().InitializeButton(fileExplorer, name, name);
        }
    }
   
}

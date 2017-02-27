using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CreateDirectoryButton : MonoBehaviour {

    [SerializeField]
    private Text directoryName;
    [SerializeField]
    private FileExplorer fileExplorer;
    [SerializeField]
    private GameObject creationPanel;

    //called by another button in creationPanel
    public void CreateDirectory()
    {
        string path = fileExplorer.currentPath + directoryName.text;
        //check for input
        if (!string.IsNullOrEmpty(directoryName.text))
        {//Check for exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                //refresh directoryview
                fileExplorer.DisplayDirectoriesAndFiles(fileExplorer.currentPath);
            }
            else
            {
                //if it exists, create directory with "( number)" just like windows explorer
                int i = 1;
                string newPath;
                do
                {
                    newPath = path + " (" + i.ToString() + ")";
                    i++;

                } while (Directory.Exists(newPath));

                Directory.CreateDirectory(newPath);
                //refresh directoryview
                fileExplorer.DisplayDirectoriesAndFiles(fileExplorer.currentPath);
            }
        }
        else Debug.Log("Invalid path: " + path);

        //finally close creation panel
        ClosePanel();
    }

    public void ActivatePanel()
    {
        //Check if you are already in top directory (this computer or device...) and if so, do nothin
        if (string.IsNullOrEmpty(fileExplorer.currentPath)) return;
        
        creationPanel.SetActive(true);
    }
    //called by cancel button in creation panel
    public void ClosePanel()
    {
        creationPanel.SetActive(false);
    }
}

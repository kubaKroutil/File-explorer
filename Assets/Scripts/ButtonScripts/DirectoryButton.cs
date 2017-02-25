using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectoryButton : BaseButtonClass {

    [SerializeField]
    private Text dateDisplay;

    public virtual void InitializeButton(FileExplorer fileExplorer, string path, string name, DateTime creationTime)            
    {
        base.InitializeButton(fileExplorer, path, name);
        dateDisplay.text = creationTime.ToString();
    }
}


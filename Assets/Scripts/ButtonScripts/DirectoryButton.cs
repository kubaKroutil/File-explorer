using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectoryButton : BaseButtonClass {

    private DateTime creationDate;


    public virtual void InitializeButton(FileExplorer fileExplorer, string path, string name, Sprite image, DateTime creationTime)            
    {
        base.InitializeButton(fileExplorer, path, name,image);
        creationDate = creationTime;
    }

    public override void ButtonClick()
    {
       //fileExplorer.
    }
}


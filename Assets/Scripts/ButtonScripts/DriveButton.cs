using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriveButton : BaseButtonClass {



    public override void ButtonClick()
    {
        fileExplorer.DisplayDirectoriesAndFiles(path);
    }
}

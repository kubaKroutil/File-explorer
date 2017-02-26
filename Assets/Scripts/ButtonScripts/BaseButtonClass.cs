using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButtonClass : MonoBehaviour {

    [SerializeField]
    protected Text nameDisplay;
    protected FileExplorer fileExplorer;
    protected string path;
    
    public virtual void InitializeButton(FileExplorer fileExplorer, string path, string name)
    {
        this.fileExplorer = fileExplorer;
        this.path = path;
        nameDisplay.text = name;
        GetComponent<Button>().onClick.AddListener(()=> ButtonClick());
        transform.localScale = new Vector3(1, 1, 1);
    }

    public virtual void ButtonClick()
    {
        fileExplorer.DisplayDirectoriesAndFiles(path);
    }
}

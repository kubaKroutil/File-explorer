using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButtonClass : MonoBehaviour {

    [SerializeField]
    protected Image image;

    protected FileExplorer fileExplorer;
    protected string path;

    public virtual void InitializeButton(FileExplorer fileExplorer, string path, string name)
    {
        this.fileExplorer = fileExplorer;
        this.path = path;
        GetComponentInChildren<Text>().text = name;
        GetComponent<Button>().onClick.AddListener(()=> ButtonClick());
    }

    public abstract void ButtonClick();
}

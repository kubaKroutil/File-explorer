using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButtonClass : MonoBehaviour {

    protected FileExplorer fileExplorer;
    protected string path;

    public virtual void InitializeButton(FileExplorer fileExplorer, string path, string name, Sprite image)
    {
        this.fileExplorer = fileExplorer;
        this.path = path;
        GetComponentInChildren<Text>().text = name;
        GetComponent<Button>().onClick.AddListener(()=> ButtonClick());

        Image[] sprites = gameObject.GetComponentsInChildren<Image>();
        sprites[1].sprite = image;
    }

    public abstract void ButtonClick();
}

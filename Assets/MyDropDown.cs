using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Linq;

public class MyDropDown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    private FileExplorer fileExplorer;
    [SerializeField]
    private GameObject dirPrefab;
    [SerializeField]
    private GameObject filePrefab;
    [SerializeField]
    private Transform directoryContent;
    [SerializeField]
    private RectTransform container;

    private void Start()
    {
        ToggleDropDown(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleDropDown(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleDropDown(false);
    }

    private void ToggleDropDown(bool open)
    {
        if (open) container.localScale = new Vector3(container.localScale.x, 1, container.localScale.y);
        else container.localScale = new Vector3(container.localScale.x, 0, container.localScale.y);
    }

    //SORTING BUTTONS
    //get infos from lists, then clear them, coz dirs and files will be added again in button creater

    public void SortByCreationDateDesc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderByDescending(p => p.CreationTime).ToArray();
        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderByDescending(p => p.CreationTime).ToArray();
        CreateButtonsAndClearContent(dirInfos, fileInfos);
    }

    public void SortByCreationDateAsc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderBy(p => p.CreationTime).ToArray();
        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderBy(p => p.CreationTime).ToArray();
        CreateButtonsAndClearContent(dirInfos, fileInfos);
    }

    public void SortByNameAsc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderBy(p => p.Name).ToArray();
        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderBy(p => p.Name).ToArray();
        CreateButtonsAndClearContent(dirInfos, fileInfos);
    }

    public void SortByNameDesc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderByDescending(p => p.Name).ToArray();
        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderByDescending(p => p.Name).ToArray();
        CreateButtonsAndClearContent(dirInfos, fileInfos);
    }

    private void CreateButtonsAndClearContent(DirectoryInfo[] dirInfos, FileInfo[] fileInfos)
    {
        fileExplorer.ClearContent(directoryContent, true);
        ToggleDropDown(false);

        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            fileExplorer.buttonCreater.CreateDirectoryButton(directoryContent, fileExplorer, dirInfo.FullName);
        }

        foreach (FileInfo fileInfo in fileInfos)
        {
            fileExplorer.buttonCreater.CreateFileButton(directoryContent, fileExplorer, fileInfo.FullName);
        }
    }
}

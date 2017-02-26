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


    public RectTransform container;
    private float speed = 10f;

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

    public void SortByCreationDateDesc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderByDescending(p => p.CreationTime).ToArray();

        fileExplorer.ClearContent(directoryContent, false);
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            GameObject newButton = (GameObject)Instantiate(dirPrefab, directoryContent);
            newButton.GetComponent<DirectoryButton>().InitializeButton(fileExplorer, dirInfo.FullName + @"\", dirInfo.Name, dirInfo.CreationTime);
        }

        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderByDescending(p => p.CreationTime).ToArray();

        foreach (FileInfo fileInfo in fileInfos)
        {
            GameObject newButton = (GameObject)Instantiate(filePrefab, directoryContent);
            newButton.GetComponent<FileButton>().InitializeButton(fileExplorer, fileInfo.FullName + @"\", fileInfo.Name, fileInfo.CreationTime);
        }
    }

    public void SortByCreationDateAsc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderBy(p => p.CreationTime).ToArray();

        fileExplorer.ClearContent(directoryContent, false);
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            GameObject newButton = (GameObject)Instantiate(dirPrefab, directoryContent);
            newButton.GetComponent<DirectoryButton>().InitializeButton(fileExplorer, dirInfo.FullName + @"\", dirInfo.Name, dirInfo.CreationTime);
        }

        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderBy(p => p.CreationTime).ToArray();

        foreach (FileInfo fileInfo in fileInfos)
        {
            GameObject newButton = (GameObject)Instantiate(filePrefab, directoryContent);
            newButton.GetComponent<FileButton>().InitializeButton(fileExplorer, fileInfo.FullName + @"\", fileInfo.Name, fileInfo.CreationTime);
        }
    }

    public void SortByNameAsc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderBy(p => p.Name).ToArray();

        fileExplorer.ClearContent(directoryContent, false);
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            GameObject newButton = (GameObject)Instantiate(dirPrefab, directoryContent);
            newButton.GetComponent<DirectoryButton>().InitializeButton(fileExplorer, dirInfo.FullName + @"\", dirInfo.Name, dirInfo.CreationTime);
        }

        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderBy(p => p.Name).ToArray();

        foreach (FileInfo fileInfo in fileInfos)
        {
            GameObject newButton = (GameObject)Instantiate(filePrefab, directoryContent);
            newButton.GetComponent<FileButton>().InitializeButton(fileExplorer, fileInfo.FullName + @"\", fileInfo.Name, fileInfo.CreationTime);
        }
    }

    public void SortByNameDesc()
    {
        DirectoryInfo[] dirInfos = fileExplorer.dirInfoList.OrderByDescending(p => p.Name).ToArray();

        fileExplorer.ClearContent(directoryContent, false);
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            GameObject newButton = (GameObject)Instantiate(dirPrefab, directoryContent);
            newButton.GetComponent<DirectoryButton>().InitializeButton(fileExplorer, dirInfo.FullName + @"\", dirInfo.Name, dirInfo.CreationTime);
        }

        FileInfo[] fileInfos = fileExplorer.fileInfoList.OrderByDescending(p => p.Name).ToArray();

        foreach (FileInfo fileInfo in fileInfos)
        {
            GameObject newButton = (GameObject)Instantiate(filePrefab, directoryContent);
            newButton.GetComponent<FileButton>().InitializeButton(fileExplorer, fileInfo.FullName + @"\", fileInfo.Name, fileInfo.CreationTime);
        }
    }
}

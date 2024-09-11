using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileExplorer : MonoBehaviour
{
    [SerializeField]
    Sprite modelSprite;
    [SerializeField]
    Sprite folderSprite;
    [SerializeField]
    Sprite fileSprite;

    [SerializeField]
    Transform contentParent;
    [SerializeField]
    GameObject contentElementPrefab;


    [SerializeField]
    string currentPath;
    [SerializeField]
    TextMeshProUGUI pathText;

    bool isDrag = false;

    [SerializeField]
    private PointableCanvasModule _pointableCanvasModule;

    /*private void OnEnable()
    {
        if (_pointableCanvasModule != null)
        {
            PointableCanvasModule.WhenSelected += HandleWhenSelected;
        }
    }*/

    /*private void OnDisable()
    {
        if (_pointableCanvasModule != null)
        {
            PointableCanvasModule.WhenSelected -= HandleWhenSelected;
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        if (_pointableCanvasModule != null)
        {
            PointableCanvasModule.WhenSelected += HandleWhenSelected;
        }
        if (currentPath == null)
        {
            currentPath = @"C:\Users\qqnei\Desktop";
        }
        DisplayDirectory(currentPath);
    }


    // Update is called once per frame
    void Update()
    {
        bool currentTriggerState = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        if (currentTriggerState && !isTriggerPressed)
        {
            isTriggerPressed = true;
            triggerPressedTime = Time.time;
        }
        else if (!currentTriggerState && isTriggerPressed)
        {
            isTriggerPressed = false;
        }
    }

    public void DisplayDirectory(string path)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        currentPath = path;
        pathText.text = currentPath;

        string parentPath = Path.GetDirectoryName(currentPath);

        if (parentPath != null && parentPath != currentPath)
        {
            CreateItem(parentPath, true, true);
        }

        string[] directories = System.IO.Directory.GetDirectories(path);
        string[] files = System.IO.Directory.GetFiles(path);

        foreach (string dir in directories)
        {
            Debug.Log("folder " + dir);
            CreateItem(dir, true, false);
        }

        foreach (string file in files)
        {
            Debug.Log("file: " + file);
            CreateItem(file, false, false);
        }
    }
    void OpenFile(string path)
    {
        //logic for model loading
    }

    void CreateItem(string path, bool isDirectory, bool isBack)
    {
        GameObject item = Instantiate(contentElementPrefab, contentParent);

        ExplorerElement e = item.AddComponent<ExplorerElement>();
        e.Initialize(path, isDirectory, this);


        var text = item.GetComponentInChildren<TextMeshProUGUI>();
        if(!isBack)
        {
            if (text != null)
            {
                text.text = System.IO.Path.GetFileName(path);
            }
        }
        else
        {
            text.text = "Back";
        }
        Image[] images = item.GetComponentsInChildren<Image>();
        Image icon;
        foreach(Image image in images)
        {
            if(image.gameObject.name == "Icon")
            {
                icon = image;
                if (Directory.Exists(path))
                {
                    icon.sprite = folderSprite;
                }
                else if(File.Exists(path))
                {
                    if(Path.GetExtension(path).ToLower() == ".fbx")
                    {
                        icon.sprite = modelSprite;
                    }
                    else
                    {
                        icon.sprite = fileSprite;
                    }
                }
            }
        }
        


    }

    private void SetUpElement()
    {

    }

    /*private void HandleWhenSelected(PointableCanvasEventArgs args)
    {
        float startTime = 0f;

        if (args.Hovered != null)
        {
            Debug.Log($"Hovered Object: {args.Hovered}");
            Debug.Log($"Type of Hovered Object: {args.Hovered.GetType()}");

            ExplorerElement element = args.Hovered.GetComponentInChildren<ExplorerElement>();
            if (element.IsDirectory)
            {
                DisplayDirectory(element.Path);
            }
            else
            {
                OpenFile(element.Path);
            }
            
            var text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log($"{text.text}");
        }
        else
        {
            Debug.Log("Selected empty area.");
        }
    }*/

    public void HandleWhenSelected(PointableCanvasEventArgs args)
    {
        StartCoroutine(CheckTriggerAndProcess(args));
    }

    private float triggerPressedTime = 0f;
    private bool isTriggerPressed = false;
    private float tapThreshold = 0.15f;

    private IEnumerator CheckTriggerAndProcess(PointableCanvasEventArgs args)
    {
        float startTime = Time.time;
        while (isTriggerPressed && Time.time - startTime < tapThreshold)
        {
            yield return null;
        }

        if (!isTriggerPressed && Time.time - triggerPressedTime < tapThreshold)
        {
            ProcessEvent(args);
        }
        else
        {
            Debug.Log("Trigger held too long or not pressed.");
        }
    }

    private void ProcessEvent(PointableCanvasEventArgs args)
    {
        if (args.Hovered != null)
        {
            Debug.Log($"Hovered Object: {args.Hovered}");
            Debug.Log($"Type of Hovered Object: {args.Hovered.GetType()}");

            ExplorerElement element = args.Hovered.GetComponentInChildren<ExplorerElement>();
            if (element.IsDirectory)
            {
                DisplayDirectory(element.Path);
            }
            else
            {
                OpenFile(element.Path);
            }

            var text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log($"{text.text}");
        }
        else
        {
            Debug.Log("Selected empty area.");
        }
    }
}

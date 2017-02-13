using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;

public class BookScript : MonoBehaviour {

    public GameObject pagePanelRight;
    public GameObject pagePanelLeft;
    public GameObject flipPageFront;
    public GameObject flipPageBack;
    public GameObject pagePivot;
    public Canvas uiCanvas;
    public bool isVisible = false;

    public int pageNumber = 0;

    private string pdfConverterPath;
    private bool pageFlipForward = false;
    private bool pageFlipBack = false;
    public string bookPath = "";
    private Quaternion initrot;
    private Quaternion backrot;
    private Vector3 startPosition;

    // Use this for initialization
    void Start () {

        pdfConverterPath = Application.dataPath + "/../Converter/pdf2img.exe";

        startPosition = transform.position;

        initrot = pagePivot.transform.localRotation;
        pagePivot.transform.RotateAround(pagePivot.transform.position, pagePivot.transform.up, 180f);
        backrot = pagePivot.transform.localRotation;

        this.Hide();
        uiCanvas.GetComponent<FileLoader>().Show();
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {

            if (OVRInput.Get(OVRInput.Button.Four))
            {
                pageNumber = pageNumber + 2;

                // Put current page texture on flipper page
                flipPageFront.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                flipPageFront.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                // Generate new images from pdf
                CallExternalProcess(pdfConverterPath, bookPath + " pageRight.png " + (pageNumber).ToString());
                CallExternalProcess(pdfConverterPath, bookPath + " pageLeft.png " + (pageNumber - 1).ToString());

                pagePanelRight.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                pagePanelRight.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                pagePanelLeft.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageLeft.png");
                pagePanelLeft.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                // Put current new texture on back flipper page
                flipPageBack.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageLeft.png");
                flipPageBack.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                GameObject.Find("Book").GetComponent<AudioSource>().Play();

                pageFlipForward = true;
                pagePivot.transform.localRotation = initrot;

            }


            if (OVRInput.Get(OVRInput.Button.Three))
            {
                pageNumber = pageNumber - 2;

                if (pageNumber < 2)
                    pageNumber = 2;

                CallExternalProcess(pdfConverterPath, bookPath + " pageRight.png " + (pageNumber).ToString());
                CallExternalProcess(pdfConverterPath, bookPath + " pageLeft.png " + (pageNumber - 1).ToString());

                // Put current page texture on flipper page
                flipPageFront.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageLeft.png");
                flipPageFront.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, 1));

                pagePanelRight.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                pagePanelRight.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                pagePanelLeft.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageLeft.png");
                pagePanelLeft.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                // Put current new texture on back flipper page
                flipPageBack.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                flipPageBack.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, 1));


                GameObject.Find("Book").GetComponent<AudioSource>().Play();

                pageFlipBack = true;
                pagePivot.transform.localRotation = backrot;

            }


            if (Input.GetKey(KeyCode.UpArrow))
                transform.Rotate(Vector3.right * 30 * Time.deltaTime);

            if (Input.GetKey(KeyCode.DownArrow))
                transform.Rotate(-Vector3.right * 30 * Time.deltaTime);

            if (Input.GetKey(KeyCode.Z))
            {
                Vector3 curPos = transform.position;
                Vector3 pos = new Vector3(curPos.x, curPos.y, curPos.z + 0.005f);
                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.X))
            {
                Vector3 curPos = transform.position;
                Vector3 pos = new Vector3(curPos.x, curPos.y, curPos.z - 0.005f);
                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 curPos = transform.position;
                Vector3 pos = new Vector3(curPos.x + 0.005f, curPos.y, curPos.z);
                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 curPos = transform.position;
                Vector3 pos = new Vector3(curPos.x - 0.005f, curPos.y, curPos.z);
                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                this.Hide();
                uiCanvas.GetComponent<FileLoader>().Show();
            }

            if(pageFlipForward)
            {
                pagePivot.transform.Rotate(Vector3.forward, 200 * Time.deltaTime);
                if (Mathf.Round(pagePivot.transform.eulerAngles.z) > 180)
                    pageFlipForward = false;
            }

            if (pageFlipBack)
            {
                pagePivot.transform.Rotate(Vector3.forward, 200 * Time.deltaTime);
                if (Mathf.Round(pagePivot.transform.eulerAngles.z) == 180)
                    pageFlipBack = false;
            }
        }

    }

    public void Reload()
    {
        CallExternalProcess(pdfConverterPath, bookPath + " pageRight.png " + pageNumber.ToString());

        pagePanelRight.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
        pagePanelRight.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }

        

        return tex;
    }

    public static void CallExternalProcess(string processPath, string arguments)
    {
        Process myProcess = new Process();
        myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        myProcess.StartInfo.CreateNoWindow = true;
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.FileName = processPath;

        myProcess.StartInfo.Arguments = arguments;
        myProcess.EnableRaisingEvents = true;
        myProcess.Start();
        myProcess.WaitForExit();
        int ExitCode = myProcess.ExitCode;
    }

    public void Show()
    {
        this.isVisible = true;
        transform.position = startPosition;
    }

    public void Hide()
    {
        this.isVisible = false;
        transform.position = new Vector3(0.22131f, 100.86162f, -8.5970f);
    }

}

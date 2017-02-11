using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;

public class BookScript : MonoBehaviour {

    public GameObject pagePanelRight;
    public GameObject pagePanelLeft;
    public GameObject pageFlipper;
    public Canvas uiCanvas;
    public Transform center;
    public bool isVisibile = false;

    public int pageNumber = 0;

    private string pdfConverterPath;
    private bool pageFlipForward = false;
    private bool pageFlipBack = false;
    public string bookPath = "";
    private Vector3 init;
    private Quaternion initrot;

    // Use this for initialization
    void Start () {

        pdfConverterPath = Application.dataPath + "/../Converter/pdf2img.exe";

        this.Hide();
        uiCanvas.GetComponent<FileLoader>().Show();
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisibile)
        {

            if (Input.GetButtonUp("Fire1"))
            {
                pageNumber = pageNumber + 2;

                // Put current page texture on flipper page
                pageFlipper.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                pageFlipper.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                // Generate new images from pdf
                CallExternalProcess(pdfConverterPath, bookPath + " pageRight.png " + (pageNumber).ToString());
                CallExternalProcess(pdfConverterPath, bookPath + " pageLeft.png " + (pageNumber - 1).ToString());

                pagePanelRight.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                pagePanelRight.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                pagePanelLeft.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageLeft.png");
                pagePanelLeft.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                GameObject.Find("Book").GetComponent<AudioSource>().Play();

                pageFlipForward = true;
                init = pageFlipper.transform.localPosition;
                initrot = pageFlipper.transform.localRotation;

            }


            if (Input.GetButtonUp("Fire2"))
            {
                pageNumber = pageNumber - 2;

                if (pageNumber < 2)
                    pageNumber = 2;

                CallExternalProcess(pdfConverterPath, bookPath + " pageRight.png " + (pageNumber).ToString());
                CallExternalProcess(pdfConverterPath, bookPath + " pageLeft.png " + (pageNumber - 1).ToString());

                // Put current page texture on flipper page
                pageFlipper.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                pageFlipper.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                pagePanelRight.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageRight.png");
                pagePanelRight.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                pagePanelLeft.GetComponent<Renderer>().material.mainTexture = LoadPNG(Application.dataPath + "/../pageLeft.png");
                pagePanelLeft.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));

                GameObject.Find("Book").GetComponent<AudioSource>().Play();

                pageFlipBack = true;
                init = pageFlipper.transform.localPosition;
                initrot = pageFlipper.transform.localRotation;
            }


            if (Input.GetKey(KeyCode.UpArrow))
                transform.Rotate(Vector3.right * 30 * Time.deltaTime);

            if (Input.GetKey(KeyCode.DownArrow))
                transform.Rotate(-Vector3.right * 30 * Time.deltaTime);

            if (Input.GetKey(KeyCode.Z) || DPadButtons.up)
            {
                Vector3 curPos = transform.position;
                Vector3 pos = new Vector3(curPos.x, curPos.y, curPos.z + 0.005f);
                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.X) || DPadButtons.down)
            {
                Vector3 curPos = transform.position;
                Vector3 pos = new Vector3(curPos.x, curPos.y, curPos.z - 0.005f);
                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.RightArrow) || DPadButtons.right)
            {
                Vector3 curPos = transform.position;
                Vector3 pos = new Vector3(curPos.x + 0.005f, curPos.y, curPos.z);
                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || DPadButtons.left)
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
                pageFlipper.transform.RotateAround(center.position, center.up, 150 * Time.deltaTime);

                if (pageFlipper.transform.rotation.z < 0)
                {
                    pageFlipForward = false;
                    pageFlipper.transform.localPosition = init;
                    pageFlipper.transform.localRotation = initrot;
                }
            }

            if (pageFlipBack)
            {
                pageFlipper.transform.RotateAround(center.position, -center.up, 150 * Time.deltaTime);

                if (pageFlipper.transform.rotation.z < 0)
                {
                    pageFlipBack = false;
                    pageFlipper.transform.localPosition = init;
                    pageFlipper.transform.localRotation = initrot;
                }
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
        this.isVisibile = true;
        transform.position = new Vector3(0.22131f, 0.86162f, -8.5970f);
    }

    public void Hide()
    {
        this.isVisibile = false;
        transform.position = new Vector3(0.22131f, 100.86162f, -8.5970f);
    }

}

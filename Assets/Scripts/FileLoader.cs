using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class FileLoader : MonoBehaviour {

    public string drive = "C:";
    public string path = "\\";

    public Button[] DirButtons;
    public Button[] FileButtons;
    public Dropdown DriveLetter;

    public int dirTopIndex = 0;
    public int fileTopIndex = 0;

    // Use this for initialization
    void Start () {

        string[] drives = Directory.GetLogicalDrives();

        foreach (string drive in drives)
        {
            if(drive != "C:\\")
            {
                Dropdown.OptionData od = new Dropdown.OptionData();
                od.text = drive;
                DriveLetter.options.Add(od);
            }

        }

        UpdateFileList();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        transform.position = new Vector3(-0.011f, 0.84f, -4.37f);
        GetComponent<OculusTouch>().pointerEnabled = true;
    }

    public void Hide()
    {
        transform.position = new Vector3(-0.011f, 100.84f, -4.37f);
        GetComponent<OculusTouch>().pointerEnabled = false;
    }

    public void UpdateFileList()
    {
        string[] dirs = Directory.GetDirectories(drive + path);
        string[] files = Directory.GetFiles(drive + path, "*.pdf");


        for(int z= 0; z<dirs.Length;z++)
        {
            dirs[z] = dirs[z].Remove(0, dirs[z].LastIndexOf('\\') + 1);
        }

        for (int z = 0; z < files.Length; z++)
        {
            files[z] = files[z].Remove(0, files[z].LastIndexOf('\\') + 1);
        }

        if (dirTopIndex < 0)
            dirTopIndex = 0;

        if (dirTopIndex > dirs.Length-1)
            dirTopIndex = dirs.Length-1;

        if (fileTopIndex < 0)
            fileTopIndex = 0;

        if (fileTopIndex > files.Length - 1)
            fileTopIndex = files.Length - 1;

        for (int x = 0; x < DirButtons.Length; x++)
        {
            if (dirTopIndex < 0)
                dirTopIndex = 0;

            if (dirs.Length > dirTopIndex + x)
                DirButtons[x].GetComponentInChildren<Text>().text = dirs[dirTopIndex + x];
            else
                DirButtons[x].GetComponentInChildren<Text>().text = "";
        }

        for (int x = 0; x < FileButtons.Length; x++)
        {
            if (fileTopIndex < 0)
                fileTopIndex = 0;

            if (files.Length > fileTopIndex + x)
                FileButtons[x].GetComponentInChildren<Text>().text = files[fileTopIndex + x];
            else
                FileButtons[x].GetComponentInChildren<Text>().text = "";
        }
    }

    public void DirListScrollDown()
    {
        dirTopIndex++;

        UpdateFileList();
    }

    public void DirListScrollUp()
    {
        dirTopIndex--;

        UpdateFileList();
    }

    public void FileListScrollDown()
    {
        fileTopIndex++;

        UpdateFileList();
    }

    public void FileListScrollUp()
    {
        fileTopIndex--;

        UpdateFileList();
    }

    public void ChangeDrive()
    {
        this.drive = DriveLetter.GetComponentInChildren<Text>().text.Substring(0,2);
        path = "\\";
        UpdateFileList();
    }

    public void ChangeDrive(Button b)
    {
        this.drive = b.GetComponentInChildren<Text>().text.Substring(0, 2);
        path = "\\";
        UpdateFileList();
    }

    public void ChangeDirectory(Button button)
    {
        if(button.name == "ButtonPrevDir")
        {
            path = path.Substring(0, path.LastIndexOf("\\"));
            path = path.Substring(0, path.LastIndexOf("\\") + 1);
        }
        else
        {
            path += button.GetComponentInChildren<Text>().text + "\\";
        }

        
        UpdateFileList();
    }
    public void ChangeFile(Button button)
    {
        string file = button.GetComponentInChildren<Text>().text;

        this.Hide();
        GameObject.Find("Book").GetComponent<BookScript>().bookPath = "\"" + drive + path + file + "\"";
        GameObject.Find("Book").GetComponent<BookScript>().pageNumber = 1;
        GameObject.Find("Book").GetComponent<BookScript>().Reload();
        GameObject.Find("Book").GetComponent<BookScript>().Show();
    }

}

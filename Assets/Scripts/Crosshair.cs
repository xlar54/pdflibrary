using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {

    private FileLoader fileLoader;

	// Use this for initialization
	void Start () {

        fileLoader = GameObject.Find("FileLoaderCanvas").GetComponent<FileLoader>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "selectable")
        {
            GameObject d = coll.gameObject;
            d.GetComponent<Image>().color = Color.yellow;
        }

    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (coll.gameObject.name == "Dropdown")
            {
                Dropdown d = coll.gameObject.GetComponent<Dropdown>();
                d.Show();
            }

            if (coll.gameObject.name == "btnExit")
            {
                Application.Quit();
            }

            if (coll.gameObject.name == "dirScrollDown")
            {
                fileLoader.DirListScrollDown();
            }

            if (coll.gameObject.name == "dirScrollUp")
            {
                fileLoader.DirListScrollUp();
            }

            if (coll.gameObject.name == "fileScrollDown")
            {
                fileLoader.FileListScrollDown();
            }

            if (coll.gameObject.name == "fileScrollUp")
            {
                fileLoader.FileListScrollUp();
            }


            if (coll.gameObject.name.Contains("drive_"))
            {
                Button b = coll.gameObject.GetComponent<Button>();
                fileLoader.ChangeDrive(b);
            }

            if (coll.gameObject.name.Contains("file_"))
            {
                Button b = coll.gameObject.GetComponent<Button>();
                fileLoader.ChangeFile(b);
            }

            if (coll.gameObject.name.Contains("dir_"))
            {
                Button b = coll.gameObject.GetComponent<Button>();
                fileLoader.ChangeDirectory(b);
            }

            if (coll.gameObject.name == ("btnCancel"))
            {
                fileLoader.Hide();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "selectable")
        {
            GameObject d = coll.gameObject;
            d.GetComponent<Image>().color = Color.white;
        }


        if (coll.gameObject.name == "Dropdown")
        {
            Dropdown d = coll.gameObject.GetComponent<Dropdown>();
            //d.Hide();
        }
    }
}

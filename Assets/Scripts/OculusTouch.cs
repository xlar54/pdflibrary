using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OculusTouch : MonoBehaviour {

    public GameObject crosshair;
    public GameObject fileLoaderCanvas;
    public GameObject touchController;
    public GameObject book;
    public bool pointerEnabled;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (OVRInput.Get(OVRInput.Button.One) && book.GetComponent<BookScript>().isVisible)
        {
            fileLoaderCanvas.GetComponent<FileLoader>().Show();
            book.GetComponent<BookScript>().Hide();
        }

        if (pointerEnabled)
        {
            Vector3 canvasPos = fileLoaderCanvas.transform.position;
            Vector3 controlPos = touchController.transform.position;
            Transform controlTransform = touchController.transform;
            float distance = Vector3.Distance(controlPos, canvasPos);

            Vector3 endPos = controlTransform.up * distance + controlTransform.position;
            endPos.z = 0;
            crosshair.transform.position = endPos;
            Vector3 lc = crosshair.transform.localPosition;
            lc.z = 0;
            crosshair.transform.localPosition = lc;

            DrawLine(controlPos, crosshair.transform.TransformPoint(Vector3.zero), Color.cyan, 0.01f);
        }

	}

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    Vector3 DrawForwardLine(Transform t, float distance, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = t.position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;
        lr.SetPosition(0, t.position);

        Vector3 endPosition = t.up * distance + t.position;

        lr.SetPosition(1, endPosition);
        GameObject.Destroy(myLine, duration);

        return endPosition;
    }
}

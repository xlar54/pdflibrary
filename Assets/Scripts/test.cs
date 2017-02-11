using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public GameObject pageA;
    public GameObject pageB;
    public GameObject pageC;
    public Transform center;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


            pageB.transform.RotateAround(center.position, center.up, 170 * Time.deltaTime);


    }



}

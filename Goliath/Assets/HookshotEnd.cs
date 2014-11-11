using UnityEngine;
using System.Collections;

public class HookshotEnd : MonoBehaviour {

    private GameObject hookshotBegin;
	// Use this for initialization
	void Start () {
        foreach (Transform child in transform.parent)
        {
            if (child.name == "PointToHookshot")
            {
                hookshotBegin = child.gameObject;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            hookshotBegin.GetComponent<Hookshot3D>().finishDrag();
        }
    }

    void OnTriggerExit(Collider col)
    {

    }
}

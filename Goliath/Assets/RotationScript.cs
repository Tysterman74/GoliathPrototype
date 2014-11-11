using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

    private bool switchOn = false;
    private GameObject rotationObject;
    private GameObject rightRotationTrigger;
    private GameObject leftRotationTrigger;
	// Use this for initialization
	void Start () {
        rotationObject = GameObject.Find("Rotation");
        rightRotationTrigger = GameObject.Find("RotationTriggerRight");
        leftRotationTrigger = GameObject.Find("RotationTriggerLeft");

        //5.9294, 0.5595, -3.3482
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !switchOn)
        {
            switchOn = true;
            if (col.GetComponent<PlayerController3D>().getFacingRight())
            {
                rotationObject.transform.Rotate(Vector3.up, 90f, Space.Self);
                col.gameObject.transform.Rotate(Vector3.up, 180f, Space.Self);
            }
            else
            {
                rotationObject.transform.Rotate(Vector3.up, -90f, Space.Self);
                col.gameObject.transform.Rotate(Vector3.up, -180f, Space.Self);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && switchOn)
        {
            switchOn = false;
        }
    }
}

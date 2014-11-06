using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour {

    private Hookshot hookshot;

	// Use this for initialization
	void Start () {
	    foreach (Transform child in transform.parent) 
        {
            if (child.gameObject.tag == "PointToHookshot")
            {
                hookshot = child.gameObject.GetComponent<Hookshot>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "MovableObject")
        {
            hookshot.finishDrag();
            col.gameObject.tag = "Untagged";
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && this.gameObject.tag != "Untagged")
        {
            Destroy(col.gameObject);
        }
    }
}

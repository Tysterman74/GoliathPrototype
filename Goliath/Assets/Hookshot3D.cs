using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hookshot3D : MonoBehaviour {

    public bool moveTo = true;
    public GameObject objectBeingMoved;
    public int force = 3000;
    public bool pullingObject = false;

    private GameObject destination;
    private GameObject player;
    private GameObject exclamation;
    private GameObject positionToLand;

    private LineRenderer rope;

    public bool grapple = false;
    public bool floating = false;
    public bool throwRope = false;
    public bool drag = false;
    public bool done = false;
    public bool release = false;
    public bool finishingDrag = false;

    private Vector2 ropePos;

	// Use this for initialization
	void Start () {
	    //destination = GameObject.F
        player = GameObject.Find("Player");
        foreach (Transform child in transform.parent)
        {
            if (child.name == "HookshotDestination")
            {
                destination = child.gameObject;
            }

            if (child.name == "Exclamation")
            {
                exclamation = child.gameObject;
            }

            if (child.name == "PlaceToLand")
            {
                positionToLand = child.gameObject;   
            }
        }
        rope = this.GetComponent<LineRenderer>();
        rope.enabled = false;

        ropePos = player.transform.position;
        exclamation.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (grapple)
        {
            if (Input.GetMouseButtonUp(1))
            {
                throwRope = true;
                grapple = false;
                rope.enabled = true;
                ropePos = player.transform.position;
            }
        }
        else if (throwRope)
        {
            player.gameObject.GetComponent<PlayerController3D>().enabled = false;
            player.rigidbody.velocity = new Vector2(0, player.rigidbody.velocity.y);
            float step = 20f * Time.deltaTime;
            Vector2 newRopePos = Vector2.MoveTowards(ropePos, destination.transform.position, step);
            ropePos = newRopePos;
            rope.SetPosition(0, player.transform.position);
            rope.SetPosition(1, ropePos);
            if (ropePos == (Vector2) destination.transform.position)
            {
                throwRope = false;
                if (moveTo)
                    floating = true;
                else
                    drag = true;
                 
            }
        }
        else if (floating)
        {
            rope.SetPosition(0, player.transform.position);
            rope.SetPosition(1, destination.transform.position);
            float step = 20 * Time.deltaTime;
            player.transform.position = Vector3.MoveTowards(player.transform.position, destination.transform.position, step);
            //Debug.Log(destination.transform.position);
            if (player.transform.position == destination.transform.position)
            {
                Debug.Log("Get money");
                player.gameObject.GetComponent<PlayerController3D>().enabled = true;
                rope.enabled = false;
                floating = false;
            }
        }
        else if (drag)
        {
           // if ()
            if (!pullingObject)
            //if (objectBeingMoved.rigidbody2D.gravityScale == 0)
            {
                objectBeingMoved.rigidbody2D.gravityScale = 1;
                rope.SetPosition(0, player.transform.position);
                rope.SetPosition(1, ropePos);
                float step = 50 * Time.deltaTime;
                ropePos = Vector2.MoveTowards(ropePos, player.transform.position, step);

                if (ropePos == new Vector2(player.transform.position.x, player.transform.position.y))
                {
                    player.gameObject.GetComponent<PlayerController3D>().enabled = true;
                    rope.enabled = false;
                    done = true;
                }
            }
            else
            {
                //Put place to land here.
                Vector2 pullForce = Vector2.MoveTowards(objectBeingMoved.transform.position, positionToLand.transform.position, 20f);
                if (pullForce.x < 0)
                    objectBeingMoved.rigidbody.AddForce(new Vector2(-force, 0));
                else
                    objectBeingMoved.rigidbody.AddForce(new Vector2(force, 0));

                ropePos = objectBeingMoved.transform.position;
                exclamation.SetActive(false);
                rope.SetPosition(0, player.transform.position);
                rope.SetPosition(1, ropePos);


                //rope.SetPosition(0, player.transform.position);
                //rope.SetPosition(1, ropePos);
                //float step = 20 * Time.deltaTime;
                //ropePos = Vector2.MoveTowards(ropePos, player.transform.position, step);
            }
        }
        else if (finishingDrag)
        {
            rope.SetPosition(0, player.transform.position);
            rope.SetPosition(1, ropePos);
            float step = 50 * Time.deltaTime;
            ropePos = Vector2.MoveTowards(ropePos, player.transform.position, step);

            if (ropePos == new Vector2(player.transform.position.x, player.transform.position.y))
            {
                player.gameObject.GetComponent<PlayerController>().enabled = true;
                rope.enabled = false;
                done = true;
            }
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if (!done)
        {
            Debug.Log(col.gameObject.tag);
            if (col.gameObject.tag == "Player")
            {
                exclamation.SetActive(true);
                grapple = true;
            } 
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            exclamation.SetActive(false);
            grapple = false;
        }
    }

    public void finishDrag()
    {
        drag = false;
        if (pullingObject)
            finishingDrag = true;
        else
        {
            player.gameObject.GetComponent<PlayerController3D>().enabled = true;
            rope.enabled = false;
            done = true;
        }
    }

    public void finishFloat()
    {
        player.gameObject.GetComponent<PlayerController3D>().enabled = true;
        rope.enabled = false;
        floating = false;
    }
}

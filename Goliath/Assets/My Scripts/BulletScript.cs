using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    private AI enemyHit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
	    {
		    if (col.gameObject.tag == "Enemy")
            {
                enemyHit = col.gameObject.GetComponent<AI>();
                enemyHit.Hit();
                Debug.Log("Hit Enemy");
            }
            Destroy(this.gameObject);
	    }
    }
}

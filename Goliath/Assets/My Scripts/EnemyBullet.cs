using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    private PlayerController player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Enemy")
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("Player is hit");
                player.Hit();
            }
            Destroy(this.gameObject);
        }
    }
}

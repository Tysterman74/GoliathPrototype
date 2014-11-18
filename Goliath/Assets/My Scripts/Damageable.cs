using UnityEngine;
using System.Collections;

//Class that allows objects to take damage
public class Damageable : MonoBehaviour {

    public int health = 10;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            Die();
        }
	}


    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Weakness hit");
        if (col.gameObject.CompareTag("Damage"))
        {
            Damage attack = col.gameObject.GetComponent<Damage>();
            
            health = health - attack.damage * attack.multiplier;
            Debug.Log(col.gameObject.name);
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}

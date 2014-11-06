using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour 
{
	public float timeTillIdle = 3.0f;
	public float timeToIdle = 2.0f;
	public float staticSpeed = 5.0f;
    public bool staySameDirection = false;
    public bool facingRight = true;
	public GameObject bullet;

	enum State {idle, moving, chasing, attack, returning};
	private State currentState = State.idle;
	private float speed;
	private float chaseSpeed = 7.0f;
	private Vector2 chaseVel;
	private Vector3 startPos;
	private Vector2 maxSpeed = new Vector2(3.0f, 3.0f);
	private float tillIdleTimer;
	private float idleTimer;
	public float attackDistance = 1.2f;
	private RaycastHit2D[] rayCastHits;
	private Transform target = null;

    private int health = 100;
    private const int fireCoolDownConst = 150;
    private Animator anim;
    private int fireCoolDown = 0;

	// Use this for initialization
	void Start () 
	{
		tillIdleTimer = timeTillIdle;
		idleTimer = timeToIdle;
		startPos = transform.position;
		speed = staticSpeed;
		rayCastHits = Physics2D.RaycastAll(transform.position, transform.right);
        anim = GameObject.Find("Enemy").GetComponent<Animator>();
        if (!facingRight)
        {
            Flip();
            facingRight = false;
            //Turn();
        }
    }

	//void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Vector2 dir = new Vector2(transform.right.x * 100.0f, transform.right.y);
	//	Gizmos.DrawRay(transform.position, dir);
	//}

	// Update is called once per frame
	void Update ()
	{
        if (transform.rigidbody2D.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (transform.rigidbody2D.velocity.x < 0 && facingRight)
        {
            Flip();
        }

        if (fireCoolDown > 0)
        {
            fireCoolDown--;
        }
        else
        {
            fireCoolDown = 0;
        }
		CheckRayCastHits();
		HandleState();
		print("Current State: " + currentState);
	}

	private void HandleState()
	{
		switch (currentState)
		{
		case State.moving:
			Move();
			break;
		case State.idle:
			Idle();
			break;
		case State.chasing:
            anim.SetFloat("Speed", 1);
			Chase();
			break;
		case State.attack:
			Attack();
			break;
		case State.returning:
			ReturnToStart();
			break;
		}
	}

	private bool CheckRayCastHits()
	{
        if (facingRight)
		    rayCastHits = Physics2D.RaycastAll(transform.position, transform.right);
        else
            rayCastHits = Physics2D.RaycastAll(transform.position, transform.right * -1);

		foreach(RaycastHit2D hit in rayCastHits)
		{
            if (hit.transform.tag == "Obstacle")
            {
                //Do nothing, put this part in because raycast go through objects.
                break;
            }
			else if (hit.transform.tag == "Player")
			{
				target = hit.transform;
                if (currentState != State.attack)
				    currentState = State.chasing;
				chaseVel = target.rigidbody2D.position - transform.rigidbody2D.position;
				return true;
			}
		}

		if(currentState == State.chasing || currentState == State.attack)
			currentState = State.returning;
		return false;
	}

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

	private void Move()
	{
		tillIdleTimer -= Time.deltaTime;
		if(tillIdleTimer <= 0)
		{
			currentState = State.idle;
			transform.rigidbody2D.velocity = transform.rigidbody2D.velocity * 0.0f;
        }
        else
        {
            if (VecLessThan(transform.rigidbody2D.velocity, maxSpeed))
            {
                //transform.rigidbody2D.AddForce(new Vector2(speed, 0.0f));
                transform.rigidbody2D.velocity = new Vector2(speed, 0.0f);
                anim.SetFloat("Speed", 1);

            }
        }
	}

	private void Idle()
	{
		idleTimer -= Time.deltaTime;
		if (idleTimer <= 0)
		{
            if (!staySameDirection)
            {
                Turn();
                if (speed > 0 && !facingRight)
                {
                    Flip();
                }
                else if (speed < 0 && facingRight)
                {
                    Flip();
                }
            }
		}
        anim.SetFloat("Speed", -1);
	}

	private void Turn()
	{
		//print(transform.right);
		//transform.Rotate(0.0f, 180.0f, 0.0f);
		//print(transform.right);
		idleTimer = timeToIdle;
		tillIdleTimer = timeTillIdle;
		speed *= -1;
		currentState = State.moving;
	}

    void FixedUpdate()
    {
        if (transform.rigidbody2D.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (transform.rigidbody2D.velocity.x < 0 && facingRight)
        {
            Flip();
        }

        if (health <= 0)
        {
            anim.SetTrigger("Die");
            Destroy(this.gameObject);
        }
    }

	private void Chase()
	{
		if(chaseVel.magnitude <= attackDistance)
		{
			currentState = State.attack;
			transform.rigidbody2D.velocity = transform.rigidbody2D.velocity * 0.0f;
			return;
		}
		chaseVel.Normalize();
		chaseVel = new Vector2(chaseVel.x *(chaseSpeed), chaseVel.y * (chaseSpeed));
        if (VecLessThan(transform.rigidbody2D.velocity, maxSpeed))
            //transform.rigidbody2D.AddForce(chaseVel);
            transform.rigidbody2D.velocity = chaseVel;
        if (transform.rigidbody2D.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (transform.rigidbody2D.velocity.x < 0 && facingRight)
        {
            Flip();
        }

	}

	private void Attack()
	{
        //Debug.Log("Chase Vel Magnitude: " + chaseVel.magnitude);
        //Debug.Log("Attack Distance: " + attackDistance);
		if(CheckRayCastHits() && chaseVel.magnitude <= attackDistance)
		{
            if (fireCoolDown <= 0)
            {
                GameObject newBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
                if (facingRight)
                    newBullet.rigidbody2D.AddForce(new Vector2(700.0f, 0f));
                else
                    newBullet.rigidbody2D.AddForce(new Vector2(-700.0f, 0f));
                //DealDamage();
                anim.SetTrigger("Attack");
                fireCoolDown = fireCoolDownConst;
            }
            anim.SetFloat("Speed", -1);
		}
		else
		{
			currentState = State.returning;
		}
	}

	void DealDamage()
	{
		//target.transform
		print ("Deal Damage!!!");
	}

	private void ReturnToStart()
	{
		Vector2 toStart = startPos - transform.position;
		toStart.Normalize();
		toStart = new Vector2(toStart.x *(chaseSpeed), toStart.y * (chaseSpeed));
		//print("Right: " + transform.right + ". toStart: " + toStart.x);
		if ((transform.right.x < 0 && toStart.x > 0) || (transform.right.x > 0 && toStart.x < 0))
			transform.Rotate(0.0f, 180.0f, 0.0f);
        if (VecLessThan(transform.rigidbody2D.velocity, maxSpeed))
        {
            //transform.rigidbody2D.AddForce(toStart);
            transform.rigidbody2D.velocity = toStart;
            anim.SetFloat("Speed", 1);
        }
		if (CheckNearPosition(transform.position, startPos))
		{
            //anim.SetFloat("Speed", -1);
			currentState = State.idle;
			speed = staticSpeed;
			print("Back");
		}
        if (transform.rigidbody2D.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (transform.rigidbody2D.velocity.x < 0 && facingRight)
        {
            Flip();
        }
	}

    public void Hit()
    {
        anim.SetTrigger("Hit");
        health -= 20;
    }

	private bool VecLessThan(Vector2 v1, Vector2 v2)
	{
		if(v1.magnitude < v2.magnitude)
			return true;
		return false;
	}

	private bool CheckNearPosition(Vector3 current, Vector3 dest)
	{
		if (current.x > dest.x - 0.1f && current.x < dest.x + 0.1f)
			return true;
		return false;
	}
	
}

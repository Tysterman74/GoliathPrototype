﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {


    private const float walkSpeed = 5f;
    private const float runSpeed = 12f;

    private int degree = 0;
    public float maxSpeed = 5f;
    public float jumpForce = 700f;
    public int ticks = 0;

    private int stamina = 100;
    public int health = 100;
    private Text staminaText;
    private Slider staminaSlider;
    private Text healthText;
    private Slider healthSlider;


    private bool running = false;
    private bool fired = false;
    private bool facingRight = true;

    public GameObject bullet;
    private Animator anim;

    bool grounded = false;
    public Transform groundCheck;
    //How big sphere will be
    float groundRadius = 0.2f;
    //What is considered ground?
    public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        staminaText = GameObject.Find("StaminaValue").GetComponent<Text>();
        staminaSlider = GameObject.Find("StaminaSlider").GetComponent<Slider>();
        staminaText.text = stamina.ToString();

        healthText = GameObject.Find("HealthValue").GetComponent<Text>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        healthText.text = health.ToString();

	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        //Vertical Speed
        anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

        //In fixed update, dont need to do Time.deltaTime
        //Do all physics in fixed updated.
        if (running && grounded && stamina > 0)
        {
            maxSpeed = runSpeed;
            stamina -= 2;

        }
        else
        {
            maxSpeed = walkSpeed;
            if (stamina < 100)
                stamina += 1;
        }

        float move = Input.GetAxis("Horizontal");



        anim.SetFloat("Speed", Mathf.Abs(move));

        rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        staminaText.text = stamina.ToString();
        staminaSlider.value = stamina;

        healthText.text = health.ToString();
        healthSlider.value = health;
	}

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (grounded && Input.GetKeyUp(KeyCode.Space))
        {
            grounded = false;
            anim.SetBool("Ground", false);
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!fired)
            {
                Fire();
                fired = true;
            }
        }

        if (ticks >= 175 && fired)
        {
            fired = false;
            ticks = 0;
        }
        ticks++;
    }

    void Fire()
    {
        anim.SetTrigger("Attack");
        GameObject newBullet = (GameObject)Instantiate(bullet, transform.position, bullet.transform.rotation);
        if (facingRight)
            newBullet.rigidbody2D.AddForce(new Vector2(700.0f, 0f));
        else
            newBullet.rigidbody2D.AddForce(new Vector2(-700.0f, 0f));
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Hit()
    {
        anim.SetTrigger("Hit");
        health -= 10;
    }

    public bool getFacingRight()
    {
        return facingRight;
    }

    public void AddDegree(int degreeToAdd)
    {
        degree += degreeToAdd;
    }
}

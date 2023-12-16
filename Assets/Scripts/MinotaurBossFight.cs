using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurBossFight : MonoBehaviour
{

    public float stunTime = 2.0f;
    public float chargeInterval = 1.0f;
    public float speed = 3.0f;
    public float chargeSpeed = 6.0f;
    public bool vertical;

    private bool isCharging = false;

    private Animator minotaurAnimator;
    private Rigidbody2D minotaurBody;
    private Vector2 playerSnapshot;

    // Start is called before the first frame update
    void Start()
    {
        minotaurBody = GetComponent<Rigidbody2D>();
        minotaurAnimator = GetComponent<Animator>();
        InvokeRepeating("DecideNextAction", 0, chargeInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCharging)
        { 
            // If not charging, walk towards the player
            Vector2 direction = (playerSnapshot - (Vector2)transform.position).normalized;
            minotaurBody.velocity = direction * speed;

            // Set the parameters based on the movement direction
            minotaurAnimator.SetFloat("Move X", direction.x);
            minotaurAnimator.SetFloat("Move Y", direction.y);

            // Play the appropriate animation based on the movement direction
            UpdateAnimation();
        }
    }

    private void DecideNextAction()
    {
        if (!isCharging)
        {
            TakePlayerSnapshot();
        }
        else
        {
            isCharging = false;
            StartCoroutine(StunCoroutine());
        }
    }

    private void BossCharge()
    {
        Vector2 direction = (playerSnapshot - (Vector2)transform.position).normalized;
        minotaurBody.velocity = direction * chargeSpeed;
    }

    private void StartCharge()
    {
        Invoke("TakePlayerSnapshot", 5);
    }

    private void TakePlayerSnapshot()
    {
        // Store the player's position as a snapshot
        playerSnapshot = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;

        // Start the charge attack
        isCharging = true;
        BossCharge();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the boss collides with a wall
        if (other.CompareTag("Pillar"))
        {
            // Stun the boss when it hits the wall
            StunBoss();
        }
    }


    private void StunBoss()
    {
        isCharging = false;
        StartCoroutine(StunCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        // Add any visual or audio feedback for the stun
        Debug.Log("Boss stunned!");

        GetComponent<SpriteRenderer>().color = Color.red;
        minotaurBody.velocity = Vector2.zero; // Stop the boss's movement
        yield return new WaitForSeconds(stunTime);
        GetComponent<SpriteRenderer>().color = Color.white;

        // Resume charging after the stun duration
        isCharging = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        minotaurBody.velocity = Vector2.zero;
        isCharging = false;


        if (player != null)
        {
            Debug.Log("You got hit!");
        }

        // Check if the boss collides with a wall
        if (collision.gameObject.CompareTag("Pillar"))
        {
            // Stun the boss when it hits the wall
            StunBoss();
        }
    }

    private void UpdateAnimation()
    {
        float moveX = minotaurAnimator.GetFloat("Move X");
        float moveY = minotaurAnimator.GetFloat("Move Y");

        if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
        {
            // Horizontal movement is more significant, play left or right animation
            if (moveX > 0)
            {
                minotaurAnimator.Play("minotaurRight");
            }
            else
            {
                minotaurAnimator.Play("minotaurLeft");
            }
        }
        else
        {
            // Vertical movement is more significant, play up or down animation
            if (moveY > 0)
            {
                minotaurAnimator.Play("minotaurUp");
            }
            else
            {
                minotaurAnimator.Play("minotaurDown");
            }
        }
    }
}

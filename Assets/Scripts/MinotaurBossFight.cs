using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurBossFight : MonoBehaviour
{

    public float stunTime = 2.0f;
    public float chargeInterval = 1.0f;
    public float speed = 3.0f;
    public float chargeSpeed = 6.0f;

    private bool isCharging = false;

    Rigidbody2D minotaurBody;
    private Vector2 playerSnapshot;

    // Start is called before the first frame update
    void Start()
    {
        minotaurBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("StartCharge", chargeInterval, chargeInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging)
        {
            Vector2 direction = (playerSnapshot - (Vector2)transform.position).normalized;
            minotaurBody.velocity = direction * chargeSpeed;
        }
        
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
}

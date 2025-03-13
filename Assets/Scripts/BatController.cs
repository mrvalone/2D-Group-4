using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BatController : MonoBehaviour
{

    public AudioSource AudioSource;
    public AudioClip squeakSFX;
    
    private Rigidbody2D rb;
    private bool batHeight;
    private IEnumerator death;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        batHeight = false;
    }

    void FixedUpdate()
    {
        if (batHeight) //checks to see if the bat is in a height zone
        {
            rb.AddForceX(Random.Range(-50.0f, 50.0f)); 
            rb.linearVelocityY = -5.0f; //makes sure the bat descends 
        }
        else //otherwise the bat moves randomly up and down, left to right
        {
            rb.AddForceX(Random.Range(-50.0f, 50.0f));
            rb.AddForceY(Random.Range(-15.0f, 35.0f));
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("batCeiling"))
        {
            batHeight = true;
        }

        if (collision.gameObject.CompareTag("playerSwing"))
        {
            Debug.Log("Bat is kill");
            death = DestroyBat(1.0f);
            StartCoroutine(death);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("batCeiling"))
        {
            batHeight = false;
        }
    }

    IEnumerator DestroyBat(float wait)
    {
        AudioSource.PlayOneShot(squeakSFX);
        yield return new WaitForSeconds(wait);

        Destroy(gameObject);
    }
}

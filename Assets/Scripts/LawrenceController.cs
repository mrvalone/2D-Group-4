using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LawrenceController : MonoBehaviour
{
    private float speed;
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private IEnumerator death;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 3.0f;
        
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        rb.linearVelocityX = speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("lawrenceBumper"))
        {
            rb.linearVelocityX *= -1;
            
            if (rend.flipX)
            {
                rend.flipX = false;
            }
            else
            {
                rend.flipX = true;
            }
        }

        if (collision.gameObject.CompareTag("playerSwing"))
        {
            Debug.Log("Lawrence is kill");
            death = DestroyLawrence(0.5f);
            StartCoroutine(death);
        }

    }

    IEnumerator DestroyLawrence(float wait)
    {
        yield return new WaitForSeconds(wait);

        Destroy(gameObject);
    }
}

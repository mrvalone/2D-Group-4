using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    public Image heart1;
    public float speed; //max speed for left-right movement
    public float jumpHeight; //height of jump
    public GameObject swordSwingR;
    public GameObject swordSwingL;
    public GameObject chalkMark;
    public AudioSource AudioSource;
    public AudioClip swingSFX;
    public AudioClip potionSFX;
    public AudioClip damageSFX;

    private string itemHeld;  
    private int itemCount;
    private int hp;
    private SpriteRenderer rend;
    private Rigidbody2D rb;
    private Animator anim;
    private bool airBorn;
    private bool faceRight;
    private bool faceLeft;
    void Start()
    {
        hp = 9;
        faceRight = true;
        faceLeft = false;
        itemHeld = "empty";
        itemCount = 0;
        airBorn = false;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        speed = 8.0f;
        jumpHeight = 15.0f;
    }

    void FixedUpdate()
    {

    }

    private void Update()
    {   
        anim.SetBool("IsJumping", airBorn);
        
        if (!airBorn)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !airBorn)
        {
            rb.linearVelocityY = jumpHeight;
            airBorn = true;
            Debug.Log("jumped");
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Attack();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            UseItem();
        }


        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocityX = speed;
            faceRight = true;
            faceLeft = false;
            rend.flipX = true;
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            rb.linearVelocityX = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocityX = (speed * -1);
            faceLeft = true;
            faceRight = false;
            rend.flipX = false;
        }
        
        if (Input.GetKeyUp(KeyCode.A))
        {
            rb.linearVelocityX = 0;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            airBorn = true;
            Debug.Log("airborn");
        }

        if (collision.gameObject.CompareTag("crystal"))
        {
            Destroy(collision.gameObject);
            // Acid rising method
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("levelEnd")) // When the player hits the end of level block
        {
            SceneManager.LoadScene("LevelTwo");
        }

        if (collision.gameObject.CompareTag("ground"))
        {
            airBorn = false;
            Debug.Log("touching grass");
        }

        if (collision.gameObject.CompareTag("bat"))
        {
            ChangeHp(-1);
            Debug.Log("bat hit - 1 damage");
        }

        if (collision.gameObject.CompareTag("lawrence"))
        {
            ChangeHp(-2);
            Debug.Log("lawrence hit - 2 damage");
        }

        if (collision.gameObject.CompareTag("torch"))
        {
            if (itemHeld != "torch")
            {
                itemHeld = "torch";
                itemCount = 1;
            }
            itemCount++;
        }

        if (collision.gameObject.CompareTag("potion"))
        {
            collision.GetComponent<Animator>().SetTrigger("trigPCollect"); // Trigger pickup animation
            collision.GetComponent<BoxCollider2D>().enabled = false; // Delete invisible potion collider
            if (itemHeld != "potion")
            {
                itemHeld = "potion";
                itemCount = 0;
                Debug.Log("Potion Acquired");
            }        
            itemCount++;
            Debug.Log("Potion Count: " + itemCount);
        }

        if (collision.gameObject.CompareTag("chalk"))
        {
            if (itemHeld != "chalk")
            {
                itemHeld = "chalk";
                itemCount = 1;
            }
            itemCount++;
        }
    }

    private void Attack() //instantiates the prefab sword attack
    {
        if (faceRight) //checks to see which way the character is facing and offsets the hitbox in game space accordingly
        {
            Instantiate(swordSwingR, (gameObject.transform.position + new Vector3(0.9f, 0.4f, 0.5f)), Quaternion.Euler(new Vector3(0,0,65.0f))); //instantiates relative to the character's position based on the direction he's facing
            AudioSource.PlayOneShot(swingSFX);
        }

        if (faceLeft)
        {
            Instantiate(swordSwingL, (gameObject.transform.position + new Vector3(-0.9f, 0.4f, 0.5f)), Quaternion.Euler(new Vector3(0, 0, -65.0f)));
            AudioSource.PlayOneShot(swingSFX);
        }
    }

    private void ChangeHp(int change) //changes HP by the amount passed as parameter. Pass negative values to reduce HP
    {
        hp += change;

        if(hp <= 0)
        {
            GameOver();
        }

        switch (hp) //switches out health heart images based on numerical HP value
        {
            case 9:

                break;

            case 8:

                break;

            case 7:

                break;

            case 6:

                break;

            case 5:

                break;

            case 4:

                break;

            case 3:

                break;

            case 2:

                break;

            case 1:

                break;
        }
    
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UseItem()
    {
        switch (itemHeld)
        {
            case "torch":

                itemCount--;
                
                break;

            case "potion":

                ChangeHp(3);
                itemCount--;
                Debug.Log("Potion Used. Remaining: " +  itemCount);
                break;

            case "chalk":

                itemCount--;
                
                break;
        }
    }
}

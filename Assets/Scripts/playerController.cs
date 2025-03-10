using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    public Image heart1;
    public float accel; //responsiveness of left-right movement
    public float speedLimit; //max speed for left-right movement
    public float jumpHeight; //height of jump
    public GameObject swordSwing;
    public GameObject chalkMark;

    private string itemHeld;
    private int itemCount;
    private int hp;
    private Rigidbody2D rb;
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
        accel = 20.0f;
        speedLimit = 5.0f;
        jumpHeight = 11.0f;
    }

    void FixedUpdate()
    {
        //stores player input off the horizontal input axis
        float moveHorizontal = Input.GetAxis("Horizontal");

        //adds force to the player rigidbody if below the speed limit
        if (rb.linearVelocityX < speedLimit && rb.linearVelocityX > (speedLimit *-1))
        {
            rb.AddForce(new Vector2(moveHorizontal, 0) * accel);
        }

        if (moveHorizontal > 0)
        {
            faceRight = true;
            faceLeft = false;
        }

        if (moveHorizontal < 0)
        {
            faceLeft = true;
            faceRight = false;
        }

    }

    private void Update()
    {   
        
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
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            airBorn = true;
            Debug.Log("airborn");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        //if (collision.gameObject.CompareTag("torch"))
        //{
        //    if (itemHeld != "torch")
        //    {
        //        itemHeld = "torch";
        //        itemCount = 1;
        //    }
        //    itemCount++;
        //}

        //if (collision.gameObject.CompareTag("potion"))
        //{
        //    if (itemHeld != "potion")
        //    {
        //        itemHeld = "potion";
        //        itemCount = 1;
        //    }
        //    itemCount++;
        //}

        //if (collision.gameObject.CompareTag("chalk"))
        //{
        //    if (itemHeld != "chalk")
        //    {
        //        itemHeld = "chalk";
        //        itemCount = 1;
        //    }
        //    itemCount++;
        //}
    }

    private void Attack() //instantiates the prefab sword attack
    {
        if (faceRight) //checks to see which way the character is facing and offsets the hitbox in game space accordingly
        {
            Instantiate(swordSwing, (gameObject.transform.position + new Vector3(1, 0, 0)), Quaternion.identity);
        }

        if (faceLeft)
        {
            Instantiate(swordSwing, (gameObject.transform.position + new Vector3(-1, 0, 0)), Quaternion.identity);
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
        SceneManager.LoadScene("levelOne");
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
                
                break;

            case "chalk":

                itemCount--;
                
                break;
        }
    }
}

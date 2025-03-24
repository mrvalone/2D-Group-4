using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    public UnityEngine.UI.Image heart1;
    public UnityEngine.UI.Image heart2;
    public UnityEngine.UI.Image heart3;
    public Sprite fullHeart;
    public Sprite heart3_4;
    public Sprite heart1_2;
    public Sprite heart1_4;
    public float speed; //max speed for left-right movement
    public float jumpHeight; //height of jump
    public GameObject swordSwingR;
    public GameObject swordSwingL;
    public GameObject chalkMark;
    public AudioSource AudioSource;
    public AudioClip jumpSFX;
    public AudioClip swingSFX;
    public AudioClip potionSFX;
    public AudioClip drinkSFX;
    public AudioClip damageSFX;
    public AudioClip crystalSFX;
    public AudioClip splashSFX;
    public AudioClip landSFX;

    private string itemHeld;  
    private int itemCount;
    private int hp;
    private SpriteRenderer rend;
    private Rigidbody2D rb;
    private Animator anim;
    private bool airBorn;
    private bool faceRight;
    private bool faceLeft;
    private bool isInvuln;
    private IEnumerator invuln;
    void Start()
    {
        hp = 12;
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
        isInvuln = false;

        string currentLevel = SceneManager.GetActiveScene().name;

        if (currentLevel == "LevelOne")
        {
            AudioManager.instance.StopMusic("LevelOne");
        }
        else if (currentLevel == "LevelTwo")
        {
            AudioManager.instance.StopMusic("LevelTwo");
        }

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
            AudioSource.clip = jumpSFX;
            AudioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Attack();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            UseItem();
            AudioSource.PlayOneShot(drinkSFX);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            Instantiate(chalkMark, (gameObject.transform.position + new Vector3(0, -1.5f, -1.0f)), Quaternion.identity);
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
            AudioSource.PlayOneShot(crystalSFX);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("crystal"))
        {
            StartCoroutine(EndGame(1.0f));
        }

        if (collision.gameObject.CompareTag("levelEnd")) // When the player hits the end of level block
        {
            SceneManager.LoadScene("LevelTwo");
        }

        if (collision.gameObject.CompareTag("ground"))
        {
            airBorn = false;
            Debug.Log("touching grass");
            AudioSource.clip = landSFX;
            AudioSource.Play();
        }

        if (collision.gameObject.CompareTag("bat") && !isInvuln)
        {
            ChangeHp(-1, collision.gameObject);
            Debug.Log("bat hit - 1 damage");
        }

        if (collision.gameObject.CompareTag("lawrence") && !isInvuln)
        {
            ChangeHp(-2, collision.gameObject);
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

            AudioSource.PlayOneShot(potionSFX);

            if (itemHeld != "potion")
            {
                itemHeld = "potion";
                itemCount = 0;
                Debug.Log("Potion Acquired");
            }        
            itemCount++;
            Debug.Log("Potion Count: " + itemCount);
        }

        if (collision.gameObject.CompareTag("acid") && !isInvuln)
        {
            ChangeHp(-2, collision.gameObject);
            AudioSource.clip = splashSFX;
            AudioSource.Play();
            Debug.Log("Acid - 2 damage");
        }

        if (collision.gameObject.CompareTag("water"))
        {
            AudioSource.clip = splashSFX;
            AudioSource.Play();
        }
    }

    private void Attack() //instantiates the prefab sword attack
    {
        if (faceRight) //checks to see which way the character is facing and offsets the hitbox in game space accordingly
        {
            Instantiate(swordSwingR, (gameObject.transform.position + new Vector3(0.9f, 0.4f, 0.5f)), Quaternion.Euler(new Vector3(0,0,65.0f))); //instantiates relative to the character's position based on the direction he's facing
        }

        if (faceLeft)
        {
            Instantiate(swordSwingL, (gameObject.transform.position + new Vector3(-0.9f, 0.4f, 0.5f)), Quaternion.Euler(new Vector3(0, 0, -65.0f)));
        }
        AudioSource.PlayOneShot(swingSFX);
    }

    private void ChangeHp(int change, GameObject source) //changes HP by the amount passed as parameter. Pass negative values to reduce HP
    {
        hp += change;
        AudioSource.PlayOneShot(damageSFX);

        if(hp <= 0)
        {
            GameOver();
        }
        else if (change < 0) //knockback and invuln frames
        {
            if (!source.CompareTag("acid")) //checks to see if the player took damage from acid
            {
                if (source.transform.position.x >= gameObject.transform.position.x) //knockback left
                {
                    rb.linearVelocityX = -15.0f;
                    rb.linearVelocityY = 10.0f;
                }
                else //knockback right
                {
                    rb.linearVelocityX = 15.0f;
                    rb.linearVelocityY = 10.0f;
                }
            }
            else //if acid, knocks directly up
            {
                rb.linearVelocityY = 15.0f;
            }

            isInvuln = true;
            StartCoroutine(invulnFrames(1.0f));
        }

        switch (hp) //switches out health heart images based on numerical HP value
        {
            case 12:
                heart2.enabled = true;   
                heart3.enabled = true;

                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                break;
            case 11:
                heart2.enabled = true;
                heart3.enabled = true;

                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = heart3_4;
                break;
            case 10:
                heart2.enabled = true;
                heart3.enabled = true;

                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = heart1_2;
                break;
            case 9:
                heart2.enabled = true;
                heart3.enabled = true;

                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = heart1_4;
                break;

            case 8:
                heart2.enabled = true;
                heart3.enabled = false;

                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                break;

            case 7:
                heart2.enabled = true;
                heart3.enabled = false;

                heart1.sprite = fullHeart;
                heart2.sprite = heart3_4;
                break;

            case 6:
                heart2.enabled = true;
                heart3.enabled = false;

                heart1.sprite = fullHeart;
                heart2.sprite = heart1_2;
                break;

            case 5:
                heart2.enabled = true;
                heart3.enabled = false;

                heart1.sprite = fullHeart;
                heart2.sprite = heart1_4;
                break;

            case 4:
                heart2.enabled = false;
                heart3.enabled = false;

                heart1.sprite = fullHeart;
                break;

            case 3:
                heart2.enabled = false;
                heart3.enabled = false;

                heart1.sprite = heart3_4;
                break;

            case 2:
                heart2.enabled = false;
                heart3.enabled = false;

                heart1.sprite = heart1_2;
                break;

            case 1:
                heart2.enabled = false;
                heart3.enabled = false;

                heart1.sprite = heart1_4;
                break;
            
            default:
                heart1.enabled = false;
                heart2.enabled = false;
                heart3.enabled = false;
                break;
        }
    
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator invulnFrames(float wait)
    {
        yield return new WaitForSeconds(wait);
        isInvuln = false;
    }

    IEnumerator EndGame(float wait)
    {
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene("EndScene");
    }

    private void UseItem()
    {
        switch (itemHeld)
        {
            case "torch":

                itemCount--;
                
                break;

            case "potion":

                ChangeHp(3, gameObject);
                itemCount--;
                Debug.Log("Potion Used. Remaining: " +  itemCount);
                break;

            case "chalk":

                itemCount--;
                
                break;
        }
    }
}

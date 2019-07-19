using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

private Animator anim;
public GameObject camera;
private Rigidbody2D rb2d;

private Vector2 charScale;
private float charScaleX;
public float speed;

public AudioClip pickupSound;
public AudioSource pickupSource;
public AudioClip enemySound;
public AudioSource enemySource;
public AudioClip bgMusic;
public AudioSource bgSource;
public AudioSource winSource;
public AudioClip winMusic;

public Text scoreText;
public Text livesText;
public Text winText;
public Text overText;

private int score;
private int lives;
public float jumpForce;

private bool facingRight = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        pickupSource.clip = pickupSound;
        enemySource.clip = enemySound;
        bgSource.clip = bgMusic;
        winSource.clip = winMusic;
        bgSource.Play();
        rb2d = GetComponent<Rigidbody2D>();

        score = 0;
        SetScoreText();
        winText.text = "";
        overText.text = "";

        lives = 3;
        SetLivesText();
    }

    void Update()
    {
        if (lives <= 1)
        {
            livesText.color = new Color(168f/255f, 23f/255f, 23f/255f);
        }
        if (lives == 0)
        { 
            Destroy(this);
            overText.text = "Game Over";
        }

        if (Input.GetKeyDown("right")) {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp("right")) {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown("left"))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp("left")) {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            anim.SetBool("Jump Bool", true);
        }

    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);

        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
            else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }

    }

    void OnTriggerEnter2D(Collider2D other) 
        {
        if (other.gameObject.CompareTag("Pickup")) 
            {
                other.gameObject.SetActive (false);
                pickupSource.Play();
                score = score + 1;
                SetScoreText();
            }

        if (score == 4)
            {
                transform.position = new Vector3(16f, -2.2f, 0f);
                camera.transform.position = new Vector3(23.75f, 0f, -10f);
                livesText.color = Color.white;
                lives = 3;
                SetLivesText();
            }

        else if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.SetActive(false);
                enemySource.Play();
                lives = lives - 1;
                SetLivesText ();
            }

        }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            anim.SetBool("Jump Bool", false);
        }
    }


    void SetScoreText ()
    {
        scoreText.text = "Score: " + score.ToString ();
            if (score >= 8) {
            bgSource.Stop();
            winSource.Play();
            winText.text = "You Win!";
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            GameObject.Destroy(enemy);
            }
    }
    void SetLivesText ()
    {
        livesText.text = "Lives: " + lives.ToString ();
    }
    void Flip ()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}

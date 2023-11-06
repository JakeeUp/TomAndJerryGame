using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    //Start Varibles
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;


    public int numOfhearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyheart;

    //FSM
    private enum State { idle,running,jumping,falling,hurt}
    private State state = State.idle;

    //inspector varibles
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int gems = 0;
    [SerializeField] private TextMeshProUGUI GemText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource gem;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private int Health;
    [SerializeField] private AudioSource Jumping;
    [SerializeField] private AudioSource Hurt;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        
        if (state != State.hurt)
        {
            Movement();
        }
       
        Movement();
        AnimationState();
        anim.SetInteger("state", (int)state);
       // StartCoroutine(hurtAnimMethod());
        Hearts();
        if (gems <= 0)
        {
            gems = 0;
            GemText.text = gems.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ExitedGame");
            Application.Quit();
        }
    }

    private void Hearts()
    {
        if (Health > numOfhearts)
        {
            Health = numOfhearts;
            numOfhearts = Health;
        }
        if(gems == 50)
        {
            Health = 5;
            GemText.text = gems.ToString();


        }
       
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Health)
            {
                hearts[i].sprite = fullHeart;

            }
            else
            {
                hearts[i].sprite = emptyheart;
            }

            if (i < numOfhearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    //IEnumerator hurtAnimMethod()
    //{
           
    //    //if (state == State.hurt)
    //    //{
    //    //    Hurt.Play();
    //    //    gameObject.GetComponent<PlayerController>().enabled = false;
    //    //    state = State.idle;
    //    //    yield return new WaitForSeconds(.1f);
           
    //    //    gameObject.GetComponent<PlayerController>().enabled = false;
    //    //    state = State.idle;
    //    //    gameObject.GetComponent<PlayerController>().enabled = true;
    //    //}
    //    //else
    //    //{

    //    //    speed = 7;

    //    //}
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Pickup")
        {
            gem.Play();
            Destroy(collision.gameObject);
            gems += 1;
            GemText.text = gems.ToString();
        }
      

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            EnemyHolder enemyHolder = other.gameObject.GetComponent<EnemyHolder>();
            if(state == State.falling)
            {
                enemyHolder.JumpedOn();
                Jump();
                gems += 1;
                GemText.text = gems.ToString();

            }
            else
            {
                gameObject.GetComponent<PlayerController>().enabled = false;
                state = State.hurt;
                Hurt.Play();
                speed = 0;
                gems -= 5;
                GemText.text = gems.ToString();

                HandleHealth(); //deals with the health and ui
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy to right therefore should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    gameObject.GetComponent<PlayerController>().enabled = true;


                }
                else
                {
                    //enemy to left therefore should be damagae and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    gameObject.GetComponent<PlayerController>().enabled = true;
                }
            }

        }
        else
        {
            gameObject.GetComponent<PlayerController>().enabled = true;
            speed = 7;
        }
    }

   
   
    private void HandleHealth()
    {
        Health -= 1;
        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {

        }

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }
    private void Jump()
    {
        Jumping.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {


        if(state == State.jumping)
        {
            //
            if(rb.velocity.y<.1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
                speed = 7;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
                speed = 7;
            }
        }
        else if(Mathf.Abs(rb.velocity.x)>2f)
        {
            //
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }
}

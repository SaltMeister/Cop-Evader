using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    GameObject conesLeft;
    AudioSource crash;
    AudioSource honk;
    AudioSource drive;
    public Text text;

    float exitLevel;
    float delay;

    int counter;
    int coneCounter;
    int playClip;

    bool isHit;
    bool triggerEnter;
    bool isDead;

    public GameObject cone;

    Animator fadeOut;
    Animator anim;
    Rigidbody2D rb2d;

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    private Vector3 velocity = Vector3.zero;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        conesLeft = GameObject.Find("Canvas");
        AudioSource[] audios = GetComponents<AudioSource>();
        honk = audios[0];
        crash = audios[1];
        drive = audios[2];

        exitLevel = 4;
        coneCounter = 10;
        delay = 5;
        playClip = 1;
        counter = 0;

        rb2d = GetComponent<Rigidbody2D>();
        text.text = "Cones left: " + coneCounter;

        isDead = false;
        isHit = false;
        triggerEnter = false;

        fadeOut = GameObject.Find("Image").GetComponent<Animator>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit == false || (isHit == true && counter < 8) )
        {
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) ||
             Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                drive.Stop();
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
             Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
            {
                Move();
            }
          
        }
        else
        {
            Stop();
            isDead = true;
            //loads the game over scene after 5 seconds afer collision
            delay -= Time.deltaTime;
            if (delay <= 0) 
                SceneManager.LoadScene("GameOver");
            if(playClip == 1) 
            {
                crash.Play();
                playClip++;
            }


        }

        if (Input.GetKeyDown(KeyCode.Space) && isDead == false)
        {
            honk.Play();
            if (coneCounter > 0) 
            {
                Instantiate(cone, new Vector3(transform.position.x - 2, transform.position.y, 0), Quaternion.identity);
                coneCounter--;
                text.text = "Cones left: " + coneCounter;
            }
            //change the canvas text to lower with each press of the cone spawn
        }
         
    }


    //detects collision with the end of the map to change scene
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("should change scenes soon");
        StartCoroutine(waitToChangeScene());
    }

    //detects collisions for the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.name != "Cone")
        {
            isHit = true;
            counter++;
        }

    }

    void SceneChange()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        else if (SceneManager.GetActiveScene().name == "Level2")
            SceneManager.LoadScene("Level3", LoadSceneMode.Single);
        else
            SceneManager.LoadScene("YouWin", LoadSceneMode.Single);
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (drive.isPlaying == false) 
            drive.Play();

        Vector3 targetVelocity = new Vector2(moveHorizontal * 9.75f, moveVertical * 2.5f);
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
    }
    void Stop()
    {
        Vector3 targetVelocity = new Vector2(0 * 9f, 0 * 3f);

        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        anim.SetBool("gotHit", true);

    }

    IEnumerator waitToChangeScene()
    {
        fadeOut.SetBool("Exit", true);
        yield return new WaitForSeconds(1.5f);
        SceneChange();
    }
}

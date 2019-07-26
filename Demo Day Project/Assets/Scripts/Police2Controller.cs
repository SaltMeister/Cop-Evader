using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police2Controller : MonoBehaviour
{
    GameObject player;

    AudioSource siren;
    AudioSource crash;
    Animator anim;

    bool playerPassed;
    bool isHit;
    bool debounce;

    int counter;
    int playClip;
    public float speed;
    private float distance;
    private Transform target;

    Rigidbody2D rb2d;
    private Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement

    // Start is called before the first frame update
    void Start()
    {
        playClip = 1;
        counter = 0;
        AudioSource[] audios = GetComponents<AudioSource>();
        siren = audios[0];
        crash = audios[1];

        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();

        isHit = false;
        playerPassed = false;
        anim = GetComponent<Animator>();
        debounce = false;

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit == false)
        {
            if (player.transform.position.x > gameObject.transform.position.x + 5)
            {
                playerPassed = true;
            }
            Move();
        }
        else if (isHit == true)
        {
            Stop();

            if (playClip == 1)
            {
                crash.Play();
                playClip++;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.name == "Car1" || collision.transform.gameObject.name == "Car2" || collision.transform.gameObject.name == "Garbage")
        {

            counter++;
            if (counter == 20)
            {
                rb2d.freezeRotation = false;
                isHit = true;
            }
           
        }
        if (collision.transform.gameObject.name == "Player") 
        {
            isHit = true;
            rb2d.freezeRotation = false;
        }

    }

    void Move() 
    {
        if (playerPassed == false)
        {
            Vector3 targetVelocity = new Vector2(2.75f, 0f);
            rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        }
        else 
        {
            if(debounce == false) 
            {
                siren.Play();
                debounce = true;
            }

            transform.position = Vector2.MoveTowards(transform.position,
            target.position, speed * Time.deltaTime);
            distance = Vector3.Distance(target.position, transform.position);

        }

    }
    void Stop() 
    {
        Vector3 targetVelocity = new Vector2(0f, 0f);

        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        anim.SetBool("gotHit", true);
        siren.Stop();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    AudioSource crash;

    Animator anim;
    int playClip;
    bool isHit;
    Rigidbody2D rb2d;
    private Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    // Start is called before the first frame update
    void Start()
    {
        crash = GetComponent<AudioSource>();
        playClip = 1;
        rb2d = GetComponent<Rigidbody2D>();
        isHit = false;
        rb2d.freezeRotation = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   if (isHit == false) 
        {
            Move();
        }
        else 
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
        if (collision.transform.gameObject.name == "Player" || collision.transform.gameObject.name == "Police") 
        {
            isHit = true;
            rb2d.freezeRotation = false;
        }

    }
    void Move()
    {
        Vector3 targetVelocity = new Vector2(2.4f, 0f);
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
    }
    void Stop()
    {
        Vector3 targetVelocity = new Vector2(0f, 0f);
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        anim.SetBool("gotHit", true);
    }
}

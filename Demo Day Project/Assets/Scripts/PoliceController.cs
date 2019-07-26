using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceController : MonoBehaviour
{
    AudioSource siren;
    AudioSource crash;
    Animator anim;

    int playClip;
    bool isHit;
    public float speed;
    private float distance;
    private Transform target;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        playClip = 1;
        AudioSource[] audios = GetComponents<AudioSource>();
        siren = audios[0];
        crash = audios[1];

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        isHit = false;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit == false) 
        {
            transform.position = Vector2.MoveTowards(transform.position,
            target.position, speed * Time.deltaTime);
            distance = Vector3.Distance(target.position, transform.position);
        }
        else 
        {
            // nothing happens
            anim.SetBool("gotHit", true);
            rb2d.freezeRotation = false;
            siren.Stop();
            if (playClip == 1)
            {
                crash.Play();
                playClip++;
            }

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.name == "Player") 
            isHit = true;

    }

}

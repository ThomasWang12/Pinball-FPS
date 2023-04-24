using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    Game game;
    Animator animator;

    public enum flipper { Left, Right };
    KeyCode keyFlipLeft = KeyCode.A;
    KeyCode keyFlipRight = KeyCode.D;
    string animFlipLeft = "Left Flipper";
    string animFlipRight = "Right Flipper";

    [SerializeField] flipper side;
    KeyCode keyFlip;
    string animFlip;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
        animator = transform.parent.GetComponent<Animator>();
    }

    void Start()
    {
        if (transform.parent.name.Contains("Left"))
        {
            side = flipper.Left;
            keyFlip = keyFlipLeft;
            animFlip = animFlipLeft;
        }
        if (transform.parent.name.Contains("Right"))
        {
            side = flipper.Right;
            keyFlip = keyFlipRight;
            animFlip = animFlipRight;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(keyFlip))
        {
            animator.Play(animFlip, 0, 0.0f);
            game.audio.Play(Sound.name.Flipper);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            game.audio.Play(Sound.name.FlipperHit);
            game.player.GetComponent<Rigidbody>().AddForce(Vector3.up * 5);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Flipper : MonoBehaviour
{
    Game game;
    Animator animator;
    Rigidbody rb;

    public enum flipper { Left, Right };
    KeyCode keyFlipLeft = KeyCode.A;
    KeyCode keyFlipRight = KeyCode.D;
    Vector3 targetRotLeft = new Vector3(-70, 0, -60);
    Vector3 targetRotRight = new Vector3(-70, 0, 60);

    [SerializeField] flipper side;
    KeyCode keyFlip;
    string animFlip;
    Quaternion initialRot;
    Vector3 targetRot;
    float rotProgress;

    /* Tunables */
    //float rotDuration = 1.0f;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        initialRot = transform.rotation;

        if (side == flipper.Left)
        {
            side = flipper.Left;
            keyFlip = keyFlipLeft;
            targetRot = targetRotLeft;
        }
        if (side == flipper.Right)
        {
            side = flipper.Right;
            keyFlip = keyFlipRight;
            targetRot = targetRotRight;
        }
    }

    void Update()
    {
        // ...
    }

    void FixedUpdate()
    {
        float t = Mathf.Clamp01(rotProgress);
        //Easing.Type.SineEaseOut(Mathf.Clamp01(rotProgress), 0, 1, rotDuration);
        Quaternion rot = Quaternion.Lerp(initialRot, Quaternion.Euler(targetRot), t);
        rb.MoveRotation(rot);

        if (Input.GetKey(keyFlip)) rotProgress += 5.0f * Time.fixedDeltaTime;
        else rotProgress -= 5.0f * Time.fixedDeltaTime;
        rotProgress = Mathf.Clamp01(rotProgress);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            game.sound.Play(Sound.name.FlipperHit);
        }
    }
}
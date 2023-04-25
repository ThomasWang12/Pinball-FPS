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
    //string animFlipLeft = "Left Flipper";
    //string animFlipRight = "Right Flipper";
    Vector3 targetRotLeft = new Vector3(0, 0, -60);
    Vector3 targetRotRight = new Vector3(0, 0, 60);

    [SerializeField] flipper side;
    KeyCode keyFlip;
    string animFlip;
    Quaternion initialRot;
    Vector3 targetRot;
    float rotProgress;

    /* Tunables */
    float rotDuration = 1.0f;

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
            //animFlip = animFlipLeft;
            targetRot = targetRotLeft;
        }
        if (side == flipper.Right)
        {
            side = flipper.Right;
            keyFlip = keyFlipRight;
            //animFlip = animFlipRight;
            targetRot = targetRotRight;
        }
    }

    void Update()
    {
        /*if (Input.GetKeyDown(keyFlip))
        {
            //animator.enabled = true;
            animator.Play(animFlip, 0, 0.0f);
        }*/
    }

    void FixedUpdate()
    {
        /*if (Input.GetKey(keyFlip))
        {
            //rb.MoveRotation(Quaternion.Euler(targetRot));
            rb.MoveRotation(rb.rotation.Diff(Quaternion.Euler(targetRot)));
            //Debug.Log(rb.rotation.eulerAngles);

            //Vector3 m_EulerAngleVelocity = new Vector3(0, 100, 0);
            //Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
            //rb.MoveRotation(rb.rotation * deltaRotation);

            game.sound.Play(Sound.name.Flipper);
        }
        else
        {
            rb.MoveRotation(rb.rotation.Diff(initialRot));
        }*/

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
            //animator.Play(animFlip + " Initial", 0, 0.0f);
            //animator.enabled = false;
            game.sound.Play(Sound.name.FlipperHit);
            //game.player.GetComponent<Rigidbody>().AddForce(Vector3.up * 5);
        }
    }
}
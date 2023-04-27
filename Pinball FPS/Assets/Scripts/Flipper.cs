using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Flipper : MonoBehaviour
{
    Game game;
    Rigidbody rb;

    public enum flipper { Left, Right };
    KeyCode keyFlipLeft = KeyCode.A;
    KeyCode keyFlipRight = KeyCode.D;
    Vector3 targetRotLeft = new Vector3(0, 0, -60);
    Vector3 targetRotRight = new Vector3(0, 0, 60);

    [SerializeField] flipper side;
    KeyCode keyFlip;
    Quaternion initialRot;
    Vector3 targetRot;
    float rotProgress;

    /* Tunables */
    float rotDuration = 0.1f;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        initialRot = transform.rotation;

        if (side == flipper.Left)
        {
            side = flipper.Left;
            keyFlip = keyFlipLeft;
            targetRot = transform.parent.rotation.eulerAngles + targetRotLeft;
        }
        if (side == flipper.Right)
        {
            side = flipper.Right;
            keyFlip = keyFlipRight;
            targetRot = transform.parent.rotation.eulerAngles + targetRotRight;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(keyFlip)) game.sound.Play(Sound.name.FlipperEnter);
        if (Input.GetKeyUp(keyFlip)) game.sound.Play(Sound.name.FlipperExit);
    }

    void FixedUpdate()
    {
        if (Input.GetKey(keyFlip))
            rotProgress += (1 / rotDuration) * Time.fixedDeltaTime;
        else rotProgress -= (1 / rotDuration) * Time.fixedDeltaTime;
        rotProgress = Mathf.Clamp01(rotProgress);

        Quaternion rot = Quaternion.Lerp(initialRot, Quaternion.Euler(targetRot), rotProgress);
        rb.MoveRotation(rot);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            game.AmmoRefill();
            game.sound.Play(Sound.name.FlipperHit);
        }
    }
}
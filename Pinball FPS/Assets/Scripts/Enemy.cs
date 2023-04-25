using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Game game;
    Rigidbody rb;
    Vector3 initialPos;
    Quaternion initialRot;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    void Update()
    {
        rb.isKinematic = !game.started;
    }

    public void Reset()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject player;
    [HideInInspector] public Audio audio;

    Vector3 startPos;
    Quaternion startRot;
    public bool started = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        audio = GameObject.Find("Sounds").GetComponent<Audio>();
    }

    void Start()
    {
        startPos = player.transform.position;
        startRot = player.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            started = false;
            player.transform.position = startPos;
            player.transform.rotation = startRot;
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

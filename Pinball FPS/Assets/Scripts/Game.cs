using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject player;
    public Audio audio;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        audio = GameObject.Find("Sounds").GetComponent<Audio>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            player.GetComponent<Rigidbody>().isKinematic = false;
    }
}

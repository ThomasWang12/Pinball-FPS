using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject player;
    [SerializeField] GameObject lookTarget;
    [HideInInspector] public Audio sound;

    Vector3 startPos;
    Quaternion startRot;
    public bool started = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        sound = GameObject.Find("Sounds").GetComponent<Audio>();
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

        //Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, 0, Camera.main.transform.rotation.eulerAngles.z);
        //Camera.main.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);


        if (Input.GetMouseButton(0))
        {
            Time.timeScale = 0.2f;
        }
        else Time.timeScale = 1;

        if (Input.GetMouseButton(1))
        {
            Time.timeScale = 0.2f;
        }
        else Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.R))
        {
            started = false;
            player.transform.position = startPos;
            player.transform.rotation = startRot;
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

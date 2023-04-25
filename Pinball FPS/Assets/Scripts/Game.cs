using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [HideInInspector] public UI ui;
    [HideInInspector] public Audio sound;

    public GameObject player;
    PlayerCamera playerCam;
    [SerializeField] GameObject lookTarget;

    Vector3 startPos;
    Quaternion startRot;

    [Space(10)]

    public bool started = false;
    public bool slowMo = false;
    public int health = 10;

    /* Tunables */
    float slowMoTimeRate = 0.2f;

    void Awake()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
        sound = GameObject.Find("Sounds").GetComponent<Audio>();
        player = GameObject.FindWithTag("Player");
        playerCam = Camera.main.GetComponent<PlayerCamera>();
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
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            sound.Play(Sound.name.Shoot);
        }

        // Slow Mo: ON
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            slowMo = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            Time.timeScale = slowMoTimeRate;
            ui.SlowMoEffect(true);
            sound.Play(Sound.name.SlowMoEnter);
        }
        // Slow Mo: OFF
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            slowMo = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            Time.timeScale = 1;
            ui.SlowMoEffect(false);
            sound.Play(Sound.name.SlowMoExit);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [HideInInspector] public UI ui;
    [HideInInspector] public Audio sound;

    public GameObject player;
    Rigidbody playerRb;
    PlayerCamera playerCam;
    [SerializeField] GameObject lookTarget;

    Vector3 startPos;
    Quaternion startRot;

    [Range(0, 10)]
    public float shootRecoilForce = 3f;

    [Space(10)]

    public bool started = false;
    public bool slowMotion = false;
    public int health = 10;
    public int ammo = 6;

    /* Tunables */
    float slowMoTimeRate = 0.2f;
    int ammoMax = 6;
    int ammoRefill = 6;

    void Awake()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
        sound = GameObject.Find("Sounds").GetComponent<Audio>();
        player = GameObject.FindWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();
        playerCam = Camera.main.GetComponent<PlayerCamera>();
    }

    void Start()
    {
        ammo = ammoMax;
        startPos = player.transform.position;
        startRot = player.transform.rotation;
    }

    void Update()
    {
        // Start game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (!started) return;

        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0)
            {
                ammo--;
                playerCam.Shoot();
                playerRb.AddForce(-Camera.main.transform.forward * 5f, ForceMode.Impulse);
                sound.Play(Sound.name.Shoot);
            }
            else sound.Play(Sound.name.NoAmmo);
        }

        // Slow Motion: ON
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            slowMotion = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            Time.timeScale = slowMoTimeRate;
            ui.SlowMoEffect(true);
            sound.Play(Sound.name.SlowMoEnter);
        }
        // Slow Motion: OFF
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            slowMotion = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            Time.timeScale = 1;
            ui.SlowMoEffect(false);
            sound.Play(Sound.name.SlowMoExit);
        }

        // Restart game
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AmmoRefill()
    {
        if (ammo + ammoRefill <= ammoMax)
            ammo += ammoRefill;
        else ammo = ammoMax;
    }

    public void DeductHealth()
    {
        if (health > 0) health--;
        else health = 0;
    }

    public void PlayerRespawn()
    {
        started = false;
        player.transform.position = startPos;
        player.transform.rotation = startRot;
        player.GetComponent<Rigidbody>().isKinematic = true;
        ammo = ammoMax;
    }
}

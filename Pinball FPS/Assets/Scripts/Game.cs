using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [HideInInspector] public UI ui;
    [HideInInspector] public Audio sound;
    WaveController WC;

    public GameObject player;
    Rigidbody playerRb;
    PlayerCamera playerCam;

    Vector3 startPos;
    Quaternion startRot;

    [Range(0, 10)]
    public float shootRecoilForce = 3f;

    [Space(10)]

    public bool started = false;
    public bool gameOver = false;
    public bool slowMotion = false;
    [Range(0, 100)]
    public float slowMoBar = 100;
    public int health = 10;
    public int ammo = 6;

    /* Tunables */
    [HideInInspector] public float slowMoMax = 100;
    float slowMoUseRate = 0.5f;
    float slowMoRefillRate = 1f;
    float slowMoTimeRate = 0.2f;
    int ammoMax = 6;
    int ammoRefill = 6;

    void Awake()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
        sound = GameObject.Find("Sounds").GetComponent<Audio>();
        WC = GameObject.Find("WaveController").GetComponent<WaveController>();
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
            if (WC.CurWave == null) WC.NextWave();
            ui.GameStart();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerRb.isKinematic = false;
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

        // Slow motion
        if (Input.GetKeyDown(KeyCode.LeftShift) && slowMoBar > 0) SlowMotion(true);
        if (Input.GetKeyUp(KeyCode.LeftShift)) SlowMotion(false);
        if (slowMotion) slowMoBar -= slowMoUseRate;
        else slowMoBar += slowMoRefillRate;
        slowMoBar = Mathf.Clamp(slowMoBar, 0, slowMoMax);
        if (slowMoBar <= 0) SlowMotion(false);

        // Restart game
        if (Input.GetKeyDown(KeyCode.R))
            Restart();

        // Level complete
        if (WC.LevelCompleted)
        {
            started = false;
            playerRb.isKinematic = true;
            ui.LevelComplete();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        // Game over
        if (health <= 0)
        {
            gameOver = true;
            started = false;
            playerRb.isKinematic = true;
            ui.GameOver();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    void SlowMotion(bool toggle)
    {
        // Slow Motion: ON
        if (toggle)
        {
            slowMotion = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = slowMoTimeRate;
            ui.SlowMoEffect(true);
            sound.Play(Sound.name.SlowMoEnter);
        }
        // Slow Motion: OFF
        else
        {
            slowMotion = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            ui.SlowMoEffect(false);
            sound.Play(Sound.name.SlowMoExit);
        }
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
        SlowMotion(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

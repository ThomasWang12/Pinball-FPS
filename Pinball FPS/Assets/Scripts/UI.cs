using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class UI : MonoBehaviour
{
    Game game;
    WaveController WC;

    [SerializeField] TMP_Text pressStartTMP;
    public RawImage crosshair;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ammoTMP;
    [SerializeField] RawImage slowMoBar;
    [SerializeField] TMP_Text waveTMP;
    [SerializeField] CanvasGroup levelComplete;
    [SerializeField] CanvasGroup gameOver;

    [SerializeField] PostProcessVolume postProcessVolume;
    Vignette vignette;
    ChromaticAberration chromaticAberration;
    ColorGrading colorGrading;

    Vector3 originMousePos;
    bool recordMousePos = false;
    float slowMoBarMaxHeight;

    /* Tunables */
    int padding = 100;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
        WC = GameObject.Find("WaveController").GetComponent<WaveController>();
    }

    void Start()
    {
        slowMoBarMaxHeight = slowMoBar.rectTransform.sizeDelta.y;
        CanvasGroupToggle(levelComplete, false);
        CanvasGroupToggle(gameOver, false);
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        SlowMoEffect(false);
    }

    void Update()
    {
        // Before game start
        if (!game.started)
        {
            if (!game.gameOver)
            {
                if (!WC.LevelCompleted)
                {
                    waveTMP.text = "Wave " + WC.currentWave + " / " + WC.totalWave;
                    waveTMP.enabled = true;
                    pressStartTMP.enabled = true;
                }
            }
        }

        // Slow motion
        if (game.slowMotion)
        {
            if (!recordMousePos)
            {
                originMousePos = Input.mousePosition;
                recordMousePos = true;
            }
            else
            {
                float crosshairX = Mathf.Clamp(Screen.width / 2 - (originMousePos.x - Input.mousePosition.x), padding, Screen.width - padding);
                float crosshairY = Mathf.Clamp(Screen.height / 2 - (originMousePos.y - Input.mousePosition.y), padding, Screen.height - padding);
                crosshair.transform.position = new Vector2(crosshairX, crosshairY);
            }
        }
        else
        {
            recordMousePos = false;
            crosshair.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        // Health
        healthTMP.text = "Health: " + game.health;

        // Ammo
        if (game.ammo > 0)
            ammoTMP.text = game.ammo + "<size=60> / 6</size>";
        else ammoTMP.text = "<size=40>REFILL\nAMMO</size>\n<size=30><cspace=-0.05em>(Touch flippers)</cspace></size>";

        // Slow Mo Bar
        float h = game.slowMoBar / game.slowMoMax;
        slowMoBar.rectTransform.sizeDelta = new Vector2(slowMoBar.rectTransform.sizeDelta.x, slowMoBarMaxHeight * h);
    }

    public void GameStart()
    {
        waveTMP.enabled = false;
        pressStartTMP.enabled = false;
    }

    public void SlowMoEffect(bool state)
    {
        vignette.active = state;
        chromaticAberration.active = state;
    }

    public void WaveComplete(int wave)
    {
        StartCoroutine(WaveComplete(wave));
        IEnumerator WaveComplete(int wave)
        {
            waveTMP.text = "Wave " + wave + " / " + WC.totalWave + " Complete!";
            waveTMP.enabled = true;
            yield return new WaitForSeconds(3.0f);
            waveTMP.enabled = false;
            if (!WC.LevelCompleted) game.PlayerRespawn();
        }
    }

    private void CanvasGroupToggle(CanvasGroup user, bool state)
    {
        int a = (state) ? 1 : 0;
        user.alpha = a;
        user.interactable = state;
        user.blocksRaycasts = state;
    }

    public void LevelComplete()
    {
        waveTMP.text = "Level Complete!";
        waveTMP.enabled = true;
        crosshair.enabled = false;
        CanvasGroupToggle(levelComplete, true);
        colorGrading.active = true;
    }

    public void GameOver()
    {
        waveTMP.text = "Game Over!";
        waveTMP.enabled = true;
        crosshair.enabled = false;
        CanvasGroupToggle(gameOver, true);
        vignette.color.value = new Color(1, 0, 0);
        vignette.active = true;
        colorGrading.colorFilter.value = new Color(1, 0.7f, 0.7f);
        colorGrading.active = true;
    }
}

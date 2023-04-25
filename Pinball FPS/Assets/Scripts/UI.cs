using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class UI : MonoBehaviour
{
    Game game;

    [SerializeField] TMP_Text pressStartTMP;
    public RawImage crosshair;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ammoTMP;

    [SerializeField] PostProcessVolume postProcessVolume;
    Vignette vignette;
    ChromaticAberration chromaticAberration;

    Vector3 originMousePos;
    bool recordMousePos = false;

    /* Tunables */
    int padding = 100;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
    }

    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
    }

    void Update()
    {
        pressStartTMP.enabled = !game.started;

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

        healthTMP.text = "Health: " + game.health;

        if (game.ammo > 0)
            ammoTMP.text = game.ammo + "<size=60> / 6</size>";
        else ammoTMP.text = "<size=40>REFILL\nAMMO</size>";
    }

    public void SlowMoEffect(bool state)
    {
        vignette.active = state;
        chromaticAberration.active = state;
    }
}

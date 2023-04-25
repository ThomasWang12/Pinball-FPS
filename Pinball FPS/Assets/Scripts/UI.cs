using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class UI : MonoBehaviour
{
    Game game;

    [SerializeField] PostProcessVolume postProcessVolume;
    Vignette vignette;
    ChromaticAberration chromaticAberration;

    [SerializeField] TMP_Text healthTMP;
    [SerializeField] RawImage crosshair;

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
        Debug.Log(Input.mousePosition);

        if (game.slowMo)
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
    }

    public void SlowMoEffect(bool state)
    {
        vignette.active = state;
        chromaticAberration.active = state;
    }
}

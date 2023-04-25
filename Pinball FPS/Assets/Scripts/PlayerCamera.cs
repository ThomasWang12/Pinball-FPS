using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    UI ui;
    Camera cam;
    Vector3 look;

    [Range(0, 10)]
    public float speed = 3;
    public LayerMask raycastMask;

    void Awake()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
    }

    void Start()
    {
        cam = Camera.main;
        look = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        // Mouse look around
        look.y += Input.GetAxis("Mouse X");
        look.x += -Input.GetAxis("Mouse Y");
        look.z = transform.parent.rotation.z;
        transform.eulerAngles = look * speed;
    }

    public void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(ui.crosshair.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, raycastMask))
        {
            Destroy(hit.transform.gameObject);
        }
    }
}
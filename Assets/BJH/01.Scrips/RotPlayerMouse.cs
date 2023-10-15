using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPlayerMouse : MonoBehaviour
{
    public float speed;

    // 회전값 누적
    float rotX;
    float rotY;

    // camera
    public Transform cam;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx * speed * Time.deltaTime;
        rotY += my * speed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, rotX, 0);
        cam.transform.localEulerAngles = new Vector3(-rotY, 0, 0);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCamera : MonoBehaviour
{
    public Camera cam;

    [Range(1.0f, 10.0f)]
    public float speed = 5.0f;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + (cam.transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal")), Time.deltaTime * 10.0f); ;
        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0));
        cam.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
    }
}

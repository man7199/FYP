using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MinimapCameraMovement : MonoBehaviour
{
//2023-1-8
    [SerializeField] float cameraSpeed = 2;


    [SerializeField] float angleOfDepression = 30;
    [SerializeField] float angle=0;  // angle stores the camera angle, in radian

    
    // Start is called before the first frame update
    void Start()
    {
        // angle = (Mathf.PI / 180) * angleOfDepression;

        // transform.rotation = Quaternion.Euler(angleOfDepression, 0, 0);
        // transform.position = initialCameraPosition;
        // Debug.Log(transform.eulerAngles.x);
    }

    // Update is called once per frame
    void Update()
    {
        //move camera
        //move left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-cameraSpeed, 0, 0));
        } 

        //move right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(cameraSpeed, 0, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, cameraSpeed * Mathf.Cos(angle), cameraSpeed * Mathf.Sin(angle)));
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0,  -cameraSpeed * Mathf.Cos(angle) - cameraSpeed * Mathf.Sin(angle)));
        }
        //the transform.position.y is used to avoid the camera went out of the map
       


        // if (Input.GetKey(KeyCode.Alpha1))
        // {
        //     float distance = transform.position.y/Mathf.Sin(angle);
        //     Vector3 pos = transform.position + transform.forward * distance;
        //     transform.RotateAround(pos, Vector3.up, 60 * Time.deltaTime);
        // }
        // if (Input.GetKey(KeyCode.Alpha2))
        // {
        //     float distance = transform.position.y / Mathf.Sin(angle);
        //     Vector3 pos = transform.position + transform.forward * distance;
        //     transform.RotateAround(pos, Vector3.up, -60 * Time.deltaTime);
        // }
    }
}
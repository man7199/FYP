using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 2f;
    //[SerializeField] float angleOfDepression = 30f;
    [SerializeField] float angle;  // angle stores the camera angle, in radian

    [SerializeField] float rightLimit;  //the min. x the camera can reach
    [SerializeField] float leftLimit;   //the max. x the camera can reach
    [SerializeField] float upLimit;     //the max. z the camera can reach
    [SerializeField] float bottomLimit; //the min z the camera can reach
    [SerializeField] float height = -875f;

    private float[] position = new float[]  { -75, -55, -25, -15, +25, +75, +125 }; //for zooming
    private float[] speed = new float[]     { 0.6f, 1.2f, 1.8f, 2.2f, 3f, 5f, 6f };
    private int num = 5;

    public bool mouseControlMovement = true;

    public (Vector3,float) getCameraPositionAndAngle()
    {
        return (transform.position, angle);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(transform.position.x, height + position[num], transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //move camera

        //set movable by mouse
        if (Input.GetKeyDown(KeyCode.Tab)){
            mouseControlMovement = !mouseControlMovement;
        }

        //mouse movement
        if (mouseControlMovement)
        {
            if (Input.mousePosition.x <= Screen.width / 60)
            {
                if (transform.position.x >= leftLimit)
                {
                    transform.Translate(new Vector3(-cameraSpeed, 0, 0));
                }
            }
            if (Input.mousePosition.x >= 59 * Screen.width / 60)
            {
                if (transform.position.x <= rightLimit)
                {
                    transform.Translate(new Vector3(cameraSpeed, 0, 0));
                }
            }
            if (Input.mousePosition.y >= 59 * Screen.height / 60)
            {
                if (transform.position.z <= upLimit)
                {
                    transform.Translate(new Vector3(0, cameraSpeed * Mathf.Sin(angle), cameraSpeed * Mathf.Cos(angle)));
                }
            }
            if (Input.mousePosition.y <= Screen.height / 60)
            {
                if (transform.position.z >= bottomLimit)
                {
                    transform.Translate(new Vector3(0, -cameraSpeed * Mathf.Sin(angle), -cameraSpeed * Mathf.Cos(angle)));
                }
            }
        }

        //move left

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (transform.position.x >= leftLimit)
            {
                transform.Translate(new Vector3(-cameraSpeed, 0, 0));
            }
        }

        //move right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (transform.position.x <= rightLimit)
            {
                transform.Translate(new Vector3(cameraSpeed, 0, 0));
            }
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (transform.position.z <= upLimit)
            {
                transform.Translate(new Vector3(0, cameraSpeed * Mathf.Sin(angle), cameraSpeed * Mathf.Cos(angle)));
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (transform.position.z >= bottomLimit)
            {
                transform.Translate(new Vector3(0, -cameraSpeed * Mathf.Sin(angle), -cameraSpeed * Mathf.Cos(angle)));
            }
        }
        //the transform.position.y is used to avoid the camera went out of the map

        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) //move towards map
        {
            //                              x,y,z
            //      new Vector3(0, -3 * cameraSpeed, 0));
            if (transform.position.y <= height - 75f)
            {
                cameraSpeed = 0.6f;
                num = 0;
            }
            else if (transform.position.y <= height - 55f)
            {
                transform.Translate(new Vector3(0, 3 * -cameraSpeed, 0));
                cameraSpeed = 1.2f;
                num = 1;
            }
            else if (transform.position.y <= height - 25f)
            {
                transform.Translate(new Vector3(0, 2* -cameraSpeed, 0));
                cameraSpeed = 1.8f;
                num = 2;
            }
            else if (transform.position.y <= height - 15f)
            {
                transform.Translate(new Vector3(0, 1.5f* -cameraSpeed, 0));
                cameraSpeed = 2.2f;
                num = 3;
            }
            else if (transform.position.y <= height + 25f)
            {
                transform.Translate(new Vector3(0, 1.5f*-cameraSpeed, 0));
                cameraSpeed = 3;
                num = 4;
            }
            else if (transform.position.y <= height + 75f)
            {
                transform.Translate(new Vector3(0, -cameraSpeed, 0));
                cameraSpeed = 5;
                num = 5;
            }
            else
            {
                transform.Translate(new Vector3(0, -cameraSpeed, 0));
                cameraSpeed = 6;
                num = 6;
            }

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f) //move away from map
        {
            if (transform.position.y >= height + 125f)
            {
		transform.Translate(new Vector3(0, cameraSpeed, 0));
                cameraSpeed = 6;
                num = 6;
            }
            else if (transform.position.y >= height + 75f)
            {
                transform.Translate(new Vector3(0, cameraSpeed, 0));
                cameraSpeed = 5;
                num = 5;
            }
            else if (transform.position.y >= height + 25f)
            {
                transform.Translate(new Vector3(0, 1.5f * cameraSpeed, 0));
                cameraSpeed = 3;
                num = 4;
            }
            else if (transform.position.y >= height - 15f)
            {
                transform.Translate(new Vector3(0, 1.5f * cameraSpeed, 0));
                cameraSpeed = 2.2f;
                num = 3;
            }
            else if (transform.position.y >= height - 25f)
            {
                transform.Translate(new Vector3(0, 1.5f * cameraSpeed, 0));
                cameraSpeed = 1.8f;
                num = 2;
            }
            else if (transform.position.y >= height - 55f)
            {
                transform.Translate(new Vector3(0, 2 * cameraSpeed, 0));
                cameraSpeed = 1.2f;
                num = 1;
            }
            else
            {
                transform.Translate(new Vector3(0, 3 * cameraSpeed, 0));
                cameraSpeed = 0.6f;
                num = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Minus)) //zoom out
        {
            if (num < position.Length)
            {
                num += 1;
                this.transform.position = new Vector3(transform.position.x, height + position[num], transform.position.z);
                cameraSpeed = speed[num];
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals)) //zoom in
        {
            if (num > 0)
            {
                num -= 1;
                this.transform.position = new Vector3(transform.position.x, height + position[num], transform.position.z);
                cameraSpeed = speed[num];
            }
        }

        if (Input.GetKey(KeyCode.Alpha1)) //zoom in
        {
		cameraSpeed -=0.1f;
        }
        if (Input.GetKey(KeyCode.Alpha2)) //zoom in
        {
		cameraSpeed +=0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) //zoom in
        {
		cameraSpeed -=0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) //zoom in
        {
		cameraSpeed +=0.1f;
        }
        //if (Input.GetKey(KeyCode.PageUp))
        //{
        //    transform.Rotate(0.1f, 0, 0);
        //    angle = (Mathf.PI / 180) * transform.eulerAngles.x;
        //}
        //if (Input.GetKey(KeyCode.PageDown))
        //{
        //    transform.Rotate(-0.1f, 0, 0);
        //    angle = (Mathf.PI / 180) * transform.eulerAngles.x;
        //}

        //if (Input.GetKey(KeyCode.Alpha0))
        //{
        //    Debug.Log("speed: " + cameraSpeed);
        //    Debug.Log("y position: " + transform.position.y);
        //}

        //if (Input.GetKey(KeyCode.Alpha1))
        //{
        //    float distance = transform.position.y / Mathf.Sin(angle);
        //    Vector3 pos = transform.position + transform.forward * distance;
        //    transform.RotateAround(pos, Vector3.up, 60 * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.Alpha2))
        //{
        //    float distance = transform.position.y / Mathf.Sin(angle);
        //    Vector3 pos = transform.position + transform.forward * distance;
        //    transform.RotateAround(pos, Vector3.up, -60 * Time.deltaTime);
        //}
    }
}
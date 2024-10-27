using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ex3
{
    [RequireComponent(typeof(Camera))]
    public class MoveCamAround : MonoBehaviour
    {
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        private void Update()
        {
            // Zoom in and out with mouse wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float zoomSpeed = 10;
            if (cam.orthographic) {
                cam.orthographicSize -= scroll * zoomSpeed;
            }
            else
            {
                cam.fieldOfView -= scroll * zoomSpeed;
            }

            // Drag camera with mouse
            if (Input.GetMouseButton(0))
            {
                float moveSpeed = 10;
                float mouseX = Input.GetAxis("Mouse X") * moveSpeed;
                float mouseY = Input.GetAxis("Mouse Y") * moveSpeed;

                cam.transform.Translate(Vector3.up * mouseY * Time.deltaTime);
                cam.transform.Translate(Vector3.right * mouseX * Time.deltaTime);
            }

            // Move camera with arrow keys
            float speed = 10;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                cam.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                cam.transform.Translate(Vector3.back * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cam.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                cam.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }
}

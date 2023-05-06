using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float dragThreshold = 0.05f;
    [SerializeField] private float cameraSpeed = 15;
    [SerializeField] private float scrollSpeed = 10;
    [SerializeField] private float minZoom = 5;
    [SerializeField] private float maxZoom = 10;
    private Camera cam;
    private Vector3 dragOrigin;
    private float xMax;
    private float xMin = 0;
    private float yMin;
    private float yMax = 0;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGamePaused())
        {
            GetInput();
            handleMouseDrag();
            handleScroll();
        }
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * (cameraSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * (cameraSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * (cameraSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * (cameraSpeed * Time.deltaTime));
        }

        transform.position = ClampCamera(transform.position);
    }


    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float newX = Mathf.Clamp(targetPosition.x, xMin, xMax);
        float newY = Mathf.Clamp(targetPosition.y, yMin, yMax);

        return new Vector3(newX, newY, targetPosition.z);
    }


    private void handleScroll()
    {
        var targetZoom = Mathf.Clamp(cam.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * scrollSpeed), minZoom, maxZoom);
        Camera.main.orthographicSize = targetZoom;
    }

    private void handleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            var dragDiff = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            if (dragDiff.magnitude > dragThreshold)
            {
                cam.transform.position = ClampCamera(cam.transform.position + dragDiff);
            }
        }
    }


    public void SetLimits(Vector3 maxTile)
    {
        Vector3 wp = cam.ViewportToWorldPoint(new Vector3(1, 0));

        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;
    }
}
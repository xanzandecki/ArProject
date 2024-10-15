using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera m_currentCamera;
    private Vector3 m_screenPoint;
    private Vector3 m_offset;
    private bool isDragging = false;
    
    void Update()
    {
        // Handle touch input for dragging
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                OnTouchDown(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                OnDrag(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnTouchUp();
            }
        }

        // Handle mouse input for dragging
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            OnDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
    }

    void OnMouseDown()
    {
        OnInteractionDown(Input.mousePosition);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void OnTouchDown(Vector3 inputPosition)
    {
        OnInteractionDown(inputPosition);
    }

    void OnTouchUp()
    {
        isDragging = false;
    }

    void OnInteractionDown(Vector3 inputPosition)
    {
        m_currentCamera = FindCamera();

        if (m_currentCamera != null)
        {
            Ray ray = m_currentCamera.ScreenPointToRay(inputPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                m_screenPoint = m_currentCamera.WorldToScreenPoint(gameObject.transform.position);
                m_offset = gameObject.transform.position - m_currentCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, m_screenPoint.z));
                isDragging = true; // Begin dragging
            }
        }
    }

    void OnDrag(Vector3 inputPosition)
    {
        if (isDragging && m_currentCamera != null)
        {
            Vector3 currentScreenPoint = new Vector3(inputPosition.x, inputPosition.y, m_screenPoint.z);
            Vector3 worldPosition = m_currentCamera.ScreenToWorldPoint(currentScreenPoint) + m_offset;
            transform.position = worldPosition; // Directly move the object
        }
    }

    Camera FindCamera()
    {
#if UNITY_2023_1_OR_NEWER
        Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
#else
        Camera[] cameras = FindObjectsOfType<Camera>();
#endif
        Camera result = null;
        int camerasSum = 0;
        foreach (var camera in cameras)
        {
            if (camera.enabled)
            {
                result = camera;
                camerasSum++;
            }
        }
        if (camerasSum > 1)
        {
            result = null;
        }
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Frustrum : MonoBehaviour
{
    Camera cam;

    private const int maxFrustrumPlanes = 6;
    private const int aabbPoints = 8;

    Plane[] planes = new Plane[maxFrustrumPlanes];

    public List<Vector3> farPoints = new List<Vector3>();
    public List<Vector3> nearPoints = new List<Vector3>();
    public float distance;

    [SerializeField] List<GameObject> TestObjests = new List<GameObject>();


    [SerializeField] Vector3 nearTopLeft;
    [SerializeField] Vector3 nearTopRight;
    [SerializeField] Vector3 nearBottomLeft;
    [SerializeField] Vector3 nearBottomRight;

    [SerializeField] Vector3 farTopLeft;
    [SerializeField] Vector3 farTopRight;
    [SerializeField] Vector3 farBottomLeft;
    [SerializeField] Vector3 farBottomRight;

    [SerializeField] public Vector3 nearCenter;
    [SerializeField] public Vector3 farCenter;

    float halfCameraHeightNear;
    float CameraHalfWidthNear;

    float halfCameraHeightfar;
    float CameraHalfWidthFar;

    [SerializeField] List<RoomObject> roomObjects = new List<RoomObject>();
    private void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        for (int i = 0; i < maxFrustrumPlanes; i++)
        {
            planes[i] = new Plane();
        }

        halfCameraHeightNear = Mathf.Tan((cam.fieldOfView / 2) * Mathf.Deg2Rad) * cam.nearClipPlane;
        CameraHalfWidthNear = (cam.aspect * halfCameraHeightNear);

        halfCameraHeightfar = Mathf.Tan((cam.fieldOfView / 2) * Mathf.Deg2Rad) * cam.farClipPlane;
        CameraHalfWidthFar = (cam.aspect * halfCameraHeightfar);

        distance = cam.farClipPlane;
    }


    private void FixedUpdate()
    {


        UpdatePlanes();
        for (int i = 0; i < roomObjects.Count; i++)
        {
            for (int j = 0; j < roomObjects[i].wordList.Count; j++)
            {
                CheckObjetColition(roomObjects[i].wordList[j]);
            }
        }
    }

    public void setLineDistance(float value) 
    {
        distance = value;
    }
    public float GetDistance() 
    {
        return distance;
    }
    void UpdatePlanes()
    {
        Vector3 frontMultFar = cam.farClipPlane * cam.transform.forward;

        Vector3 nearPos = cam.transform.position;
        nearPos += cam.transform.forward * cam.nearClipPlane;
        planes[0].SetNormalAndPosition(cam.transform.forward, nearPos);


        Vector3 farPos = cam.transform.position;
        farPos += (cam.transform.forward) * cam.farClipPlane;
        planes[1].SetNormalAndPosition(cam.transform.forward * -1, farPos);

        SetNearPoints(nearPos);
        SetFarPoints(farPos);

        planes[2].Set3Points(cam.transform.position, farBottomLeft, farTopLeft);//left
        planes[3].Set3Points(cam.transform.position, farTopRight, farBottomRight);//right
        planes[4].Set3Points(cam.transform.position, farTopLeft, farTopRight);//top
        planes[5].Set3Points(cam.transform.position, farBottomRight, farBottomLeft);//bottom

        for (int i = 2; i < maxFrustrumPlanes; i++)
        {
            planes[i].Flip();
        }
    }

    public void SetNearPoints(Vector3 nearPos)
    {

        Vector3 nearPlaneDistance = cam.transform.position + (cam.transform.forward * cam.nearClipPlane);

        nearTopLeft = nearPlaneDistance + (cam.transform.up * halfCameraHeightNear) - (cam.transform.right * CameraHalfWidthNear);

        nearTopRight = nearPlaneDistance + (cam.transform.up * halfCameraHeightNear) + (cam.transform.right * CameraHalfWidthNear);

        nearBottomLeft = nearPlaneDistance - (cam.transform.up * halfCameraHeightNear) - (cam.transform.right * CameraHalfWidthNear);

        nearBottomRight = nearPlaneDistance - (cam.transform.up * halfCameraHeightNear) + (cam.transform.right * CameraHalfWidthNear);
    }
    public void SetFarPoints(Vector3 farPos)
    {

        Vector3 farPlaneDistance = cam.transform.position + (cam.transform.forward * cam.farClipPlane);


        farTopLeft = farPlaneDistance + (cam.transform.up * halfCameraHeightfar) - (cam.transform.right * CameraHalfWidthFar);

        farTopRight = farPlaneDistance + (cam.transform.up * halfCameraHeightfar) + (cam.transform.right * CameraHalfWidthFar);

        farBottomLeft = farPlaneDistance - (cam.transform.up * halfCameraHeightfar) - (cam.transform.right * CameraHalfWidthFar);

        farBottomRight = farPlaneDistance - (cam.transform.up * halfCameraHeightfar) + (cam.transform.right * CameraHalfWidthFar);
    }

    private Vector3 Rotate(Vector3 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
        float vx = vector.x;
        float vy = vector.y;
        vector.x = (cos * vx) - (sin * vy);
        vector.y = (sin * vx) + (cos * vy);
        return vector;
    }

    public void CheckObjetColition(WorldObject currentObject)
    {
        bool isInside = false;

        for (int i = 0; i < aabbPoints; i++)
        {
            int counter = maxFrustrumPlanes;

            for (int j = 0; j < maxFrustrumPlanes; j++)
            {
                if (planes[j].GetSide(currentObject.aabb[i]))
                {
                    counter--;
                }
            }

            if (counter == 0)
            {
                Debug.Log("Está adentro");
                isInside = true;
                break;
            }
        }

        if (isInside)
        {
            for (int i = 0; i < currentObject.meshFilter.mesh.vertices.Length; i++)
            {
                int counter = maxFrustrumPlanes;

                for (int j = 0; j < maxFrustrumPlanes; j++)
                {
                    if (planes[j].GetSide(currentObject.gameObject.transform.TransformPoint(currentObject.meshFilter.mesh.vertices[i])))
                    {
                        counter--;
                    }
                }

                if (counter == 0)
                {
                    Debug.Log("Está adentro vert ");
                    currentObject.gameObject.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            if (currentObject.gameObject.activeSelf)
            {
                Debug.Log("Está afuera");
                currentObject.gameObject.SetActive(false);
            }
        }
    }
    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        Gizmos.color = Color.green;

        //Plano Cercano
        DrawPlane(nearTopRight, nearBottomRight, nearBottomLeft, nearTopLeft);

        //Plano Lejano
        DrawPlane(farTopRight, farBottomRight, farBottomLeft, farTopLeft);

        // Plano Derecho
        DrawPlane(nearTopRight, farTopRight, farBottomRight, nearBottomRight);

        // Plano Izquierdo
        DrawPlane(nearTopLeft, farTopLeft, farBottomLeft, nearBottomLeft);

        // Plano Superior
        DrawPlane(nearTopLeft, farTopLeft, farTopRight, nearTopRight);

        //Plano Inferior
        DrawPlane(nearBottomLeft, farBottomLeft, farBottomRight, nearBottomRight);

        Gizmos.color = Color.red;
        int fov = (int)cam.fieldOfView;

        for (int i = 0; i < farPoints.Count; i++)
        {
            Gizmos.DrawLine(nearCenter, farPoints[i]);
        }
        Gizmos.color = Color.green;
    }
    public void DrawPlane(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(p1, p3);
        //Gizmos.DrawLine(p2, p4);
        Gizmos.color = Color.green;
    }
}
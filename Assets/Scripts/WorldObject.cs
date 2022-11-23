using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] bool isStatic;
    [SerializeField] bool showPoints;


    const int aabbPoints = 8;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Vector3[] aabb;
    public Vector3 v3Extents;
    public Vector3 scale;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        aabb = new Vector3[aabbPoints];
        v3Extents = meshRenderer.bounds.extents;
    }
    private void Start()
    {
        SetAABBPoints();
        if (isStatic)
        {
            enabled = false;
        }
    }
    void Update()
    {
        SetAABBPoints();
    }
    public void SetAABBPoints()
    {
        if (scale != transform.lossyScale)
        {
            Quaternion rotation = transform.rotation;
            transform.rotation = Quaternion.identity;
            v3Extents = meshRenderer.bounds.extents;
            scale = transform.lossyScale;
            transform.rotation = rotation;
        }


        Vector3 center = meshRenderer.bounds.center;
        Vector3 size = v3Extents;

        aabb[0] = new Vector3(center.x - size.x, center.y + size.y, center.z - size.z);  // Front top left corner
        aabb[1] = new Vector3(center.x + size.x, center.y + size.y, center.z - size.z);  // Front top right corner
        aabb[2] = new Vector3(center.x - size.x, center.y - size.y, center.z - size.z);  // Front bottom left corner
        aabb[3] = new Vector3(center.x + size.x, center.y - size.y, center.z - size.z);  // Front bottom right corner
        aabb[4] = new Vector3(center.x - size.x, center.y + size.y, center.z + size.z);  // Back top left corner
        aabb[5] = new Vector3(center.x + size.x, center.y + size.y, center.z + size.z);  // Back top right corner
        aabb[6] = new Vector3(center.x - size.x, center.y - size.y, center.z + size.z);  // Back bottom left corner
        aabb[7] = new Vector3(center.x + size.x, center.y - size.y, center.z + size.z);  // Back bottom right corner


        // tranformar las Posiciones en puntos en el espacio
        //aabb[0] = transform.TransformPoint(aabb[0]);
        //aabb[1] = transform.TransformPoint(aabb[1]);
        //aabb[2] = transform.TransformPoint(aabb[2]);
        //aabb[3] = transform.TransformPoint(aabb[3]);
        //aabb[4] = transform.TransformPoint(aabb[4]);
        //aabb[5] = transform.TransformPoint(aabb[5]);
        //aabb[6] = transform.TransformPoint(aabb[6]);
        //aabb[7] = transform.TransformPoint(aabb[7]);


        // Roto el punto en la direccion que rota el objeto (Punto a rotar , pivot en el que rota , angulo en cada eje)
        aabb[0] = RotatePointAroundPivot(aabb[0], transform.position, transform.rotation.eulerAngles);
        aabb[1] = RotatePointAroundPivot(aabb[1], transform.position, transform.rotation.eulerAngles);
        aabb[2] = RotatePointAroundPivot(aabb[2], transform.position, transform.rotation.eulerAngles);
        aabb[3] = RotatePointAroundPivot(aabb[3], transform.position, transform.rotation.eulerAngles);
        aabb[4] = RotatePointAroundPivot(aabb[4], transform.position, transform.rotation.eulerAngles);
        aabb[5] = RotatePointAroundPivot(aabb[5], transform.position, transform.rotation.eulerAngles);
        aabb[6] = RotatePointAroundPivot(aabb[6], transform.position, transform.rotation.eulerAngles);
        aabb[7] = RotatePointAroundPivot(aabb[7], transform.position, transform.rotation.eulerAngles);

    }
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        DrawAABB();
        if (showPoints)
        {
            DrawVert();
        }
    }

    public void DrawAABB()
    {

        Gizmos.color = Color.blue;

        if (!Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < aabbPoints; i++)
        {
            Gizmos.DrawSphere(aabb[i], 0.05f);
        }


        // Draw the AABB Box 
        Gizmos.DrawLine(aabb[0], aabb[1]);
        Gizmos.DrawLine(aabb[1], aabb[3]);
        Gizmos.DrawLine(aabb[3], aabb[2]);
        Gizmos.DrawLine(aabb[2], aabb[0]);
        Gizmos.DrawLine(aabb[0], aabb[4]);
        Gizmos.DrawLine(aabb[4], aabb[5]);
        Gizmos.DrawLine(aabb[5], aabb[7]);
        Gizmos.DrawLine(aabb[7], aabb[6]);
        Gizmos.DrawLine(aabb[6], aabb[4]);
        Gizmos.DrawLine(aabb[7], aabb[3]);
        Gizmos.DrawLine(aabb[6], aabb[2]);
        Gizmos.DrawLine(aabb[5], aabb[1]);

        Gizmos.color = Color.green;
    }
    public void DrawVert()
    {

        Gizmos.color = Color.red;

        for (int i = 0; i < meshFilter.mesh.vertices.Length; i++)
        {
            Gizmos.DrawSphere(gameObject.transform.TransformPoint(meshFilter.mesh.vertices[i]), 0.05f);
        }

        Gizmos.color = Color.green;
    }
}

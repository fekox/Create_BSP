using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binaryspacepartitioningalgorithm : MonoBehaviour
{

    const int walls = 12;
    const int wordMaxPoints = 72;

    Camera cam;

    [SerializeField] Frustrum frustrum;
    [SerializeField] Plane[] wallPlanes = new Plane[walls];

    List<Vector3> colitionPoints = new List<Vector3>();
    List<Vector3> colitionPointsDistance = new List<Vector3>();

    public Vector3[] wordPoints;

    void Start()
    {
        for (int i = 0; i < walls; i++)
        {
            wallPlanes[i] = new Plane();
        }
        wordPoints = new Vector3[wordMaxPoints];

        setWordPoints();
        setWordPlanes();


        cam = Camera.main;

        initPoints();
    }

    void Update()
    {
        checkColitions();
        updatePoints();
    }

    public void checkColitions() 
    {
        colitionPoints.Clear();

        for (int i = 0; i < frustrum.farPoints.Count; i++)
        {
            List<Vector3> aux = new List<Vector3>();

            for (int j = 0; j < walls; j++)
            {
                Vector3 point = checkRayPlaneColition(frustrum.nearCenter, frustrum.farPoints[i], ref wallPlanes[j]);
                
                if (Mathf.Sign(Vector3.Dot(point,cam.transform.forward)) == 1)
                {
                    aux.Add(point);
                }
            }

            Vector3 auxVec;

            if (aux.Count != 0)
            {
                auxVec = aux[0];

                for (int h = 0; h < aux.Count; h++)
                {
                    if (Vector3.Distance(auxVec, frustrum.nearCenter) > Vector3.Distance(aux[h], frustrum.nearCenter))
                    {
                        auxVec = aux[h];
                    }
                }

                colitionPoints.Add(auxVec);
            }
        }
    }

    public void initPoints() 
    {
        frustrum.nearCenter = cam.transform.position;
        frustrum.nearCenter += cam.transform.forward * cam.nearClipPlane;

        frustrum.farCenter = cam.transform.position;
        frustrum.farCenter += (cam.transform.forward) * cam.farClipPlane;

        int fov = (int)(cam.fieldOfView * cam.aspect);

        frustrum.farPoints.Clear();


        frustrum.farPoints.Add(Quaternion.Euler(0.0f, (float)(-fov / 2), 0.0f) * (cam.transform.forward * cam.farClipPlane));

        for (int i = 1; i < fov; i++)
        {
            frustrum.farPoints.Add(Quaternion.Euler(new Vector3(0.0f, (float)i, 0.0f)) * frustrum.farPoints[0]);
        }
    }

    public void updatePoints()
    {
        frustrum.nearCenter = cam.transform.position;
        frustrum.nearCenter += cam.transform.forward * cam.nearClipPlane;

        frustrum.farCenter = cam.transform.position;
        frustrum.farCenter += (cam.transform.forward) * cam.farClipPlane;


        int fov = (int)(cam.fieldOfView * cam.aspect);

        frustrum.farPoints.Clear();


        frustrum.farPoints.Add(Quaternion.Euler(0.0f, (float)(-fov / 2), 0.0f) * (cam.transform.forward * cam.farClipPlane));

        for (int i = 1; i < fov; i++)
        {
            frustrum.farPoints.Add(Quaternion.Euler(new Vector3(0.0f, (float)i, 0.0f)) * frustrum.farPoints[0]);
        }
    }

    public Vector3 checkRayPlaneColition(Vector3 a, Vector3 b, ref Plane currentPlane)
    {
        Vector3 ba = b - a;
        float nDotA = Vector3.Dot(currentPlane.normal, a);
        float nDotBA = Vector3.Dot(currentPlane.normal, ba);

        return a + (((currentPlane.distance - nDotA) / nDotBA) * ba);
    }
    public void setWordPoints()
    {
        // Esquina principal
        wordPoints[0] = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z + 10);
        wordPoints[1] = new Vector3(transform.position.x - 10, transform.position.y + 5, transform.position.z + 10);

        // Esquina final primera fila
        wordPoints[2] = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z - 50);
        wordPoints[3] = new Vector3(transform.position.x - 10, transform.position.y + 5, transform.position.z - 50);

        // Esquina final tercera fila
        wordPoints[4] = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z - 50);
        wordPoints[5] = new Vector3(transform.position.x + 50, transform.position.y + 5, transform.position.z - 50);

        // Esquina final cuarta fila
        wordPoints[6] = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z + 10);
        wordPoints[7] = new Vector3(transform.position.x + 50, transform.position.y + 5, transform.position.z + 10);


        // paredes intermedias
        wordPoints[8] = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z - 10);
        wordPoints[9] = new Vector3(transform.position.x - 10, transform.position.y + 5, transform.position.z - 10);

        wordPoints[10] = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z - 30);
        wordPoints[11] = new Vector3(transform.position.x - 10, transform.position.y + 5, transform.position.z - 30);

        wordPoints[12] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z - 50);
        wordPoints[13] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z - 50);

        wordPoints[14] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z - 50);
        wordPoints[15] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z - 50);

        wordPoints[16] = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z - 30);
        wordPoints[17] = new Vector3(transform.position.x + 50, transform.position.y + 5, transform.position.z - 30);

        wordPoints[18] = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z - 10);
        wordPoints[19] = new Vector3(transform.position.x + 50, transform.position.y + 5, transform.position.z - 10);

        wordPoints[20] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z + 10);
        wordPoints[21] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z + 10);

        wordPoints[22] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z + 10);
        wordPoints[23] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z + 10);

        // puntos internos

        // primera puerta
        wordPoints[24] = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z - 10);
        wordPoints[25] = new Vector3(transform.position.x - 3, transform.position.y + 5, transform.position.z - 10);

        wordPoints[26] = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z - 10);
        wordPoints[27] = new Vector3(transform.position.x + 3, transform.position.y + 5, transform.position.z - 10);

        // segunda puerta
        wordPoints[28] = new Vector3(transform.position.x + 17, transform.position.y, transform.position.z - 10);
        wordPoints[29] = new Vector3(transform.position.x + 17, transform.position.y + 5, transform.position.z - 10);

        wordPoints[30] = new Vector3(transform.position.x + 23, transform.position.y, transform.position.z - 10);
        wordPoints[31] = new Vector3(transform.position.x + 23, transform.position.y + 5, transform.position.z - 10);

        // tercera puerta
        wordPoints[32] = new Vector3(transform.position.x + 37, transform.position.y, transform.position.z - 10);
        wordPoints[33] = new Vector3(transform.position.x + 37, transform.position.y + 5, transform.position.z - 10);

        wordPoints[34] = new Vector3(transform.position.x + 43, transform.position.y, transform.position.z - 10);
        wordPoints[35] = new Vector3(transform.position.x + 43, transform.position.y + 5, transform.position.z - 10);

        // cuarta puerta
        wordPoints[36] = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z - 30);
        wordPoints[37] = new Vector3(transform.position.x - 3, transform.position.y + 5, transform.position.z - 30);

        wordPoints[38] = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z - 30);
        wordPoints[39] = new Vector3(transform.position.x + 3, transform.position.y + 5, transform.position.z - 30);

        // quinta puerta
        wordPoints[40] = new Vector3(transform.position.x + 17, transform.position.y, transform.position.z - 30);
        wordPoints[41] = new Vector3(transform.position.x + 17, transform.position.y + 5, transform.position.z - 30);

        wordPoints[42] = new Vector3(transform.position.x + 23, transform.position.y, transform.position.z - 30);
        wordPoints[43] = new Vector3(transform.position.x + 23, transform.position.y + 5, transform.position.z - 30);

        // sexta puerta
        wordPoints[44] = new Vector3(transform.position.x + 37, transform.position.y, transform.position.z - 30);
        wordPoints[45] = new Vector3(transform.position.x + 37, transform.position.y + 5, transform.position.z - 30);

        wordPoints[46] = new Vector3(transform.position.x + 43, transform.position.y, transform.position.z - 30);
        wordPoints[47] = new Vector3(transform.position.x + 43, transform.position.y + 5, transform.position.z - 30);

        // septima puerta
        wordPoints[48] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z - 43);
        wordPoints[49] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z - 43);

        wordPoints[50] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z - 37);
        wordPoints[51] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z - 37);

        // octaba puerta
        wordPoints[52] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z - 23);
        wordPoints[53] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z - 23);

        wordPoints[54] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z - 17);
        wordPoints[55] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z - 17);

        // novena puerta
        wordPoints[56] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z - 3);
        wordPoints[57] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z - 3);

        wordPoints[58] = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z + 3);
        wordPoints[59] = new Vector3(transform.position.x + 10, transform.position.y + 5, transform.position.z + 3);

        // decima puerta
        wordPoints[60] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z - 43);
        wordPoints[61] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z - 43);

        wordPoints[62] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z - 37);
        wordPoints[63] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z - 37);

        // decimo primera puerta
        wordPoints[64] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z - 23);
        wordPoints[65] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z - 23);

        wordPoints[66] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z - 17);
        wordPoints[67] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z - 17);

        // decimo segunda puerta
        wordPoints[68] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z - 3);
        wordPoints[69] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z - 3);

        wordPoints[70] = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z + 3);
        wordPoints[71] = new Vector3(transform.position.x + 30, transform.position.y + 5, transform.position.z + 3);
    }
    public void setWordPlanes()
    {
        wallPlanes[0].Set3Points(wordPoints[2], wordPoints[3], wordPoints[1]);
        wallPlanes[1].Set3Points(wordPoints[4], wordPoints[5], wordPoints[3]);
        wallPlanes[2].Set3Points(wordPoints[6], wordPoints[7], wordPoints[5]);
        wallPlanes[3].Set3Points(wordPoints[0], wordPoints[1], wordPoints[7]);
        wallPlanes[4].Set3Points(wordPoints[9], wordPoints[19], wordPoints[18]);
        wallPlanes[5].Set3Points(wordPoints[18], wordPoints[19], wordPoints[9]);
        wallPlanes[6].Set3Points(wordPoints[11], wordPoints[17], wordPoints[16]);
        wallPlanes[7].Set3Points(wordPoints[16], wordPoints[17], wordPoints[11]);
        wallPlanes[8].Set3Points(wordPoints[13], wordPoints[21], wordPoints[20]);
        wallPlanes[9].Set3Points(wordPoints[20], wordPoints[21], wordPoints[13]);
        wallPlanes[10].Set3Points(wordPoints[15], wordPoints[23], wordPoints[22]);
        wallPlanes[11].Set3Points(wordPoints[22], wordPoints[23], wordPoints[15]);

        //for (int i = 0; i < walls; ++i)
        //{
        //    GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //    p.name = "Plane " + i.ToString();
        //    p.transform.position = -wallPlanes[i].normal * wallPlanes[i].distance;
        //    p.transform.rotation = Quaternion.FromToRotation(Vector3.up, wallPlanes[i].normal);
        //}
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        Gizmos.color = Color.black;

        for (int i = 0; i < wordMaxPoints; i++)
        {
            Gizmos.DrawSphere(wordPoints[i], 0.5f);
        }

        Gizmos.color = Color.blue;

        for (int i = 0; i < colitionPoints.Count; i++)
        {
            Gizmos.DrawSphere(colitionPoints[i], 0.5f);
        }

        //for (int i = 0; i < colitionPoints.Count; i++)
        //{
        //    Gizmos.DrawLine(frustrum.nearCenter, colitionPoints[i]);
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP : MonoBehaviour
{
    struct Line
    {
        public Vector3 start;
        public Vector3 dir;
    }

    [SerializeField] private Room[] rooms;
    [SerializeField] private float lineMargin;
    [SerializeField] private float renderDis;
    [SerializeField] private float iterFreq;

    private Line[] lines = new Line[25];


    private void OnDrawGizmos()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            //chequeo si la camara esta en el cuarto
            if (rooms[i].PosInsideRoom(transform.position))
            {
                rooms[i].isVisible = true;
            }
            else
            {
                rooms[i].isVisible = false;
            }
        }

        InitLines();

        DrawLines();

        for (int i = 0; i < lines.Length; i++)
        {
            CheckLine(lines[i]);
        }
    }

    private void CheckLine(Line line) //Va chequeando cada punto de la linea imaginaria.
    {
        Vector3[] points = new Vector3[(int)(renderDis / iterFreq)];

        Gizmos.color = Color.green;

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = line.start + line.dir * iterFreq * i;
            Gizmos.DrawSphere(points[i], 0.1f);

            for (int j = 0; j < rooms.Length; j++)
            {
                //chequeo si el punto esta en el cuarto
                if (rooms[j].PosInsideRoom(points[i]))
                {
                    rooms[j].isVisible = true;
                }

            }
        }
    }

    public void InitLines() // Inicializa las lineas segun la posicion inicial y la direccion.
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].start = transform.position;
            lines[i].dir = (transform.forward * renderDis + transform.right * lineMargin * (-lines.Length / 2) + i * transform.right * lineMargin).normalized;

        }
    }

    public void DrawLines() //Dibuja las lineas. (Los puntos verdes)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            Vector3[] points = new Vector3[(int)(renderDis / iterFreq)];

            Gizmos.color = Color.green;

            for (int j = 0; j < points.Length; j++)
            {
                points[j] = lines[i].start + lines[i].dir * iterFreq * j;
                Gizmos.DrawSphere(points[j], 0.1f);
            }
        }
    }
}

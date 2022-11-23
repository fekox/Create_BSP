using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomObject : MonoBehaviour
{

    public  List<WorldObject> wordList = new List<WorldObject>();
    const int walls = 4;
    [SerializeField] Plane[] wallPlanes = new Plane[walls];
    Vector3 test = new Vector3(0, 0, 0);

    private void Awake()
    {
       
        wordList = GetComponentsInChildren<WorldObject>().ToList();
    }
    public void Start()
    {
        

    }
    public void Update()
    {
        pointInRoom(test);
    }
    public void pointInRoom(Vector3 point) 
    {

    }
}

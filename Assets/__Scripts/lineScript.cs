using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineScript : MonoBehaviour
{
    public GameObject sphere;
    public GameObject launchPoint;
    LineRenderer lineRend;
    // Start is called before the first frame update
    void Start()
    {
        lineRend = gameObject.GetComponent<LineRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        
        List<Vector3> pos = new List<Vector3>();
        pos.Add(sphere.transform.position);
        pos.Add(launchPoint.transform.position);
        lineRend.startWidth = 10f;
        lineRend.endWidth = 10f;
        lineRend.SetPositions(pos.ToArray());
        lineRend.useWorldSpace = true;
    }
}

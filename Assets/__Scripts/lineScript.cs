using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineScript : MonoBehaviour
{
    private LineRenderer LR;
    public List<Vector2> positions;
    private Renderer Rend;
    Vector3 pos;
    void Start()
    {
        LR = GetComponent<LineRenderer>();
        Rend = GetComponent<Renderer>();
        //Set your GameObject position from where you want draw a line
        pos = new Vector3(0f, 0f, 0f);
    }
    void Update()
    {
        LR.SetPosition(0, pos);
        Vector3 CursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LR.SetPosition(1, CursorPosition);

    }
}

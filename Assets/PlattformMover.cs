using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformMover : MonoBehaviour
{
    public enum path { Vertical, Horizontal }
    public path pathType;
    Vector2 movePos;
    Vector2 startPos;
    public float moveFreq;
    public float moveDis;

    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if (pathType == path.Horizontal)
        {

            movePos.x = startPos.x + Mathf.Sin(Time.time * moveFreq) * moveDis;
            transform.position = new Vector2(movePos.x, transform.position.y);




        }
        if (pathType == path.Vertical)
        {
            movePos.y = startPos.y + Mathf.Sin(Time.time * moveFreq) * moveDis;
            transform.position = new Vector2(transform.position.x, movePos.y);
        }

    }
}

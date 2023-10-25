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

    private Rigidbody2D rb;

    Transform objectParent;
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (pathType == path.Horizontal)
        {

            movePos.x = startPos.x + Mathf.Sin(Time.time * moveFreq) * moveDis;
            //transform.position = new Vector2(movePos.x, transform.position.y);
            rb.position = (new Vector2(movePos.x, transform.position.y));


        }
        if (pathType == path.Vertical)
        {
            movePos.y = startPos.y + Mathf.Sin(Time.time * moveFreq) * moveDis;
            //transform.position = new Vector2(transform.position.x, movePos.y);
            rb.MovePosition(new Vector2(transform.position.x, movePos.y));
        }

    }
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
       objectParent = collision.transform.parent;

        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(collision.gameObject.name);
            collision.gameObject.transform.SetParent(transform, true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(objectParent, true);
        }
    }
}

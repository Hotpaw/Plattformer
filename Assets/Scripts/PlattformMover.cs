using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlattformMover : MonoBehaviour
{
    public enum path { Vertical, Horizontal }
    public path pathType;
    Vector2 movePos;
    Vector2 startPos;
    public float moveFreq;
    public float moveDis;

    private Rigidbody2D rb;


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
            transform.position = new Vector2(movePos.x, transform.position.y);



        }
        if (pathType == path.Vertical)
        {
            movePos.y = startPos.y + Mathf.Sin(Time.time * moveFreq) * moveDis;
            transform.position = new Vector2(transform.position.x, movePos.y);

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Gamepad currentGamepad = Gamepad.current;
        bool leftstick = currentGamepad.leftStick.IsActuated() 
            || currentGamepad.dpad.IsActuated() 
            || Keyboard.current.aKey.IsActuated() 
            || Keyboard.current.dKey.IsActuated();
        


        if (leftstick)
        {
            collision.gameObject.transform.SetParent(null);
        }
        if (!leftstick)
        {


            collision.gameObject.transform.SetParent(transform, true);


        }
        Debug.Log(leftstick);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D (collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(null);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody2D rb;
    public int characterSpeed=3;
    private Lord lord;
    void Start()
    {
    rb=GetComponent<Rigidbody2D>();
    lord=GetComponent<Lord>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
         {
         Move();   
         }else
         {
        rb.velocity=Vector2.zero;
        }
    }
    private void Move(){
        mousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mousePos=new Vector3(mousePos.x,mousePos.y,0);

        Vector3 difference=mousePos-transform.position;
        difference=difference.normalized;

        rb.velocity=new Vector2(difference.x,difference.y)*Time.deltaTime*lord.speed*100;

    }
}
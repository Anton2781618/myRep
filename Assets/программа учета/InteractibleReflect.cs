using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractibleReflect : MonoBehaviour
{
     Rigidbody2D RB;
    
    Vector2 lastvelo;

    private void Start()
    {
       
        RB = transform.GetComponent<Rigidbody2D>();
       // RB.AddForce(new Vector2(Random.Range(-10000,10000), Random.Range(-10000, 10000)));
        
       
        
    }

    void Update()
    {
        lastvelo = RB.velocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var speed = lastvelo.magnitude;
     
        Vector2 direction = Vector2.Reflect(lastvelo.normalized, other.contacts[0].normal);
        RB.velocity = direction *speed;
       
    }

    void OnTriggerStay2D (Collider2D other)
    {
        if (other.tag == "Finish")
        {
             Debug.DrawLine(transform.position, other.transform.position, Color.white);
           
           
        }
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }
        print("Collided");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, 0.0f);
    }
}

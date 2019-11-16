using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public float thrust;
    public Rigidbody2D rb;
    public float direction = 1f;
    private Vector3 force = new Vector3(3, 0, 0);
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(thrust * force * direction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

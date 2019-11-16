﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    // Start is called before the first frame update

    public float gravity;
    public float radiusScale;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos2d = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos2d, transform.localScale.x * radiusScale);
        int i = 0;
        Rigidbody2D rb;

        Vector3 force;
        print(hitColliders.Length);
        while (i < hitColliders.Length)
        {
   
            rb = hitColliders[i].GetComponent<Rigidbody2D>();
            force = transform.position - hitColliders[i].transform.position;
            float dist = force.magnitude;
            if(dist != 0) {
                Vector3 gravityDirection = force.normalized;
                Vector3 gravityVector = transform.localScale.x * transform.localScale.x * (gravityDirection * gravity) / (dist * dist);

                rb.AddForce(gravityVector, ForceMode2D.Force);
                print(gravityVector.magnitude);
                hitColliders[i].transform.right = rb.velocity.normalized;
            }
            i++;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    // Start is called before the first frame update

    public float gravity = 150;
    public float radiusScale = 20;

    public Sprite[] planetSprites;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = planetSprites[Random.Range(0, planetSprites.Length - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos2d = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos2d, GetComponent<CircleCollider2D>().bounds.size.x / 2 * radiusScale);
        int i = 0;
        Rigidbody2D rb;

        Vector3 force;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "Player" || hitColliders[i].gameObject.tag == "PowerUp")
            {
                i++;
                continue;
            }
            rb = hitColliders[i].GetComponent<Rigidbody2D>();
            if(rb.velocity.magnitude <= 0.0f)
            {
                i++;
                continue;
            }
            force = transform.position - hitColliders[i].transform.position;
            float dist = force.magnitude;
            if (dist != 0) {
                Vector3 gravityDirection = force.normalized;
                Vector3 gravityVector = GetComponent<CircleCollider2D>().bounds.size.x / 2 * GetComponent<CircleCollider2D>().bounds.size.x / 2 * (gravityDirection * gravity) / (dist * dist);

                rb.AddForce(gravityVector, ForceMode2D.Force );
                hitColliders[i].transform.right = rb.velocity.normalized * Time.deltaTime;
            }
            i++;
        }
    }
}

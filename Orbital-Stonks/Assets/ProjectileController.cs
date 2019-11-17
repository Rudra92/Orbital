using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float explosionRange;
    public GameObject explosionPrefab;
    public bool isExplosive = false;
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
            collision.gameObject.GetComponent<PlayerBehaviour>().hit();
            Destroy(gameObject);
            return;
        } else if (collision.tag == "Projectile") {
            return;
        }
        print("Collided");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, 0.0f);
        if (isExplosive) {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);
            for (int i = 0; i < hitColliders.Length; ++i) {
                if (hitColliders[i].tag == "Player") {
                    hitColliders[i].gameObject.GetComponent<PlayerBehaviour>().hit();
                }
            }
            Destroy(gameObject);
        }
    }
}

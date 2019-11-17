using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{

    public float lifeTime;
    public float fullSize;
    public float expandTime;
    private float remainingTime;
    private Color color;
    private float size;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = lifeTime;
        color = this.GetComponent<SpriteRenderer>().material.color;
        size = 0;
        transform.localScale = new Vector3(size, size, 0);
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0) {
            Destroy(gameObject);
        }
        if (remainingTime >= lifeTime - expandTime) {
            size = fullSize * (lifeTime - remainingTime) / expandTime;
            transform.localScale = new Vector3(size, size, 0);
        } else {
            this.GetComponent<SpriteRenderer>().material.color = new Color(color.r, color.g, color.b, remainingTime / (lifeTime - expandTime));
        }
    }
}

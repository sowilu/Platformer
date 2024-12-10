using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 20;
    public Vector2 damageRage = new Vector2(10, 20);
    
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction * speed;
    }

    private void OnColisionEnter2D(Collider2D other)
    {
        var damage = Random.Range(damageRage.x, damageRage.y);
        
        print("Hit" + other.gameObject.name + " for " + damage + " damage");
        Destroy(gameObject);
    }
}

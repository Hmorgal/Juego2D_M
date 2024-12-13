using UnityEngine;

public class EnemyShotControl : MonoBehaviour
{
    
    Rigidbody2D rb;
    [SerializeField] int speed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 shotDirection = EnemyFireControl.PlayerPosition - new Vector2(transform.position.x, transform.position.y);
        rb.linearVelocity = shotDirection.normalized * speed;
        
        Invoke("destroyShot",3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void destroyShot(){

        Destroy(gameObject);

    }

    void OnTriggerEnter2D(Collider2D other){

        if (other.CompareTag("Player")){

            other.SendMessage("hurt");
            Destroy(gameObject);

        }
        

    }
}

using UnityEngine;

public class ShotControl : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] int speed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (PlayerMove.left == false){

            rb.linearVelocity = Vector2.right * speed;

        } else {

            rb.linearVelocity = Vector2.left * speed;

        }
        
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

        if (other.CompareTag("enemy")){

            Destroy(other.gameObject);
            Destroy(gameObject);

        }
        

    }
}

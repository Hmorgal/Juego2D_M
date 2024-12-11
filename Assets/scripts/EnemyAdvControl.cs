using UnityEngine;

public class EnemyAdvControl : MonoBehaviour
{
    [SerializeField] int speed = 3;
    [SerializeField] Vector3 SecondPosition;
    [SerializeField] Vector3 endPosition;

    Vector3 startPosition;
    [SerializeField] int movingTo = 0;
    [SerializeField] SpriteRenderer sprite;
    float prevXPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        prevXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (movingTo==0){

            transform.position = Vector3.MoveTowards(transform.position, SecondPosition, speed * Time.deltaTime);

            if (transform.position == SecondPosition){

                movingTo = 1;

            }

        } else if (movingTo==1) {

            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

            if (transform.position == endPosition){

                movingTo = 2;

            }

        } else if (movingTo==2) {

            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

            if (transform.position == startPosition){

                movingTo = 0;

            }
            
        } 

        if (prevXPos > transform.position.x){

            //Izquierda

            sprite.flipX = false;

        } else if (prevXPos < transform.position.x){

            sprite.flipX = true;

        }

        prevXPos = transform.position.x;

    }

    void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.tag == "Player"){

            if (GameManager.invulnerable == false){

                // other.gameObject.GetComponent<PlayerMove>().hurt();

                other.gameObject.SendMessage("hurt");

            }

        }

    }
}

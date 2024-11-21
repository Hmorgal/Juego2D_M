using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrabControl : MonoBehaviour
{

    [SerializeField] int speed = 3;
    [SerializeField] Vector3 endPosition;
    Vector3 startPosition;
    bool movingToEnd = true;
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
        if (movingToEnd==true){

            transform.position = Vector3.MoveTowards(
                                transform.position,
                                endPosition,
                                speed * Time.deltaTime);

            if (transform.position == endPosition){

                movingToEnd = false;

            }

        } else {

            transform.position = Vector3.MoveTowards(
                                transform.position,
                                startPosition,
                                speed * Time.deltaTime);

            if (transform.position == startPosition){

                movingToEnd = true;

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

            SceneManager.LoadScene("Level_1");

        }

    }
}

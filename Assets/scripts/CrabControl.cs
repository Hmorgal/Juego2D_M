using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrabControl : MonoBehaviour
{

    [SerializeField] int speed = 3;
    [SerializeField] Vector3 endPosition;
    Vector3 startPosition;
    bool movingToEnd = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
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
    }

    void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.tag == "Player"){

            SceneManager.LoadScene("Level_1");

        }

    }
}

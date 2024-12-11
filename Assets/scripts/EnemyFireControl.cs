using UnityEngine;

public class EnemyFireControl : MonoBehaviour
{
    public static Vector2 PlayerPosition;
    [SerializeField] GameObject Player;

    void Start(){

        Player = GameObject.FindGameObjectWithTag("Player");

    }

    // Esto se usa cuando el jugador entre en rango

    void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.tag == "Player"){

            transform.parent.GetComponent<JumperControl>().PlayerInRange = true;

        }

    }

    // Esto se usa cuando el jugador sigue en rango

    void OnTriggerStay2D(Collider2D other){

        if (other.gameObject.tag == "Player"){

            PlayerPosition = Player.transform.position;

        }

    }

    // Esto se usa cuando el jugador sale del rango

    void OnTriggerExit2D(Collider2D other){

        if (other.gameObject.tag == "Player"){

            transform.parent.GetComponent<JumperControl>().PlayerInRange = false;

        }

    }

}

using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumperControl : MonoBehaviour
{

    public bool PlayerInRange;
    [SerializeField] GameObject shot;
    [SerializeField] bool EnemyFireColdown;

    [SerializeField] AudioClip sndShot;

    AudioSource audioSrc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerInRange == true && EnemyFireColdown == false){

            Instantiate(shot, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            audioSrc.PlayOneShot(sndShot);

            EnemyFireColdown = true;

            Invoke("FinishColdown", 5);

        }

    }

    void FinishColdown(){

        EnemyFireColdown = false;

    }

    void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.tag == "Player"){

            if (GameManager.invulnerable == false){

                other.gameObject.SendMessage("hurt");

            }

        }

    }
}

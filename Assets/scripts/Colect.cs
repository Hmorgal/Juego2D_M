using UnityEngine;

public class Colect : MonoBehaviour
{

    public GameObject gema;
    int contador = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gema.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){

        if (other.CompareTag("item")){

            Destroy(other.gameObject);

            contador++;

            print(contador);

        }

    }
}

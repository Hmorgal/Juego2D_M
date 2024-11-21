using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour

{

    public int MoveSpeed = 4;
    public int Jump = 5;
    public int JumpCount = 1;
    public int JumpMaxCount = 1;

    Rigidbody2D rb;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float inputX = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(inputX * MoveSpeed, rb.linearVelocity.y);

        if (inputX == 0){

            anim.SetBool("IsRunning", false);

        } else {

            anim.SetBool("IsRunning", true);

        }

        //Controla el salto desde tierra

        if (Input.GetKeyDown(KeyCode.Space) && TouchingGround() == true){

            rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);

        }

        //Controla el doble salto

        if (Input.GetKeyDown(KeyCode.Space) && TouchingGround() == false && JumpCount > 0){

            // rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 8);

            JumpCount--;

        }

        if (TouchingGround() == true){

            JumpCount = JumpMaxCount;

        }

        

        //Mira si el jugador mira a la derecha o a la izquierda

        if (inputX>0){

            sprite.flipX = false;   

        } else if (inputX<0){

            sprite.flipX = true; 

        }

        //Animacion de salto

        if (NoJumping() == true){

            anim.SetBool("IsJumping", false);

        } else {

            anim.SetBool("IsJumping", true);

        }


    }

    void OnTriggerStay2D(Collider2D other){

        if (other.gameObject.tag == "Water"){

            JumpCount = 1;

        }

    }

    bool NoJumping(){

        RaycastHit2D toucht = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            0.7f);

        if (toucht.collider == null){

            return false;

        }else{

            return true;

        }

    }

    bool TouchingGround(){

        RaycastHit2D toucht = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            0.2f);

        if (toucht.collider == null){

            return false;

        }else{

            return true;

        }

    }
}

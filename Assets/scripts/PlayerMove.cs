using UnityEngine;

public class PlayerMove : MonoBehaviour

{

    public int MoveSpeed = 4;
    public int Jump = 5;

    Rigidbody2D rb;

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

        if (Input.GetKeyDown(KeyCode.Space) && TouchingGround() == true){

            rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);

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

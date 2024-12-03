using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour

{

    public int MoveSpeed = 4;
    public int Jump = 5;
    public int JumpCount = 1;
    public int JumpMaxCount = 1;
    public GameObject gema;

    public static bool left = false;

    int contador = 0;

    Rigidbody2D rb;

    [SerializeField] GameObject shot;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;

    [SerializeField] TMP_Text txtLives, txtItems, txtTimes;

    [SerializeField] GameObject txtWin, txtLose;

    [SerializeField] int startLives = 3;
    [SerializeField] int startTime = 180;

    bool win = false;
    bool lose = false;

    float time;

    [SerializeField] AudioClip sndJump, sndShot, sndItem, sndHurt;

    AudioSource audioSrc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        audioSrc = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        gema.SetActive(true);

        txtLose.SetActive(false);
        txtWin.SetActive(false);

        time = startTime;

        GameManager.invulnerable = false;

        GameManager.lives = startLives;

        txtLives.text = "Vidas: " + GameManager.lives;

        txtItems.text = "Items: " + contador;

        txtTimes.text = time.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!win && !lose){
        
            float inputX = Input.GetAxis("Horizontal");

            rb.linearVelocity = new Vector2(inputX * MoveSpeed, rb.linearVelocity.y);

            if (inputX == 0){

                anim.SetBool("IsRunning", false);

            } else {

                anim.SetBool("IsRunning", true);
                anim.SetBool("IsCrouching", false);

            }

            //Controla el salto desde tierra

            if (Input.GetKeyDown(KeyCode.Space) && TouchingGround() == true){

                rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);

                audioSrc.PlayOneShot(sndJump);

            }

            //Controla el doble salto

            if (Input.GetKeyDown(KeyCode.Space) && TouchingGround() == false && JumpCount > 0){

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 8);

                JumpCount--;

                audioSrc.PlayOneShot(sndJump);

            }

            if (TouchingGround() == true){

                JumpCount = JumpMaxCount;

            }

            //Mira si el jugador mira a la derecha o a la izquierda

            if (inputX>0){

                sprite.flipX = false;
                left = false;

            } else if (inputX<0){

                sprite.flipX = true;
                left = true; 

            }

            //Mira si el jugador se agacha

            if (Input.GetKeyDown(KeyCode.S)){

                anim.SetBool("IsCrouching", true);

            } else if (Input.GetKeyUp(KeyCode.S)){

                anim.SetBool("IsCrouching", false);

            }

            //Animacion de salto

            if (NoJumping() == true){

                anim.SetBool("IsJumping", false);

            } else {

                anim.SetBool("IsJumping", true);

            }

            //Disparo

            if (Input.GetMouseButtonDown(0)){

                anim.SetBool("IsShooting", true);

                audioSrc.PlayOneShot(sndShot);

                Instantiate(shot, new Vector3(transform.position.x, transform.position.y + 1.7f, 0), Quaternion.identity);

            }

            //Temporizador

            time = time - Time.deltaTime;

            if (time <= 0){

                time = 0;

                lose = true;

                txtLose.SetActive(true);

                Invoke("GoToMenu", 3);

            }

            //Esto calcula los minutos y segundos

            float minutes, seconds;

            minutes = Mathf.Floor(time/60);

            seconds = Mathf.Floor(time % 60);

            //Esto muestra el contador de forma correcta
            
            txtTimes.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        
        } else {

            rb.linearVelocity = Vector2.zero;

        }

    }

    //Mira la colisión con los objetos

    void OnTriggerEnter2D(Collider2D other){

        //Gema

        if (other.CompareTag("item")){

            Destroy(other.gameObject);

            audioSrc.PlayOneShot(sndItem);

            contador++;

            txtItems.text =  "Items: " + contador;

            if (contador == 10){

                txtWin.SetActive(true);

                win = true;

                Invoke("GoToCredits", 3);

            }

        }

        //Item de invencibilidad

        if (other.CompareTag("Invencible")){

            Destroy(other.gameObject);

            audioSrc.PlayOneShot(sndItem);

            becomeInvincible();

        }

    }

    //Agua

    void OnTriggerStay2D(Collider2D other){

        if (other.gameObject.tag == "Water"){

            JumpCount = 1;

        }

    }

    //Item de invencibilidad

    void becomeInvincible(){

        sprite.color = Color.green;

        GameManager.invulnerable = true;

        Invoke("becomeVulnerable", 5);

    }

    void becomeVulnerable(){

        sprite.color = Color.white;

        GameManager.invulnerable = false;

    }

    // Daño recibido

    public void hurt(){

        GameManager.lives --;

        audioSrc.PlayOneShot(sndHurt);

        txtLives.text = "Vidas: " + GameManager.lives;

        sprite.color = Color.red;

        GameManager.invulnerable = true;

        Invoke("becomeVulnerable", 2);

        
        if (GameManager.lives == 0){

            lose = true;

            GameManager.lives = 3;

            GameManager.invulnerable = false;

            txtLose.SetActive(true);

            Invoke("GoToMenu", 3);

        }

    }

    //Este metodo manda al menu principal

    void GoToMenu(){

        SceneManager.LoadScene("MainMenu");

    }

    void GoToCredits(){

        SceneManager.LoadScene("Credits");

    }

    //comprueba si el personaje esta en el aire

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

    //comprueba que el personaje esté tocando el suelo

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

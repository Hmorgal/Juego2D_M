using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour

{

    public int MoveSpeed = 4;
    public int Jump = 5;
    public int JumpCount = 1;
    public int JumpMaxCount = 1;
    public int AirAmount = 8;
    public GameObject gema;

    public static bool left = false;
    
    int contador = 0;

    [SerializeField] int NeededGems = 10;

    Rigidbody2D rb;
    CapsuleCollider2D PlayerCollider;

    Vector2 ColliderPSize;
    Vector2 ColliderPOffset;

    Vector2 ColliderPSizeCrouch;
    Vector2 ColliderPOffsetCrouch;

    [SerializeField] GameObject shot;
    [SerializeField] GameObject AirMeter;

    [SerializeField] SpriteRenderer sprite;
    
    [SerializeField] Animator anim;
    [SerializeField] Animator AirMeterAnim;

    [SerializeField] TMP_Text txtLives, txtItems, txtTimes;

    [SerializeField] GameObject txtWin, txtLose, txtDJump;

    [SerializeField] int startLives = 3;
    [SerializeField] int startTime = 180;

    bool win = false;
    bool lose = false;
    public bool level1F = false;
    public bool DJump = false;

    bool ShotInCoolDown = false;
    bool AirInCoolDown = false;

    float time;

    [SerializeField] AudioClip sndJump, sndShot, sndItem, sndHurt;

    AudioSource audioSrc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        audioSrc = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();

        PlayerCollider = GetComponent<CapsuleCollider2D>();

        ColliderPSize = new Vector2(PlayerCollider.size.x, PlayerCollider.size.y);
        ColliderPOffset = new Vector2(PlayerCollider.offset.x, PlayerCollider.offset.y);

        ColliderPSizeCrouch = new Vector2(PlayerCollider.size.x, 1.44663f);
        ColliderPOffsetCrouch = new Vector2(PlayerCollider.offset.x, 0.7278455f);

        gema.SetActive(true);
        AirMeter.SetActive(false);

        txtLose.SetActive(false);
        txtWin.SetActive(false);
        txtDJump.SetActive(false);

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
            float inputY = Input.GetAxis("Vertical");

            if (inputY >= 0){

                rb.linearVelocity = new Vector2(inputX * MoveSpeed, rb.linearVelocity.y);

                if (inputX == 0){

                    anim.SetBool("IsRunning", false);

                } else {

                    anim.SetBool("IsRunning", true);
                    anim.SetBool("IsCrouching", false);

                }

            } else {

                inputX = 0;

                rb.linearVelocity = new Vector2(inputX * MoveSpeed, rb.linearVelocity.y);

                if (inputX == 0){

                    anim.SetBool("IsRunning", false);

                } else {

                    anim.SetBool("IsRunning", true);
                    anim.SetBool("IsCrouching", false);

                }

            }

            //control el movimiento en las escaleras

            if (GameManager.onLader == true){

                if (inputY > 0){

                    rb.linearVelocity = new Vector2(rb.linearVelocityX, inputY * MoveSpeed);

                } else if (inputY < 0){

                    rb.linearVelocity = new Vector2(rb.linearVelocityX, inputY * MoveSpeed);

                }


            }

            //Controla el salto desde tierra

            if (Input.GetKeyDown(KeyCode.Space) && TouchingGround() == true){

                rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);

                audioSrc.PlayOneShot(sndJump);

            }

            //Controla el doble salto

            if (DJump == true){

                if (Input.GetKeyDown(KeyCode.Space) && TouchingGround() == false && JumpCount > 0){

                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 8);

                    JumpCount--;

                    audioSrc.PlayOneShot(sndJump);

                }

                if (TouchingGround() == true){

                    JumpCount = JumpMaxCount;

                }

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

            if (inputY < 0){

                anim.SetBool("IsCrouching", true);

                PlayerCollider.size = ColliderPSizeCrouch;
                PlayerCollider.offset = ColliderPOffsetCrouch;

            } else if (inputY >= 0){

                anim.SetBool("IsCrouching", false);

                PlayerCollider.size = ColliderPSize;
                PlayerCollider.offset = ColliderPOffset;    

            }

            //Animacion de salto

            if (NoJumping() == true){

                anim.SetBool("IsJumping", false);

            } else {

                anim.SetBool("IsJumping", true);

            }

            //Disparo

            if (Input.GetMouseButtonDown(0) && ShotInCoolDown == false){

                anim.SetBool("IsShooting", true);

                audioSrc.PlayOneShot(sndShot);

                if (inputY < 0){

                    Instantiate(shot, new Vector3(transform.position.x, transform.position.y + 0.87f, 0), Quaternion.identity);

                } else if (inputY >= 0){

                    Instantiate(shot, new Vector3(transform.position.x, transform.position.y + 1.7f, 0), Quaternion.identity);

                }

                ShotInCoolDown = true;

                Invoke("ShotCollDownEnd",0.2f);

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

            if (contador == NeededGems){

                txtWin.SetActive(true);

                win = true;

                Invoke("GoToCredits", 3);

            }

        }

        //Item de doble salto

        if (other.CompareTag("DJump")){

            Destroy(other.gameObject);

            audioSrc.PlayOneShot(sndItem);

            DJump = true;

            txtDJump.SetActive(true);

            Invoke("DJumpHide", 4);

        }

        //Item de invencibilidad

        if (other.CompareTag("Invencible")){

            Destroy(other.gameObject);

            audioSrc.PlayOneShot(sndItem);

            becomeInvincible();

        }

        //Entra en el agua

        if (other.CompareTag("Water")){

            AirMeter.SetActive(true);

            AirMeterAnim.SetBool("PlayerInWater", true);

            AirAmount = 8;

        }

        //Escaleras

        if (other.CompareTag("Lader")){

            GameManager.onLader = true;
            rb.gravityScale = 0;
            rb.linearDamping = 20;

        }

    }

    //Agua

    void OnTriggerStay2D(Collider2D other){

        if (other.gameObject.tag == "Water"){

            JumpCount = 1;

            Jump = 4;

            MoveSpeed = 3;

            if (AirInCoolDown == false){

                Invoke("AirTimer", 1);

                AirInCoolDown = true;

            }

        }

    }

    void OnTriggerExit2D(Collider2D other){

        //Agua

        if (other.gameObject.tag == "Water"){

            AirMeter.SetActive(false);

            AirMeterAnim.SetBool("PlayerInWater", false);

            Jump = 8;

            MoveSpeed = 6;

        }

        //Escaleras

        if (other.CompareTag("Lader")){

            GameManager.onLader = false;
            rb.gravityScale = 1;
            rb.linearDamping = 0;

        }

    }

    //Se ejecuta cuando el medidior de aire se vacia y el jugador sigue en el agua

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

        if (level1F == false){

            SceneManager.LoadScene("Level_2");

            level1F = true;

        } else if (level1F == true) {

            SceneManager.LoadScene("Credits");

        }

    }

    //textos de habilidades

    void DJumpHide(){

        txtDJump.SetActive(false);

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

    //Enfriamiento del disparo

    void ShotCollDownEnd(){

        ShotInCoolDown = false;

    }

    //Controla el tiempo en el agua

    void AirTimer(){

        if (AirAmount == 0){

            hurt();

            AirAmount = 8;

            AirInCoolDown = false;

        } else {

            AirAmount --;

            AirInCoolDown = false;

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

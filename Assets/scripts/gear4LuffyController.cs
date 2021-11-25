using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gear4LuffyController : MonoBehaviour
{
    gear4LuffyController(){
        instance=this;
    }
    public float velocidad = 10;
    private const int ANIMATION_QUIETO = 0;
    private const int ANIMATION_VOLAR = 1;
    private const int ANIMATION_PUÑO = 3;
    private const int ANIMATION_CAE= 2;
 
    private float vidaFrezer, VidaActualFrezer=1;
    private float EnergiaLuffy,EnergiaActualLuffy=0.2f;
    public static gear4LuffyController instance;
    private SpriteRenderer spriteRenderer;
    public luffyController luffyNormal;
    public FrezerController frezer;
    private Animator animator;
    public AudioSource SonidoElephantGun;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        frezer=FindObjectOfType<FrezerController>();
       
       
    }
      
    // Update is called once per frame
    bool EstaAtacando = false;

    void Update()
    {
            
         if (Input.GetKey(KeyCode.D))//Si presiono la tecla rigtharrow voy a ir hacia la derecha
            {
                estaCayendo=false;
                rb.velocity = new Vector2(velocidad, rb.velocity.y);//velocidad de mi objeto
                CambiarAnimacion(ANIMATION_VOLAR);//Accion correr 
                spriteRenderer.flipX = false;//Que mi objeto mire hacia la derecha
                
              
            }
            
            else if (Input.GetKey(KeyCode.A))
            {
                estaCayendo=false;
                rb.velocity = new Vector2(-velocidad, rb.velocity.y);
                CambiarAnimacion(ANIMATION_VOLAR);
                spriteRenderer.flipX = true;
            
           
            }else if(Input.GetKey(KeyCode.W)){

            }
            else if (Input.GetKey(KeyCode.F))
            {
                EstaAtacando = true;
                SonidoElephantGun.Play();
               CambiarAnimacion(ANIMATION_PUÑO);
               Invoke("puño",3f);
               Invoke("isAttack",5.1f);

              
            }else if (Input.GetKey(KeyCode.O))
            {
               CambiarAnimacion(ANIMATION_CAE);
            } else if(!estaCayendo&&!EstaAtacando)            
            {
                CambiarAnimacion(ANIMATION_QUIETO);//Metodo donde mi objeto se va a quedar quieto
                rb.velocity = new Vector2(0, rb.velocity.y);//Dar velocidad a nuestro objeto
            }else if(EstaMuerto){
                CambiarAnimacion(ANIMATION_CAE);
                Invoke("DestruccionPj",1f);
            }
    }
    bool EstaMuerto= false;
    public void Muere(){
        EstaMuerto= true;
    }
    void DestruccionPj(){
        Destroy(this.gameObject);
    }
    public void isAttack(){
        EstaAtacando=false;  
    }
    private void puño(){
        empuje();
        
    }
    bool estaCayendo = false;
  
    void OnCollisionEnter2D(Collision2D other)
    {
       // EstaSaltando = false;
        if (other.gameObject.tag == "supernova"&&EstaAtacando==true)
        {
            Destroy(other.gameObject);
       
        }else if (other.gameObject.tag == "supernova"&&EstaAtacando==false)
        {
            estaCayendo=true;
            frezer.DañoRecibeLuffy(0.2f);
           
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
            Invoke("IsNotCae",1f);
            Destroy(other.gameObject); 
        }else if (other.gameObject.tag == "RayoMortal"&&EstaAtacando==true)
        {
            Destroy(other.gameObject);
          
        }else if (other.gameObject.tag == "RayoMortal"&&EstaAtacando==false)
        {
             
            estaCayendo=true;
            frezer.DañoRecibeLuffy(0.1f);
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
            Invoke("IsNotCae",1f);
            Destroy(other.gameObject);
            
          
        }else if (other.gameObject.tag == "RayoCuerpoxcuerpo"&&EstaAtacando==true)
        {   
            Destroy(other.gameObject);
           
        }else if (other.gameObject.tag == "RayoCuerpoxcuerpo"||other.gameObject.tag == "puas"&&EstaAtacando==false)
        {
             
            estaCayendo=true;
            frezer.DañoRecibeLuffy(0.1f);
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
            Invoke("IsNotCae",1f);
            Destroy(other.gameObject);
            
          
        }else if(other.gameObject.tag == "Frezer"&&EstaAtacando==true)
        {
            frezer.DañoRecibeFrezer(0.1f);
        } 
      
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Frezer"&&frezer.CuerxCuerpoEstatus())
        {
            frezer.DañoLufCxC();
              
            //Invoke("DañoLuffyCuerpoxCuerpo",0.5f);
        }
    }
    void IsNotCae(){
        estaCayendo=false;
    }
    void empuje(){
        if (spriteRenderer.flipX)
        { rb.velocity = new Vector2(5, rb.velocity.y);
        }
        else
        { rb.velocity = new Vector2(-5, rb.velocity.y);
        }
    }
    void empujeadelante(){
        if (spriteRenderer.flipX)
        {
            rb.velocity = new Vector2(-8, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(8, rb.velocity.y);
        }
    } 
      private void CambiarAnimacion(int animacion)
    {
        animator.SetInteger("Estado", animacion);
    }
}

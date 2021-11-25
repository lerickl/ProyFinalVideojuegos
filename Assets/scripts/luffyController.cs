using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class luffyController : MonoBehaviour
{
    private float vidaLuffy, VidaActualLuffy=1;
     
    public AudioSource sonidoTransformacionGear4;
    public AudioSource SonidoRedHawk;
    public AudioSource SonidoElephantGun;
    private float vidaFrezer, VidaActualFrezer=1;
   
    public float velocidad = 5;
    private const int ANIMATION_QUIETO = 0;
    private const int ANIMATION_CORRER = 1;
    private const int ANIMATION_PUÑO = 3;
    private const int ANIMATION_REDHAWK= 2;
    private const int ANIMATION_CAE = 4;
    private const int ANIMATION_SALTAR=5;
    private const int ANIMATION_PUÑO_3MARCHA=6;
    private const int ANIMATION_TRANSFORMACION=10;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private bool EstaSaltando = false;
    public float fuerzaSalto = 10;
    public BoxCollider2D plataform;
    public GameObject nubeInvok;
    public static luffyController instance;
    public FrezerController frezer;
    public bool EstaAtacando=false;
    public bool estaTransformandose=false;
    public bool estaCayendo=false;
    //datos para manejar los ataques con las coliciones
    public bool animacionCaer=false;
    public GameObject Frezer;
    private void Awake(){
        if(instance==null){
            instance=this;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        frezer=FindObjectOfType<FrezerController>();
        
       
    }
  
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.T)){
            //Frezer()
            frezer.testdata();
        }
            
        if (Input.GetKey(KeyCode.W) && !EstaSaltando)
            {
                CambiarAnimacion(ANIMATION_SALTAR);
                Saltar();
                EstaSaltando = true;
            }
            else if (Input.GetKey(KeyCode.D))//Si presiono la tecla rigtharrow voy a ir hacia la derecha
            {
                estaCayendo=false;
                rb.velocity = new Vector2(velocidad, rb.velocity.y);//velocidad de mi objeto
                CambiarAnimacion(ANIMATION_CORRER);//Accion correr 
                spriteRenderer.flipX = false;//Que mi objeto mire hacia la derecha
                
                if (Input.GetKey(KeyCode.W) && !EstaSaltando)
                {
                    CambiarAnimacion(ANIMATION_SALTAR);
                    Saltar();
                    EstaSaltando = true;
                }
            }
            
            else if (Input.GetKey(KeyCode.A))
            {
                estaCayendo=false;
                rb.velocity = new Vector2(-velocidad, rb.velocity.y);
                CambiarAnimacion(ANIMATION_CORRER);
                spriteRenderer.flipX = true;
                /*if(Input.GetKey(KeyCode.C))
                {
                    
                    CambiarAnimacion(ANIMATION_SLIDE);
                }
            */if (Input.GetKey(KeyCode.W) && !EstaSaltando)
                {
                    CambiarAnimacion(ANIMATION_SALTAR);
                    Saltar();
                    EstaSaltando = true;
                  
                }
            }else if (Input.GetKey(KeyCode.F))
            {
                 EstaAtacando=true;
               CambiarAnimacion(ANIMATION_PUÑO);
               Invoke("isAttack",0.5f);
            }else if (Input.GetKey(KeyCode.C))
            {
                EstaAtacando=true;
                SonidoRedHawk.Play();
                CambiarAnimacion(ANIMATION_REDHAWK);
                Invoke("isAttackEmpuje",2f);
                Invoke("isAttack",3.8f);

            }else if (Input.GetKey(KeyCode.R))
            {
                EstaAtacando=true;
               SonidoElephantGun.Play();
               CambiarAnimacion(ANIMATION_PUÑO_3MARCHA);
               Invoke("isAttackEmpuje",3f);
                Invoke("isAttack",3.8f);
            } else if(EstaSaltando==false&&EstaAtacando==false&&estaTransformandose==false&&estaCayendo==false)            
            {
                CambiarAnimacion(ANIMATION_QUIETO);//Metodo donde mi objeto se va a quedar quieto
                rb.velocity = new Vector2(0, rb.velocity.y);//Dar velocidad a nuestro objeto
            }
             if(VidaActualLuffy<=0){
                 CambiarAnimacion(ANIMATION_CAE);
                Invoke("luffyMuere",1.5f);
            }
             if(VidaActualFrezer<=0){
                  
                Invoke("FrezerMuere",1.5f);
            }
           
            if(Input.GetKey(KeyCode.Q)){
                frezer.TiempoTransformacionGear4luffy();
                estaTransformandose=true;
               
                sonidoTransformacionGear4.Play();
                CambiarAnimacion(ANIMATION_TRANSFORMACION);
                frezer.TiempoTransformacionGear4luffy();
                Invoke("transformacion",5f);
                Destroy(gameObject, 5.01f);
            //time count up//
           
                
                 
            }
            if(Input.GetKeyUp(KeyCode.Q)){
                 
                
                
            }
           // Debug.Log(VidaActualLuffy);
           // Debug.Log(EnergiaActualLuffy);
            
    }
    public void isAttackEmpuje(){
        empujeadelante();
          
    }
    private int Gear4=1;
    public void transformacion (){

        if(Gear4==1){
            var position = new Vector3(transform.position.x , transform.position.y , transform.position.z-1);
            Instantiate(nubeInvok, position, nubeInvok.transform.rotation);
            Gear4=0;
        }

    }
     
    public void luffyMuere(){
        Destroy(this.gameObject);
    }
    public void isAttack(){
        EstaAtacando=false;  
    }
    private void CambiarAnimacion(int animacion)
    {
        animator.SetInteger("Estado", animacion);
    }
      private void Saltar()
    {
        CambiarAnimacion(ANIMATION_SALTAR);
        rb.velocity = Vector2.up * fuerzaSalto;//Vector 2.up es para que salte hacia arriba
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
     void OnCollisionEnter2D(Collision2D other)
    {
        EstaSaltando = false;
        if (other.gameObject.tag == "supernova"&&EstaAtacando==true)
        {
            Destroy(other.gameObject);
       
        }else if (other.gameObject.tag == "supernova"&&EstaAtacando==false)
        {
            estaCayendo=true;
            frezer.DañoRecibeLuffy(0.2f);
           
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
            Destroy(other.gameObject);
            //Destroy(this.gameObject);
            //PersonajeController.IncrementerPuntajeEn10();
        }else if (other.gameObject.tag == "RayoMortal"&&EstaAtacando==true)
        {
            Destroy(other.gameObject);
          
        }else if (other.gameObject.tag == "RayoMortal"&&EstaAtacando==false)
        {
             
            estaCayendo=true;
            frezer.DañoRecibeLuffy(0.1f);
            CambiarAnimacion(ANIMATION_CAE);
            Invoke("empuje",1f/5);
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

//aqui empieza 
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Image = UnityEngine.UI.Image;
using Random = System.Random;
    

public class MovimientosDoffy : MonoBehaviour
{
    public float velocityX = 4;
    
    public Image barraSaludLuffyimg;
    private Image barraSaludFrezerImg;
    public Image barraEnergiaLuffyimg;
    public Image barraVidaEnemyimg;
    private float vidaLuffy, VidaActualLuffy=1;    
    private float vidaFrezer, VidaActualFrezer=1;
    private float EnergiaLuffy,EnergiaActualLuffy=0.2f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject ataquePiso , ataquePisoIzquierda;
    private int LanzarAtaquePiso=2;
  
    
    public AudioSource ataqueSable;
    public AudioSource sonidoPuas;
    public AudioSource ataqueCerca;
    public AudioSource ataquePatada;
 
    private const int animacion_idle = 0;
    private const int animacion_walk = 1;
    private const int animacion_run = 2;
    private const int animacion_ataque_araniaso = 3;
    private const int animacion_ataque_patada = 7;
    private const int animacion_ataque_piso = 4;
   
    private const int animacion_ataque_sable =6 ;
    private const int animacion_ataque_puas = 5;

    // public const int LUFFYCHOCA = 8;
    // public const int PISO = 9;

    private int contadorAtaques = 0;
    public bool EstaAtacando= false;
    public string DirJugador;
     private luffyController luffy;
    private Transform[] transforms;
    
    private float tiempoDetectar=4, cuentaBajo;
    private float tiempoTeleport=3, cuentaBajoTeleport;
     private gear4LuffyController gear4Luffy;
    
   
    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       spriteRenderer = GetComponent<SpriteRenderer>();
       luffy= luffyController.instance;
       cuentaBajo=tiempoDetectar;
       cuentaBajoTeleport=tiempoTeleport;
        ubicarPlayer();
    
    }

    // Update is called once per frame
    void Update()
    {
       
       if(luffyController.instance==false && gear4LuffyController.instance==false){
            //g4lufy=true;
            gear4Luffy=gear4LuffyController.instance;
            Debug.Log("luffy 4 marcha instanciado");
            ubicarPlayer();
        }
        if(EstaAtacando==false){
            CambiarAnimacion(0);
        }

        contador();
      

    }

    

    public void telepor(){
        var initialPosition= UnityEngine.Random.Range(0, transforms.Length);
        transform.position = transforms[initialPosition].position;
         cuentaBajo = tiempoDetectar;
         cuentaBajoTeleport =tiempoTeleport;
    }
    
   
    
    public void ubicarPlayer(){
 
        if(luffyController.instance==true)
        {
            if (transform.position.x> luffyController.instance.transform.position.x)
            {
                //transform.localScale= new Vector3(0.6706f,0.6034f,1);
                Debug.Log("esta a la izquierda");
                DirJugador="izquierda";
                 spriteRenderer.flipX=true;
                
            
            }else
            {
                // transform.localScale= new Vector3(0.6706f,0.6034f,1);
                Debug.Log("esta a la derecha");
                    DirJugador="derecha";
                    spriteRenderer.flipX=false;
                
            }
        }else if(gear4LuffyController.instance==true){
            if (transform.position.x> gear4LuffyController.instance.transform.position.x)
            {
                 transform.localScale= new Vector3(0.6706f,0.6034f,1);
                Debug.Log("Luffy G4 esta a la izquierda");
                DirJugador="izquierda";

                   spriteRenderer.flipX=true;
            
            }else
            {
                transform.localScale= new Vector3(0.6706f,0.6034f,1);
                Debug.Log("Luffy G4 esta a la derecha");
                    DirJugador="derecha";
                      spriteRenderer.flipX=false;
                
            }
        }
 
    }
    
   
    public void AtaqueDoflamingo()
    {
         var distancia=0.0f;
        if(luffyController.instance==true){
            distancia=transform.position.x-luffyController.instance.transform.position.x;
            Debug.Log("luffy normal test distancias:"+distancia);
        }else if(gear4LuffyController.instance==true){
            distancia=transform.position.x-gear4LuffyController.instance.transform.position.x;
        }
        if(distancia>15){
          
            EstaAtacando=true;
           
            if(EstaAtacando==true)
            {
           
                CambiarAnimacion(animacion_ataque_piso);
                
          
                Invoke("AtaquePiso", 0.01f);
                Invoke("isAttack",1.0f); 
                sonidoPuas.PlayOneShot(sonidoPuas.clip,1f/2);
            }

        }
        
        if(distancia > 10 && distancia< 15)
        {
            EstaAtacando=true;

            if (EstaAtacando == true)
            {
                CambiarAnimacion(animacion_ataque_patada);
                ataquePatada.PlayOneShot(ataquePatada.clip, 1f/2);
                Invoke("AtaquePatada", 0.5f);
                Invoke("isAttack",1.0f); 
                
            }
        }

        if (distancia >6 && distancia <10)
        {
            EstaAtacando = true;
            if (EstaAtacando == true)
            {
                TPtransporte();
               CambiarAnimacion(animacion_ataque_sable);
               ataqueSable.PlayOneShot(ataqueSable.clip,1f/2);
               Invoke("AtaqueSable",1.0f);
               Invoke("isAttack",1.0f); 
             //  TPtransporte();
            }
        }

        if (distancia >= 5 && distancia <=6)
        {
            EstaAtacando = true;
            if (EstaAtacando == true)
            {
                CambiarAnimacion(animacion_ataque_puas);
                ataqueCerca.PlayOneShot(ataqueCerca.clip,1f/2);
                Invoke("isAttack",1.0f); 
            }
        }

        if (distancia > 0 && distancia < 4)
        {
            EstaAtacando = true;
            if (EstaAtacando == true)
            {
                CambiarAnimacion(animacion_ataque_araniaso);
                Invoke("AtaqueAraniaso", 1.0f/2);
                Invoke("isAttack",1.5f);
            }
        }
        
        
        if(distancia<0&&distancia>-4){
            EstaAtacando=true;
            if(EstaAtacando==true){
              
                CambiarAnimacion(animacion_ataque_araniaso);
                
                Invoke("AtaqueAraniaso", 1.0f/2);
                Invoke("isAttack",1.5f); 
                
            }
        }
        
        if(distancia <= -5 && distancia >= -6)
        {
            EstaAtacando = true;
            if (EstaAtacando == true)
            {
                CambiarAnimacion(animacion_ataque_puas);
                ataqueCerca.PlayOneShot(ataqueCerca.clip,1f/2);
                Invoke("isAttack",1.0f); 
            }
        }
        
        if (distancia < -6 && distancia >- 10)
        {
            EstaAtacando = true;
            if (EstaAtacando == true)
            {
                TPtransporte();
                CambiarAnimacion(animacion_ataque_sable);
                ataqueSable.PlayOneShot(ataqueSable.clip,1f/2);
                Invoke("AtaqueSable",1.0f);
                Invoke("isAttack",1.0f); 
                //  TPtransporte();
            }
        }
        
        
        if(distancia < -10 && distancia> -15)
        {
            EstaAtacando=true;

            if (EstaAtacando == true)
            {
                CambiarAnimacion(animacion_ataque_patada);
                ataquePatada.PlayOneShot(ataquePatada.clip, 1f/2);
                Invoke("AtaquePatada", 0.5f);
                Invoke("isAttack",1.0f); 
                
            }
        }
        
        if(distancia< -15){
          
            EstaAtacando=true;
           
            if(EstaAtacando==true)
            {
           
                CambiarAnimacion(animacion_ataque_piso);
                
                // Invoke("animacion_ataque_piso", 0.1f);
                Invoke("AtaquePiso", 0.01f);
                Invoke("isAttack",1.0f); 
                sonidoPuas.PlayOneShot(sonidoPuas.clip,1f/2);
            }

        }
        
    }
    
    
    
    public void isAttack(){
        EstaAtacando=false;  
    }
    
    
    public void contador()
    {
        cuentaBajo-=Time.deltaTime;
        cuentaBajoTeleport-=Time.deltaTime;
        if (cuentaBajo<=0f)
        {
            AtaqueDoflamingo();
            ubicarPlayer();
            cuentaBajo=tiempoDetectar;
          

        }
        if (cuentaBajoTeleport<=0f)
        {
             
            ubicarPlayer();
            cuentaBajoTeleport=tiempoTeleport;
 
        }
    }
    

    private void CambiarAnimacion(int animacion)
    {
       animator.SetInteger("Estado", animacion);
        
    }
    
    public void TPtransporte(){
    
        if (spriteRenderer.flipX&&DirJugador=="izquierda")
        {

            rb.velocity = new Vector2(-6, rb.velocity.y);
        }
        else
        {
                    
            rb.velocity = new Vector2(6, rb.velocity.y);
        }
    }
    
    public void AtaquePiso()
    {
        if(luffyController.instance==true)
        {
              if (spriteRenderer.flipX && DirJugador == "izquierda")
                {
                    
                    var position = new Vector2( luffyController.instance.transform.position.x , luffyController.instance.transform.position.y);
                    Instantiate(ataquePiso, position, ataquePiso.transform.rotation);  
                }
                else
                {
                    var position = new Vector2(luffyController.instance.transform.position.x  , luffyController.instance.transform.position.y);
                    
                    Instantiate(ataquePiso ,position, ataquePiso.transform.rotation );  
                }
        }else if(gear4LuffyController.instance==true)
        {
            if (spriteRenderer.flipX && DirJugador == "izquierda")
            {
            
                var position = new Vector2( gear4LuffyController.instance.transform.position.x , gear4LuffyController.instance.transform.position.y);
                Instantiate(ataquePiso, position, ataquePiso.transform.rotation);  
            }
            else
            {
                var position = new Vector2(gear4LuffyController.instance.transform.position.x  , gear4LuffyController.instance.transform.position.y);
            
                Instantiate(ataquePiso ,position, ataquePiso.transform.rotation );  
            }
        }
        

    }

    public void AtaqueSable()
    {
        if(luffyController.instance==true)
        {
              if (spriteRenderer.flipX&&DirJugador=="izquierda")
                {
                    CambiarAnimacion(animacion_walk);
                    rb.velocity = new Vector2(-5, rb.velocity.y);

                }
                else
                {
                    CambiarAnimacion(animacion_walk);
                    rb.velocity = new Vector2(5, rb.velocity.y);
                            
                }
        }else if(gear4LuffyController.instance==true){
            if (spriteRenderer.flipX&&DirJugador=="izquierda")
            {
                    CambiarAnimacion(animacion_walk);
                rb.velocity = new Vector2(-5, rb.velocity.y);

            }
            else
            {
                CambiarAnimacion(animacion_walk);
                rb.velocity = new Vector2(5, rb.velocity.y);
                        
            }
        }

        
    }


    public void AtaquePatada()
    {
        if (spriteRenderer.flipX && DirJugador == "izquierda")
        {
            rb.velocity = new Vector2(-6, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(6, rb.velocity.y);
        }
    }
   
    
    //Area de Vida//
    public void DañoLufCxC(){
        Invoke("DañoLuffyCuerpoxCuerpo",0.05f);
    }
     void DañoLuffyCuerpoxCuerpo()
    {
        VidaActualLuffy = VidaActualLuffy-0.001f;
        barraSaludLuffyimg.fillAmount = VidaActualLuffy ;
    }
    public void DañoRecibeLuffy(float daño)
    {
    
        VidaActualLuffy = VidaActualLuffy-daño;
        barraSaludLuffyimg.fillAmount = VidaActualLuffy ;
    }
    public void DañoRecibeFrezer(float daño)
    {
        //vidaFrezer=GetComponent<FrezerController>
        
        VidaActualFrezer -= daño;
        barraVidaEnemyimg.fillAmount = VidaActualFrezer ;
    }
    

    public void AtaqueAraniaso()
    {
        if (spriteRenderer.flipX&&DirJugador=="izquierda")
        {
 
            rb.velocity = new Vector2(-5, rb.velocity.y);
        }
        else
        {
                       
            rb.velocity = new Vector2(5, rb.velocity.y);
        }
    }
    

   
    
}


    
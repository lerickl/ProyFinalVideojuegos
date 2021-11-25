using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrezerController : MonoBehaviour
{
    //variables de vida
    public Image barraSaludLuffyimg;
    private Image barraSaludFrezerImg;
    public Image barraEnergiaLuffyimg;
    public Image barraVidaEnemyimg;
    private float vidaLuffy, VidaActualLuffy=1;    
    private float vidaFrezer, VidaActualFrezer=1;
    private float EnergiaLuffy,EnergiaActualLuffy=0.2f;
    private int velocidadmovimiento;
    //variables obj//
    private Rigidbody2D rb;
    private Vector2 DireccionAttack;
    private luffyController luffy;
    public GameObject luffyOject;
    public GameObject gear4Object;
    //controladores //
    public gear4LuffyController gear4Luffy;
    public static FrezerController instance;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public AudioSource sonidoCuerpoxCuerpo;
    public AudioSource sonidotransporte;
    private Transform[] transforms;
    private float tiempoDetectar=3, cuentaBajo;
    private float tiempoTeleport=3, cuentaBajoTeleport;
    
    private int EstadoNormal=0;
    private int LanzarSupernova=1;
    private int LanzarRayoMortal=2;
    private int Rayo_cuerpoxcuerpo=3;
    private int cuerpoxcuerpo=4;
    private int transporte=5;


    //area de objetos de ataque//
    
    public GameObject Rayos_cuerpoxcuerpo_derecha, Rayos_cuerpoxcuerpo_izquierda;

    public GameObject superN;
    public GameObject superND;
    public GameObject RayoMortalO;
    public GameObject RayoMortalOD;
    public bool EstaAtacando=false;
    public bool EstaAtacandoCuerpoxCuerpo=false;
    public int cantSupernovas=1;
    
    // Start is called before the first frame update
    public string DirJugador;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //sonidoCuerpoxCuerpo= GetComponent<AudioSource>();
        
        //luffyController=FindObjectOfType<luffyController>();
        
        luffy= luffyController.instance;
        ubicarPlayer();
 
    }
    public void testdata(){
        Debug.Log("si funciona pasar datos de un controlador a otro");
    }
    bool g4lufy=false;
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
        var temp=1;
        if(VidaActualLuffy<=0&&temp==1){
            temp=0;
            Destroy(luffyOject.gameObject);
            Destroy(gear4Object.gameObject);
             
            
        }
         if(VidaActualFrezer<=0&&temp==1){
            Destroy(this.gameObject);
            
        }
        contador();
       
        
    }
    public bool CuerxCuerpoEstatus(){
        return EstaAtacandoCuerpoxCuerpo;
    }
    public void TiempoTransformacionGear4luffy(){
        cuentaBajo=12;
    }
    public void FrezerAttack(){
        var distancia=0.0f;
        if(luffyController.instance==true){
            distancia=transform.position.x-luffyController.instance.transform.position.x;
            Debug.Log("luffy normal test distancias:"+distancia);
        }else if(gear4LuffyController.instance==true){
            distancia=transform.position.x-gear4LuffyController.instance.transform.position.x;
        }
        
        if(distancia>0&&distancia<4){
            EstaAtacando=true;
            if(EstaAtacando==true){
               EstaAtacandoCuerpoxCuerpo=true;
               sonidoCuerpoxCuerpo.PlayOneShot(sonidoCuerpoxCuerpo.clip, 1f/2);
               CambiarAnimacion(cuerpoxcuerpo);
               
               Invoke("CuerpoxCuerpo",0.0f);
               Invoke("isAttack",1.5f); 
               Invoke("OFFAttackCuerpoxCuerpo", 1.5f);
               
           }
            }
        if(distancia>4&&distancia<6){
            EstaAtacando=true;
            if(EstaAtacando==true){
              CambiarAnimacion(Rayo_cuerpoxcuerpo);
               Invoke("RayoCuerpoxCuerpo", 1.0f/2);
               Invoke("isAttack",1.5f); 
           }

            }
      
        if(distancia>6&&distancia<8){
            EstaAtacando=true;
           if(EstaAtacando==true){
                CambiarAnimacion(LanzarRayoMortal);
                Invoke("RayoMortal", 1.0f/2);
                Invoke("isAttack",1.5f); 
            
           }
           
        }
        if(distancia>6&&distancia<8){
            EstaAtacando=true;
            CambiarAnimacion(LanzarRayoMortal);
            Invoke("RayoMortal", 1.0f/2);
            Invoke("isAttack",1.5f);
           
        }
        if(distancia>8&&distancia<12){
            EstaAtacando=true;             
            CambiarAnimacion(LanzarSupernova);             
            Invoke("supernova", 1.0f/2);
            Invoke("isAttack",1f); 
 
        }
        if(distancia>12){
            EstaAtacando=true;
            sonidotransporte.PlayOneShot(sonidotransporte.clip, 1f/2);
            CambiarAnimacion(transporte);
            TPtransporte();
            Invoke("isAttack",1f/2);
        }
         
        if(distancia<0&&distancia>-4){
            EstaAtacando=true;
            sonidoCuerpoxCuerpo.PlayOneShot(sonidoCuerpoxCuerpo.clip, 1f/2);
            CambiarAnimacion(cuerpoxcuerpo);
            
            Invoke("CuerpoxCuerpo", 1.0f/2);
            Invoke("isAttack",1.5f); 
        }
        if(distancia<-4&&distancia>-6){
            EstaAtacando=true;
            CambiarAnimacion(Rayo_cuerpoxcuerpo);
            Invoke("RayoCuerpoxCuerpo", 1.0f/2);
            Invoke("isAttack",1.5f); 

            }
      
        if(distancia<-6&&distancia>-10){
            EstaAtacando=true;
            CambiarAnimacion(LanzarRayoMortal);
            Invoke("RayoMortal", 1.0f/2);
            Invoke("isAttack",1.5f); 
           cantSupernovas=1;
        }
        if(distancia<-10&&distancia>-15){
            EstaAtacando=true;
                CambiarAnimacion(LanzarSupernova);
            
            Invoke("supernova", 1.0f/2);
            Invoke("isAttack",1f);
            cantSupernovas=1;
        }
        if(distancia<-15){
            EstaAtacando=true;
            sonidotransporte.PlayOneShot(sonidotransporte.clip, 1f/2);
            CambiarAnimacion(transporte);
            TPtransporte();
            Invoke("isAttack",1f/2);
        
        }
    }
  
    public void isAttack(){
        EstaAtacando=false;  
    }
    public void OFFAttackCuerpoxCuerpo(){
        EstaAtacando=false;  
    }
    private float countback=2;

   
    public void contador(){
        
        cuentaBajo-=Time.deltaTime;
        cuentaBajoTeleport-=Time.deltaTime;
        if (cuentaBajo<=0f)
        {
            FrezerAttack();
            ubicarPlayer();
            cuentaBajo=tiempoDetectar;
                cantSupernovas--;

        }
       
    }
    private void CambiarAnimacion(int animacion)
    {
        animator.SetInteger("Estado", animacion);
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
    public void telepor(){
        var initialPosition= UnityEngine.Random.Range(0, transforms.Length);
        transform.position = transforms[initialPosition].position;
        cuentaBajo = tiempoDetectar;
        cuentaBajoTeleport =tiempoTeleport;
    }
    public void RayoMortal(){
       
        if (spriteRenderer.flipX&&DirJugador=="izquierda")
            {
               // DireccionAttack =(luffy.transform.position- transform.position).normalized*velocidadmovimiento;
                var position = new Vector2(transform.position.x -8f , transform.position.y  );
                Instantiate(RayoMortalO, position, RayoMortalO.transform.rotation);
          
            }
            else
            { 
                 //DireccionAttack =(luffy.transform.position- transform.position).normalized*velocidadmovimiento;
                var position = new Vector2(transform.position.x +8f , transform.position.y );
                Instantiate(RayoMortalOD, position, RayoMortalOD.transform.rotation);
              
            }
          
     
    }
    public void supernova(){
        
          if (spriteRenderer.flipX&&DirJugador=="izquierda")
                    {

                        var position = new Vector2(transform.position.x- 2.5f, transform.position.y + .5f);
                        Instantiate(superN, position, superN.transform.rotation);
                    }
                    else
                    {
                        var position = new Vector2(transform.position.x + 2.5f, transform.position.y + .5f);
                        Instantiate(superND, position, superND.transform.rotation);
                    }
    }
     public void RayoCuerpoxCuerpo(){
        
          if (spriteRenderer.flipX&&DirJugador=="izquierda")
                    {

                        var position = new Vector3(transform.position.x- 2.5f, transform.position.y + .5f,transform.position.z-1);
                        Instantiate(Rayos_cuerpoxcuerpo_izquierda, position, superN.transform.rotation);
                      
                    }
                    else
                    {
                        var position = new Vector3(transform.position.x + 2.5f, transform.position.y + .5f,transform.position.z-1);
                        Instantiate(Rayos_cuerpoxcuerpo_derecha, position, superND.transform.rotation);
                      
                    }
    }
     public void CuerpoxCuerpo(){
        
          if (spriteRenderer.flipX&&DirJugador=="izquierda")
                    {
 
                        rb.velocity = new Vector2(-5, rb.velocity.y);
                    }
                    else
                    {
                       
                        rb.velocity = new Vector2(5, rb.velocity.y);
                    }
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
    
     
}

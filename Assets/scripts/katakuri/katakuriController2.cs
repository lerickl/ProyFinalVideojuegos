using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class katakuriController2 : MonoBehaviour
{

    //---------------------
    // public Text frase;

    public AudioSource sonidoPelea;
    public AudioSource sonidoPuños;
    public AudioSource sonidoPuño;
    public GameObject brazo;
    public GameObject puños;
    public GameObject luffy_;
     

    private float vidaLuffy, vidaLuffyActual = 1;
    private float vidaKatakuri, vidaKatakuriActual = 1;
    private float tiempoDetectar = 4,cuentaBajo;
    private float tiempoTeleport = 3, cuentaBajoTeleport;
    private luffyController luffy;
    private gear4LuffyController gear4Luffy;
    private float tiempoAtacar=0;
    private int contador = 0;

    private const int ANIMATION_QUIETO = 0;
    private const int ANIMATION_LANZA = 1;
    private const int ANIMATION_PISOTON = 2;
    private const int ANIMATION_ATTACKDISTANCIA= 3;
    private const int ANIMATION_PUÑO = 4;
    private const int ANIMATION_CIEN_PIES=8;
   private const int ANIMATION_DISTANCIA_PUÑOS=9;
    //private const int ANIMATION_ATAQUE_DISTANCIA=10;

 
    public Image barraSaludKatakuriImg;
    public Image barraSaludLuffy;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

 
    private bool estagolpeando=false;
    private bool habilitar_golpe=false;
    private bool habilitar_puños = false;
    public string DirecJugador;
    public int cantGolpe = 1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        luffy = luffyController.instance;
        cuentaBajo = tiempoDetectar;
        cuentaBajoTeleport = tiempoTeleport;
        ubicarPlayer();
        //audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(luffyController.instance==false && gear4LuffyController.instance==false){
            //g4lufy=true;
            gear4Luffy=gear4LuffyController.instance;
            Debug.Log("luffy 4 marcha instanciado");
            ubicarPlayer();
        }
        if (estagolpeando==false)
        {
            CambiarAnimacion(ANIMATION_QUIETO);
        }
        contar_ataque();

    }

    private void contar_ataque()
    {
        cuentaBajo -= Time.deltaTime;
        cuentaBajoTeleport -= Time.deltaTime;

        if (cuentaBajo<=0f)
        {
           KaatakuriAtaca();
            ubicarPlayer();
            cuentaBajo = tiempoDetectar;
           
        }
        if (cuentaBajoTeleport<=0f)
        {
            ubicarPlayer();
            cuentaBajoTeleport = tiempoTeleport;
        }
    }

    public void DañoRecibeKatakuri()
    {
        vidaKatakuri = 0.2f;
        vidaKatakuriActual = vidaKatakuriActual - vidaKatakuri;
        barraSaludKatakuriImg.fillAmount = vidaKatakuriActual;
    }

    public void DañoRecibeLuffy()
    {
        
            vidaLuffy = 0.2f;
            vidaLuffyActual = vidaLuffyActual - vidaLuffy;
            barraSaludLuffy.fillAmount = vidaLuffyActual;
    }

    private void muerte_luffy()
    {
        if (vidaLuffyActual <=0)
        {
            Destroy(luffy_);
        }
    }

    IEnumerator Esperar(){
        yield return new WaitForSecondsRealtime((3/2));
    }
    private void CambiarAnimacion(int animacion)
    {
        animator.SetInteger("Estado", animacion);
    }

    public void KaatakuriAtaca()
    {
        var distancia=0.0f;
        if(luffyController.instance==true){
            distancia=transform.position.x-luffyController.instance.transform.position.x;
            Debug.Log("luffy normal test distancias:"+distancia);
        }else if(gear4LuffyController.instance==true){
            distancia=transform.position.x-gear4LuffyController.instance.transform.position.x;
        }
         if (distancia >0 && distancia<4 )
        {
            estagolpeando = true;
            if (estagolpeando==true)
            {
                sonidoPelea.PlayOneShot(sonidoPelea.clip,1f);
                 CambiarAnimacion(ANIMATION_PISOTON);
                Invoke("isAttack",1.5f);
                habilitar_golpe = false;
            }
        }
        if (distancia>4 && distancia<6)
        {
            estagolpeando = true;
            if (estagolpeando==true)
            {
                sonidoPelea.PlayOneShot(sonidoPelea.clip, 1f);
                CambiarAnimacion(ANIMATION_LANZA);
                Invoke("isAttack",1.5f);
                habilitar_golpe = false;
            }
        }
        if (distancia>6 && distancia<8)
        {
            estagolpeando = true;
            if (estagolpeando ==true)
            {
                sonidoPelea.PlayOneShot(sonidoPelea.clip, 1f);
                CambiarAnimacion(ANIMATION_CIEN_PIES);
                rb.velocity = new Vector2(-3,rb.velocity.y);
                Invoke("isAttack",1.5f);
                habilitar_golpe = false;
            }
        }

        if (distancia>8 && distancia<10)
        {
            estagolpeando = true;
            if (estagolpeando==true)
            {
                sonidoPelea.PlayOneShot(sonidoPelea.clip, 1f/2 );
                CambiarAnimacion(ANIMATION_PUÑO);
                rb.velocity = new Vector2(-5, rb.velocity.y);
                Invoke("isAttack",1f);
                habilitar_golpe = false;
            }
        }
        if (distancia>10 && distancia<13)
        {
            estagolpeando = true;
            if (estagolpeando==true)
            {
                sonidoPuño.PlayOneShot(sonidoPuño.clip, 1f/2);
                habilitar_golpe = true;
                Invoke("golpe_distancia",0.5f);
                Invoke("isAttack", 0.5f);
                Debug.Log("ataque");
            }
            cantGolpe = 1;
        }
       
        if (distancia>13 && distancia<14)
        {
            estagolpeando = true;
            if (estagolpeando==true)
            {
                sonidoPuños.PlayOneShot(sonidoPuños.clip, 1f / 2);
                habilitar_puños = true;
                Invoke("golpe_puños", 0.5f);
                Invoke("isAttack", 0.5f);
                Debug.Log("ataque de puños");
            }
        }
        //------------------------------------------
        if (distancia < 0 && distancia > -4)
        {
            estagolpeando = true;
            if (estagolpeando == true)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                sonidoPelea.PlayOneShot(sonidoPelea.clip, 1f);
                CambiarAnimacion(ANIMATION_PISOTON);
                Invoke("isAttack", 1.5f);
                habilitar_golpe = false;
            }
        }
        if (distancia < -4 && distancia > -6)
        {
            estagolpeando = true;
            if (estagolpeando == true)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                sonidoPelea.PlayOneShot(sonidoPelea.clip, 1f);
                CambiarAnimacion(ANIMATION_LANZA);
                Invoke("isAttack", 1.5f);
                habilitar_golpe = false;
            }
        }
        if (distancia < -6 && distancia >- 8)
        {
            estagolpeando = true;
            if (estagolpeando == true)
            {
                sonidoPelea.PlayOneShot(sonidoPelea.clip, 1f);
                CambiarAnimacion(ANIMATION_CIEN_PIES);
                rb.velocity = new Vector2(3, rb.velocity.y);
                Invoke("isAttack", 1.5f);
                habilitar_golpe = false;
            }
        }

        if (distancia <- 8 && distancia > -10)
        {
            estagolpeando = true;
            if (estagolpeando == true)
            {
                sonidoPelea.PlayOneShot(sonidoPelea.clip, 1f / 2);
                CambiarAnimacion(ANIMATION_PUÑO);
                rb.velocity = new Vector2(3, rb.velocity.y);
                Invoke("isAttack", 1f);
                habilitar_golpe = false;
            }
        }
        if (distancia < -10 && distancia >-13)
        {
            estagolpeando = true;
            if (estagolpeando == true)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                sonidoPuño.PlayOneShot(sonidoPuño.clip, 1f / 2);
                habilitar_golpe = true;
                Invoke("golpe_distancia", 0.5f);
                Invoke("isAttack", 0.5f);
                Debug.Log("ataque");
            }
            cantGolpe = 1;
        }

        if (distancia < -13 && distancia > -14)
        {
            estagolpeando = true;
            if (estagolpeando == true)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                sonidoPuños.PlayOneShot(sonidoPuños.clip, 1f / 2);
                habilitar_puños = true;
                Invoke("golpe_puños", 0.5f);
                Invoke("isAttack", 0.5f);
                Debug.Log("ataque de puños");
            }
        }

    }
    public void golpe_puños()
    {
        if (habilitar_puños == true)
        {
            var position = new Vector2(luffyController.instance.gameObject.transform.position.x - 1, luffyController.instance.transform.position.y);
            Instantiate(puños, position, puños.transform.rotation);

        }
    }
    public void golpe_distancia()
    {
        if (habilitar_golpe==true)
        {
            var position = new Vector2(luffyController.instance.gameObject.transform.position.x-1,luffyController.instance.transform.position.y);
            Instantiate(brazo, position, brazo.transform.rotation);

        }

    }
    public void isAttack()
    {
        estagolpeando = false;
    }
    
    private void ubicarPlayer()
    {
        if (transform.position.x > luffyController.instance.transform.position.x)
        {
            transform.localScale = new Vector3(1.6678f, 1.0157f, 1);
            Debug.Log("esta a la izquierda");
            DirecJugador = "izquierdo";
        }
        else
        {
            transform.localScale = new Vector3(-1.6678f, 1.0157f, 1);
            Debug.Log("esta a la derecha");
           DirecJugador = "derecha";
        }
    }
    
}

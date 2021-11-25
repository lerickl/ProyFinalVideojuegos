using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class katakuriController : MonoBehaviour
{

    //---------------------
    public Text frase;
    
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    public GameObject punto_golpe;
    public GameObject luffy_;

    private float vidaLuffy, vidaLuffyActual = 1;
    private float vidaKatakuri, vidaKatakuriActual = 1;
    private float tiempoDetectar = 4,cuentaBajo;
    private float tiempoTeleport = 3, cuentaBajoTeleport;
    private luffyController luffy;
    private float tiempoAtacar=0;
    private int contador = 0;

    private const int ANIMATION_QUIETO = 0;
    private const int ANIMATION_LANZA = 1;
    private const int ANIMATION_PISOTON = 2;
    private const int ANIMATION_ATTACKDISTANCIA= 3;
    private const int ANIMATION_PUÑO = 4;
    private const int ANIMATION_RAFAGA_PUÑO=5;

 
    public Image barraSaludKatakuriImg;
    public Image barraSaludLuffy;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    private bool estaSaltando=false;
    private bool retroceder=false;
    private bool estagolpeando=false;
    private bool daño=false;
    private string DirecJugador;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        luffy = luffyController.instance;
        cuentaBajo = tiempoDetectar;
        cuentaBajoTeleport = tiempoTeleport;
        ubicarPlayer();
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        muerte_luffy();
        contar_ataque();
        KaatakuriAtaca();
        if (retroceder==true)
        {
            rb.velocity = new Vector2(2,rb.velocity.y);
        }
        if (estagolpeando== false)
        {
            CambiarAnimacion(ANIMATION_QUIETO);
        }
         else
        {
            CambiarAnimacion(ANIMATION_PUÑO);
            daño_golpear();
            audioSource.PlayOneShot(audioClips[0]);
            rb.velocity = new Vector2(-1,rb.velocity.y);
           
        }
        if (contador==1)
        {
            frase_final();
            CambiarAnimacion(ANIMATION_LANZA);
            daño_lufffi();
            rb.velocity = new Vector2(-1, rb.velocity.y);
            audioSource.PlayOneShot(audioClips[0]);
        }
        if (contador==3)
        {
          CambiarAnimacion(ANIMATION_PISOTON);
            daño_lufffi();
            rb.velocity = new Vector2(-1,rb.velocity.y);
            audioSource.PlayOneShot(audioClips[0]);
        }
        if (contador == 5)
        {
           CambiarAnimacion(ANIMATION_ATTACKDISTANCIA);
            daño_lufffi();
            audioSource.PlayOneShot(audioClips[0]);
           rb.velocity = new Vector2(-1, rb.velocity.y);
        }
        if (contador==7)
        {
            CambiarAnimacion(ANIMATION_RAFAGA_PUÑO);
            daño_lufffi();
            audioSource.PlayOneShot(audioClips[0]);
            rb.velocity = new Vector2(-1, rb.velocity.y);
        }

    }

    private void contar_ataque()
    {
        cuentaBajo -= Time.deltaTime;
        cuentaBajoTeleport -= Time.deltaTime;

        if (cuentaBajo<=0f)
        {
          //  KaatakuriAtaca();
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
        var distancia = transform.position.x - luffyController.instance.transform.position.x;
        
         if (distancia > 16)
        {
            estagolpeando = true;
        }
        else
        {
            estagolpeando = false;
        }
    }

    private void daño_golpear()
    {
        var distancia_golpe = punto_golpe.transform.position.x - luffyController.instance.transform.position.x;

        if (distancia_golpe <=0)
        {
            DañoRecibeLuffy();

            Debug.Log("puño");
        }
    }
    public void daño_lufffi()
    {
        var distancia_luffi = transform.position.x - luffyController.instance.transform.position.x;
        Debug.Log(distancia_luffi+"luffi");
        if (distancia_luffi <=1)
        {
            DañoRecibeLuffy();
        }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "A")
        {
            estagolpeando = true;
            contador++;
            if (contador == 2 || contador == 4 || contador == 6 || contador == 8)
            {
                estagolpeando = false;
                retroceder = true;

            }

            Debug.Log(contador);
        }
       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="A")
        {
            Debug.Log("collision_personaje");
            estagolpeando = false;
            DañoRecibeKatakuri();

            if (vidaKatakuriActual<=0)
            {
                Destroy(this.gameObject);
                frase_final();
                Debug.Log("muerte");
            }
            
        }

        
    }

    private void frase_final()
    {
        var position = new Vector2(frase.transform.position.x,frase.transform.position.y);
        var rotation =frase.transform.rotation;
        Instantiate(frase,position,rotation);
    }
}

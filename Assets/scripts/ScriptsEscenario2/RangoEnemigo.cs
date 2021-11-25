using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangoEnemigo : MonoBehaviour
{
    public Animator ani;
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    public MovimientosDoffy enemigo;
    // private static readonly int Walk = Animator.StringToHash("walk");
    // private static readonly int Run = Animator.StringToHash("run");
    // private static readonly int Attack = Animator.StringToHash("attack");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("LUFFYDres_0"))
        {
            //ani.SetInteger("Estado", 3);
           
            
            // ani.SetBool(Walk, false);
            // ani.SetBool(Run, false);
            // ani.SetBool(Attack, true);
            enemigo.EstaAtacando = true;
            //GetComponent<BoxCollider2D>().enabled = false;
            
        }
    }
    
    
   
}

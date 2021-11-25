using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class supernovaController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocityX = -5f;
    private luffyController luffyController;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        luffyController = FindObjectOfType<luffyController>();
        
        Destroy(gameObject, 4);
        
    }

    // Update is called once per frame
    void Update()
    {
          rb.velocity = Vector2.right * velocityX;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "LUFFYDres_0")
        {
             
            Destroy(this.gameObject);
            //PersonajeController.IncrementerPuntajeEn10();
        }
    }
}

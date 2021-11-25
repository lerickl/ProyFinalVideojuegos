using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nubeInvokController : MonoBehaviour
{
    
    public GameObject LuffyGearFourt;
    // Start is called before the first frame update
    void Start()
    {
       Invoke("invok", 1.5f);
       Destroy(gameObject, 1.6f);
       
    }

    // Update is called once per frame
    void Update()
    {
            
    }
    public void invok(){
        var position = new Vector3(transform.position.x, transform.position.y ,transform.position.z-1);
        Instantiate(LuffyGearFourt, position, LuffyGearFourt.transform.rotation);
    }
}

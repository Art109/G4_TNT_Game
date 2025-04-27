using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMov : MonoBehaviour
{
    
    public Rigidbody2D Popup;
    public Transform Target;
    public float VeloPop = 10f;
    
    

    void Start()
    {
        Popup = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
       Vector2 direcao = (Target.position - transform.position);
       Popup.velocity = direcao * VeloPop;
    }
}

#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plsWork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + " got on trigger enter: " + other.gameObject.name);
        if(other.gameObject.GetComponent<Damager>() == true)
        {
            Debug.Log(this.gameObject.name + " found a Damager component at " + other.gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("On collision enter: " + collision.gameObject.name);
    }
}

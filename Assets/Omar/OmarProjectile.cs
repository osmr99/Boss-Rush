#pragma warning disable IDE0051
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omar
{
    public class OmarProjectile : MonoBehaviour
    {
        [SerializeField] float speed;
        Rigidbody rigidBody;
        GameObject gameObj;
        // Start is called before the first frame update
        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.velocity = transform.forward * speed;
            Destroy(gameObject, 2f);
        }

        public void DestroyProjectile()
        {
            if (gameObj.GetComponent<Damageable>())
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            gameObj = other.gameObject;
            //Debug.Log(gameObj);
        }
    }
}
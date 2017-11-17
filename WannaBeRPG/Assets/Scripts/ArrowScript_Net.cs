using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArrowScript_Net : NetworkBehaviour
{
    public float throwPower;
    protected Rigidbody r;
    protected Vector3 v;
    protected bool hasHit = false;
    protected float timer = 0;
    protected GameObject go;
    protected PlayerScript.ArrowType type;

    protected void Start()
    {
        if(!isServer)
        {
            return;
        }
            r = GetComponent<Rigidbody>();
            r.AddForce(transform.up * throwPower);
    }

    [ServerCallback]
    protected void Update()
    {
        if((timer += Time.deltaTime) > 3 && hasHit)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
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
        r = GetComponent<Rigidbody>();
        r.AddForce(transform.up * throwPower);
    }

    protected void Update()
    {
        if((timer += Time.deltaTime) > 3 && hasHit)
        {
            GameController.Instance.playerScript.ReturnLostArrow(go, type);
        }
    }

}

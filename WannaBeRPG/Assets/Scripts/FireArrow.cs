using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : ArrowScript
{
    private void Awake()
    {
        go = gameObject;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Props"))
        {
            PropsScript p = collision.gameObject.gameObject.GetComponent<PropsScript>();

            if(p.IsFlammable())
            {
                p.SetOnFire();
            }
        }

        hasHit = true;
    }
}


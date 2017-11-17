using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingArrow : ArrowScript
{

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            int randX = Random.Range(-1, 2);
            int randZ = Random.Range(-1, 2);
            if (randX == 0)
            {
                randX = -1;
            }
            if (randZ == 0)
            {
                randZ = -1;
            }
            r.AddForce(new Vector3(randX * 500, -500, randZ * 500));
        }
        go = gameObject;
        type = PlayerScript.ArrowType.Piercing;
        hasHit = true;
    }
}

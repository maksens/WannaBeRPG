using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinScript : MonoBehaviour
{
    private Rigidbody r;
    private Animator a;
    private bool beenHit = false;
    private float timer = 0;

    private void Awake()
    {
        r = GetComponent<Rigidbody>();
        a = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Arrow"))
        {
            a.SetBool("beenHit", true);
            beenHit = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerScript p = GameController.Instance.playerScript;
            p.hp -= 100;
        }
    }
	
	private void Update ()
    {
        if (beenHit)
        {
            if ((timer += Time.deltaTime) > 0.3f)
            {
                a.SetBool("beenHit", false);
                beenHit = false;
                timer = 0;
            }
        }
    }
}

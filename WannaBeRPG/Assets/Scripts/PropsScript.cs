using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsScript : MonoBehaviour, IFlammable
{
    protected bool isFlammable;

    public virtual bool IsFlammable()
    {
        return isFlammable;
    }

    public virtual void SetOnFire()
    {
        Debug.Log("Burn");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public PlayerScript_Net playerScriptNet;
    public PlayerScript playerScript;
    public GameObject player;
    public Camera mainCam;

	// Use this for initialization

    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}
}

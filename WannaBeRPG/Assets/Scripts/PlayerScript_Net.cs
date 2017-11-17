using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript_Net : NetworkBehaviour
{
    private Animator playerAnim;
    private Rigidbody rigid;
    private Vector3 moveDirection = new Vector3();
    public GameObject[] arrowPrefabs;
    public GameObject arrowSpawnPoint;
    private bool isCrouching = false;
    private bool isDead = false;
    private bool isMoving = false;
    private bool isRunning = true;
    private bool hasLaunchArrow = false;
    private bool hasStartedToDie = false;
    private float moveSpeed = 1f;
    private float rotX;
    private float rotY;
    private float throwPow = 0;
    private float timer = 0;
    private int arrowCount = 10;
    public int hp = 100;
    private const int ARROW_TYPE = 2;
    private const int QUIVER_MAX_QUANTITY = 10;
    private const int PIERCING_SPOT = 0;
    private const int FIRE_SPOT = 1;
    private Camera mainCam;
    public GameObject cameraSpot;

    public enum ArrowType
    {
        Piercing = 0,
        Fire = 1,
        Ice = 2
    }

    private enum KeyType
    {
        W = KeyCode.W,
        A = KeyCode.A,
        S = KeyCode.S,
        D = KeyCode.D,
        C = KeyCode.C,
        Shift = KeyCode.Z,
        Space = KeyCode.Space,
        Q = KeyCode.Q
    }

    private void Start()
    {
       if(!isLocalPlayer)
       {
           Destroy(this);
           return;
       }

        mainCam = GameController.Instance.mainCam;
        rigid = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        if (hp <= 0)
        {
            isDead = true;
        }

        if (!isDead)
        {
            RotatePlayer();
            PlayerInput();
            PrepareArrow();
            rigid.velocity = moveDirection * moveSpeed;
        }
        else
        {
            if (!hasStartedToDie)
            {
                playerAnim.SetBool("isDead", true);
                hasStartedToDie = true;
                moveDirection = Vector3.zero;
            }
        }
    }

    private void RotatePlayer()
    {
        rotY += Input.GetAxis("Mouse X") * 3;
        rotX += Input.GetAxis("Mouse Y") * -2;
        rotX = Mathf.Clamp(rotX, -20, 20);
        transform.rotation = Quaternion.Euler(0, rotY, 0);
        mainCam.gameObject.transform.position = cameraSpot.transform.position;
        mainCam.gameObject.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        arrowSpawnPoint.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    [Command]
    private void CmdSpawnShell(ArrowType type)
    {
        GameObject arrow = Instantiate(arrowPrefabs[0], arrowSpawnPoint.transform.position, Quaternion.Euler(new Vector3(90 + rotX, rotY, 0)));
        ArrowScript_Net a = arrow.GetComponent<ArrowScript_Net>();
        a.throwPower = throwPow;
        NetworkServer.Spawn(arrow);
    }

    private void PrepareArrow()
    {
        if (Input.GetMouseButton(0))
        {
            if (!hasLaunchArrow)
            {
                playerAnim.SetBool("isAiming", true);
                hasLaunchArrow = true;
            }

            throwPow += Time.deltaTime * 400;

            if (throwPow >= 1000)
                throwPow = 1000;
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnim.SetBool("isAiming", false);
            hasLaunchArrow = false;
            CmdSpawnShell(ArrowType.Piercing);
            throwPow = 0;
        }
    }

    private void PlayerInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerAnim.SetBool("WalkFront", true);
            playerAnim.SetBool("WalkBack", false);
            playerAnim.SetBool("WalkRight", false);
            playerAnim.SetBool("WalkLeft", false);
            moveDirection = transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerAnim.SetBool("WalkFront", false);
            playerAnim.SetBool("WalkBack", true);
            playerAnim.SetBool("WalkRight", false);
            playerAnim.SetBool("WalkLeft", false);
            moveDirection = -transform.forward;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerAnim.SetBool("WalkFront", false);
            playerAnim.SetBool("WalkBack", false);
            playerAnim.SetBool("WalkRight", false);
            playerAnim.SetBool("WalkLeft", true);
            moveDirection = -transform.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerAnim.SetBool("WalkFront", false);
            playerAnim.SetBool("WalkBack", false);
            playerAnim.SetBool("WalkRight", true);
            playerAnim.SetBool("WalkLeft", false);
            moveDirection = transform.right;
        }
        else
        {
            playerAnim.SetBool("WalkFront", false);
            playerAnim.SetBool("WalkBack", false);
            playerAnim.SetBool("WalkRight", false);
            playerAnim.SetBool("WalkLeft", false);
            moveDirection = new Vector3(0, rigid.velocity.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching)
            {
                playerAnim.SetBool("isCrouching", true);
                isCrouching = true;
                moveSpeed = 1f;
            }
            else
            {
                playerAnim.SetBool("isCrouching", false);
                isCrouching = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isRunning)
            {
                playerAnim.SetBool("isRunning", true);
                isRunning = true;
                moveSpeed = 3.5f;
            }
            else
            {
                playerAnim.SetBool("isRunning", false);
                isRunning = false;
                moveSpeed = 2f;
            }
        }
    }
}

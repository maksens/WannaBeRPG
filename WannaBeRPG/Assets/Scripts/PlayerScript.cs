using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private Animator playerAnim;
    private Rigidbody rigid;
    private Vector3 moveDirection = new Vector3();
    public GameObject[] arrowPrefabs;
    public GameObject arrowSpawnPoint;
    public List<LinkedList<GameObject>> quiver = new List<LinkedList<GameObject>>(ARROW_TYPE);
    public List<LinkedList<GameObject>> lostArrows = new List<LinkedList<GameObject>>(ARROW_TYPE);
    private Dictionary<GameObject, ArrowScript> arrowToScript = new Dictionary<GameObject, ArrowScript>();
    private bool isCrouching = false;
    private bool isDead = false;
    private bool isMoving = false;
    private bool isRunning = true;
    private bool hasLaunchArrow = false;
    private bool hasStartedToDie = false;
    public bool hasBow = false;
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
        mainCam = GameController.Instance.mainCam;
        rigid = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        if(hasBow)
        {
            //for (int i = 0; i < quiver.Count; ++i)
            //{
            quiver.Add(new LinkedList<GameObject>());
            //}

            //for (int i = 0; i < lostArrows.Count; ++i)
            //{
            lostArrows.Add(new LinkedList<GameObject>());
            //}

            for (int i = 0; i < arrowCount; ++i)
            {
                GameObject g = Instantiate(arrowPrefabs[0]);
                arrowToScript.Add(g, g.GetComponent<ArrowScript>());
                g.SetActive(false);
                quiver[(int)ArrowType.Piercing].AddFirst(g);
            }
        }
    }

    private void FixedUpdate()
    {
        //foreach (KeyCode key in System.Enum.GetValues(typeof(KeyType)))
        //    if (Input.GetKeyDown(key))
        //        MovePlayer((KeyType)key);
        //    else if(!Input.anyKey)
        //    {
        //        playerAnim.SetBool("WalkFront", false);
        //        playerAnim.SetBool("WalkBack", false);
        //        playerAnim.SetBool("WalkRight", false);
        //        playerAnim.SetBool("WalkLeft", false);
        //        moveDirection = new Vector3(0, rigid.velocity.y, 0);
        //    }

        if (hp <= 0)
        {
            isDead = true;
        }

        if (!isDead)
        {
            RotatePlayer();
            PlayerInput();

            if(hasBow)
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

        if (hasBow)
            arrowSpawnPoint.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    public void ReturnLostArrow(GameObject arrow, ArrowType arrowType)
    {
        arrow.SetActive(false);
        arrowToScript[arrow].throwPower = 0;
        lostArrows[(int)arrowType].AddFirst(arrow);
    }

    private void LaunchArrow(ArrowType type)
    {
        int arrowType = (int)type;

        if (quiver[arrowType].Count != 0)
        {
            GameObject arrow = quiver[arrowType].First.Value;
            quiver[arrowType].RemoveFirst();
            arrow.SetActive(true);
            arrow.transform.position = arrowSpawnPoint.transform.position;
            arrow.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            arrowToScript[arrow].throwPower = throwPow;
        }
        else
            Debug.Log("No More Arrows of That Type");
    }

    public void PickUpArrow(int nbOfArrow, ArrowType type)
    {
        int arrowType = (int)type;

        for (int i = 0; i < nbOfArrow; ++i)
        {
            if (lostArrows[arrowType].Count > 0)
            {
                if (quiver[arrowType].Count < QUIVER_MAX_QUANTITY)
                {
                    quiver[arrowType].AddFirst(lostArrows[arrowType].First.Value);
                    lostArrows[arrowType].RemoveFirst();
                }
                else
                    Debug.Log("Quiver Is Full");
            }
        }
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
            LaunchArrow(ArrowType.Piercing);
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

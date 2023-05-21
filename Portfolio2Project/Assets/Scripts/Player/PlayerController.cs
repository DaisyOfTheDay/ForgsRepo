using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Skills skills;

    [Header("----- Player Stats -----")]
    [Range(1, 25)][SerializeField] int iHP;
    [Range(1, 20)][SerializeField] float playerSpeed;
    [Range(1, 20)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int maxJumps;
    [Range(2, 5)][SerializeField] float sprintMod;
    [SerializeField] private float damagecoolDown;
    public NewStaff.Element playerElement;

    [Header("----- Player Weapon -----")]
    public NewStaff playerWeapon; //set in awake
    [SerializeField] int ShootRange; //the distance the player can shoot
    [SerializeField] float ShotCooldown; //the cooldown the player has between shots    
    private bool isShooting; //checks if the player is currently shooting

    private int jumpsUsed;
    private Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isSprinting;
    private int iHPOriginal;
    private bool damagedRecently;

<<<<<<< HEAD
    [Header("----- Player Weapon -----")]
    //the distance the player can shoot
    [SerializeField] int ShootRange;
    //the cooldown the player has between shots
    [SerializeField] float ShotCooldown;
    //checks if the player is currently shooting
    bool isShooting;

    //[SerializeField] GameObject playerWeaponHolder;
    [SerializeField] public NewStaff playerWeaponScript;


    GameObject playerWeapon;

    // Start is called before the first frame update
=======
>>>>>>> branchworks
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerWeapon = GetComponentInChildren<NewStaff>();
    }

    void Start()
    {
        iHPOriginal = iHP;
        //Debug.Log(iHPOriginal);
        //Debug.Log(iHP);
    }

    void Update()
    {
        if (skills.canMove())
        {
            Movement();
        }

        Sprint();

        if (Input.GetButton("Shoot") && !isShooting && !gameManager.instance.isPaused)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetAxis("Movement1") != 0)
        {
            skills.useSkill(1);
        }
        if (Input.GetAxis("Movement2") != 0)
        {
            skills.useSkill(2);
        }
        if (Input.GetAxis("Movement3") != 0)
        {
            skills.useSkill(3);
        }
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpsUsed = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpsUsed < maxJumps)
        {
            jumpsUsed++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        if (!isSprinting && Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= sprintMod;
        }
        else if (isSprinting && Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= sprintMod;
        }
    }
    public void TakeDamage(int amount)
    {
        if (damagedRecently == false)
        {
<<<<<<< HEAD
            damagedRecently = true;
            StartCoroutine(resetDamagedRecently());
            Debug.Log("my damage" + amount);
            //-= used, negative amounts heal.         
            iHP -= amount;
=======
            StartCoroutine(ResetDamagedRecently());
            
            //Debug.Log("my damage" + amount);        
            iHP -= amount; //-= used, negative amounts heal. 
>>>>>>> branchworks
            if (amount > 0)
            {
                gameManager.instance.showDamage();
                if (iHP <= 0)
                {
                    iHP = 0;
                    gameManager.instance.youLose();
                }
            }
            else
            {
                if (iHP > iHPOriginal)
                {
                    iHP = iHPOriginal;
                }
            }
            gameManager.instance.UpdateHealthBar();
        }
    }

    IEnumerator resetDamagedRecently()
    {
        damagedRecently = true;
        yield return new WaitForSeconds(damagecoolDown);
        damagedRecently = false;
    }

    IEnumerator Shoot()
    {
        if (!isShooting)
        {
            isShooting = true;

<<<<<<< HEAD
            RaycastHit hit;

            if (playerWeaponScript != null)
                playerWeaponScript.Shoot(ShotCooldown);
           /* else if (waterWeapon != null)
                waterWeapon.Shoot(ShotCooldown);
            else if (earthWeapon != null)
                earthWeapon.Shoot(ShotCooldown);*/

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, ShootRange))
            //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * ShootRange, out hit))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                if (damageable != null)
                {

                    damageable.TakeDamage(2);
                }
=======
            if (playerWeapon != null)
            {
                playerWeapon.Shoot();
>>>>>>> branchworks
            }

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, ShootRange))
            {
                //Debug.Log("Shot");

                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    IDamage damageable = hit.collider.GetComponent<IDamage>();
                    damageable.TakeDamage(1);
                }
            }
        }

        yield return new WaitForSeconds(ShotCooldown);

        isShooting = false;
    }

<<<<<<< HEAD

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerRespawn.transform.position;
        iHP = iHPOriginal;
        gameManager.instance.ResetHpBar();
        controller.enabled = true;
    }

    public int getHealth()
=======
    public int GetHealth()
>>>>>>> branchworks
    {
        return iHP;
    }

    public int getOriginalHealth()
    {
        return iHPOriginal;
    }

    public void changeJumpsUsed(int ammount)
    {
        jumpsUsed += ammount;
    }
<<<<<<< HEAD

    //Changer Gravity and Returns Original Gravity
    public float changeGravity(float ammount)
=======
    public float ChangeGravity(float ammount) //Changes Gravity and Returns Original Gravity
>>>>>>> branchworks
    {
        float gravityOrig = gravityValue;
        gravityValue = ammount;
        return gravityOrig;
    }
}
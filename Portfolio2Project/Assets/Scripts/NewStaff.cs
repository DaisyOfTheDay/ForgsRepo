using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class NewStaff : MonoBehaviour
{
    [Header("----- Shooting Stuff -----")]
    [SerializeField] private bool addBulletSpread;
    [SerializeField] private Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform shootPos;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float delay;
    [SerializeField] private LayerMask mask;

    [Header("----- Melee Stuff -----")]
    [SerializeField] private float meleeCooldown;
    [SerializeField] Collider hitbox;
    [SerializeField] GameObject[] weaponModels;
    //[SerializeField] Texture fireStaff;
    //[SerializeField] Texture waterStaff;
    //[SerializeField] Texture earthStaff;

    [Header("----- Special Attack Stuff -----")]
    [SerializeField] GameObject fireball;
    [SerializeField] public ParticleSystem explosion;
    [SerializeField] float fireballSpeed;

    private bool canSpecial;

    public enum Element
    {
        Fire,
        Water,
        Earth
    }
    public Element playerElement;

    PlayerController player;

    private float lastShootTime;
    public bool isShooting;
    public bool canMelee;
    public GameObject weapon;
    public bool isAttacking;

    [Header("----- Audio Stuff -----")]
    [SerializeField] List<AudioClip> audios = new List<AudioClip>();

    private void Awake()
    {
        hitbox.enabled = false;
        canSpecial = true;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //switch (playerElement)
        //{
        //    case Element.Fire:
        //        weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", fireStaff);
        //        break;
        //    case Element.Water:
        //        weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", waterStaff);
        //        break;
        //    case Element.Earth:
        //        weaponModels[0].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", earthStaff);
        //        break;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (playerElement != player.playerElement)
        {
            SetElement();
        }
        Melee();
        SpecialAttack();
    }

    public void Shoot()
    {
        Debug.Log("Weapon Shoot Called");
        if (Input.GetButton("Shoot"))
        {

            isShooting = true;
            canMelee = false;
            if (lastShootTime + delay < Time.time)
            {
                Vector3 direction = GetDirection();
                if (Physics.Raycast(shootPos.position, direction, out RaycastHit hit, float.MaxValue, mask))
                {
                    TrailRenderer trail = Instantiate(trailRenderer, shootPos.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                    lastShootTime = Time.time;
                }
            }
        }
        StartCoroutine(ResetShooting());
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = transform.parent.forward;

        if (addBulletSpread)
        {
            dir += new Vector3(
                Random.Range(bulletSpread.x, bulletSpread.x),
                Random.Range(bulletSpread.y, bulletSpread.y),
                Random.Range(bulletSpread.z, bulletSpread.z)
                );
        }
        return dir;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = shootPos.transform.position;

        while (time < 1)
        {
            if (trail.gameObject != null)
            {
                trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
                time += Time.deltaTime / trail.time;
            }
            yield return null;
        }

        if (trail.gameObject != null)
        {
            trail.transform.position = hit.point;
        }

        Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal)); //hit.point

        if (trail.gameObject != null)
        {
            Destroy(trail.gameObject);
            yield return new WaitForSeconds(1);
        }
    }

    public void Melee()
    {
        if (canMelee && Input.GetMouseButtonDown(1))
        {
            canMelee = false;
            isAttacking = true;
            hitbox.enabled = true;
            Animator anim = weapon.GetComponent<Animator>();
            //weaponModels[0].SetActive(false);
            switch (playerElement)
            {
                case Element.Fire:
                    Debug.Log("Melee Fire");
                    weaponModels[1].SetActive(true);
                    GameManager.instance.playerScript.PlayExternalAudio(audios[3]);
                    anim.SetTrigger("SwordMelee");
                    break;
                case Element.Water:
                    weaponModels[2].SetActive(true);
                    GameManager.instance.playerScript.PlayExternalAudio(audios[4]);
                    anim.SetTrigger("SpearMelee");
                    break;
                case Element.Earth:
                    weaponModels[3].SetActive(true);
                    GameManager.instance.playerScript.PlayExternalAudio(audios[5]);
                    anim.SetTrigger("HammerMelee");
                    break;
            }

            StartCoroutine(ResetMeleeCooldown());
        }
    }
    IEnumerator ResetMeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
        isAttacking = false;
        hitbox.enabled = false;
    }

    IEnumerator ResetShooting()
    {
        yield return new WaitForSeconds(0.5f);
        isShooting = false;
        canMelee = true;
    }

    public void SetElement()
    {
        playerElement = player.playerElement;
    }

    public void ResetMeleeWeaponModel()
    {
        //weaponModels[0].SetActive(true);
        weaponModels[1].SetActive(false);
        weaponModels[2].SetActive(false);
        weaponModels[3].SetActive(false);
    }


    public AudioClip GetShootAudio()
    {
        return audios[(int)playerElement];
    }

    public void SpecialAttack()
    {
        if (Input.GetButtonUp("Special"))   //When the button is held and then released
        {
            if (!canSpecial)
            {
                return;
            }

            switch (playerElement)
            {
                case Element.Fire:
                    isShooting = true;
                    Debug.Log("Special");
                    Instantiate(fireball, shootPos.transform.position, shootPos.transform.rotation);
                    //fb.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * fireballSpeed);//.velocity = shootPos.transform.forward * 10 * Time.deltaTime;
                    //Destroy(fb, 5);
                    break;
                case Element.Water:

                    break;
                case Element.Earth:

                    break;
            }
        }
        
    }
}
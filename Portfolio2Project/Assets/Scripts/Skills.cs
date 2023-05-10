using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform blinkAimIndicatorPrefab;

    [Header("----- Values ------")]
    [Header("~Dash~")]
    [Range(1,50)][SerializeField] float DashSpeed;
    [Range(0,1)][SerializeField] float DashTime;
    [Range(1, 20)][SerializeField] float DashCooldown;
    bool canDash;

    [Header("~High Jump~")]
    [Range(1, 50)][SerializeField] float JumpForce;
    [Range(0, 1)][SerializeField] float JumpTime;
    [Range(1, 20)][SerializeField] float HiJumpCooldown;
    bool canHiJump;

    [Header("~Slow Fall~")]
    [Range(1, 50)][SerializeField] float NewGravityForce;
    [Range(1, 20)][SerializeField] float SlowFallCooldown;
    bool canSlowFall;

    [Header("~Blink~")]
    [Range(1, 50)][SerializeField] float BlinkDistance;
    [Range(1, 20)][SerializeField] float BlinkCooldown;
    bool canBlink;
    bool aiming;

    [Header("~Invisibility~")]
    [Range(1, 50)][SerializeField] float invisDuration;
    [Range(1, 20)][SerializeField] float InvisCooldown;
    bool canInvis;


    bool CanMove = true;
    float gravityOrig;
    Transform blinkAimIndicator;
    public void Dash()
    {
        if (canDash)
        {
            canDash= false;
            CanMove= false;
            playerController.changeJumpsUsed(1);
            StartCoroutine(dashCoroutine());
            StartCoroutine(dashCooldownCoroutine());
        }


    }

    IEnumerator dashCoroutine()
    {
        var startTime = Time.time;
        while (Time.time < startTime + DashTime)
        {
            controller.Move(transform.forward * DashSpeed * Time.deltaTime);
            yield return null;
        }
        CanMove= true;
        StopCoroutine(dashCoroutine());

    }

    IEnumerator dashCooldownCoroutine()
    {
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }

    public void hiJump()
    {
        if (canHiJump)
        {
            canHiJump= false;
            CanMove = false;
            playerController.changeJumpsUsed(1);
            StartCoroutine(hiJumpCoroutine());
            StartCoroutine(hiJumpCooldownCoroutine());
        }

    }

    IEnumerator hiJumpCoroutine()
    {

        var startTime = Time.time;
        while (Time.time < startTime + JumpTime)
        {
            controller.Move(transform.up * JumpForce * Time.deltaTime);
            yield return null;
        }
        CanMove = true;
        StopCoroutine(hiJumpCoroutine());

    }

    IEnumerator hiJumpCooldownCoroutine()
    {
        yield return new WaitForSeconds(HiJumpCooldown);
        canHiJump = true;
    }

    public void slowFall()
    {
        if (canSlowFall)
        {
            canSlowFall= false;
            StartCoroutine(slowFallCoroutine());
            StartCoroutine(slowFallCooldownCoroutine());
        }
    }

    IEnumerator slowFallCoroutine()
    {
        gravityOrig = playerController.changeGravity(NewGravityForce);
        while(!controller.isGrounded)
        {
            if (controller.velocity.y > 0)
            {
                playerController.changeGravity(gravityOrig);
            }
            else
            {
                playerController.changeGravity(NewGravityForce);
            }
            yield return null;
        }
        playerController.changeGravity(gravityOrig);
        StopCoroutine(slowFallCoroutine());

    }

    IEnumerator slowFallCooldownCoroutine()
    {
        yield return new WaitForSeconds(SlowFallCooldown);
        canSlowFall = true;
    }

    public void blinkAim()
    {
        if (canBlink)
        {
            canBlink= false;
            aiming = true;
            StartCoroutine(blinkAimCoroutine());
            StartCoroutine(blinkCooldownCoroutine());
        }
    }

    IEnumerator blinkAimCoroutine()
    {
        while (aiming)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, BlinkDistance))
            {
                if (!blinkAimIndicator)
                {
                    blinkAimIndicator = Instantiate(blinkAimIndicatorPrefab);
                }
                blinkAimIndicator.position = hit.point;
            }
            else
            {
                if (blinkAimIndicator)
                {
                    Destroy(blinkAimIndicator.gameObject);
                }

            }
            Debug.Log("Coroutine Running");
            yield return null;
        }

    }

    IEnumerator blinkCooldownCoroutine()
    {
        yield return new WaitForSeconds(BlinkCooldown);
        canBlink = true;
    }

    public void blinkFire()
    {
        controller.enabled= false;
        aiming = false;
        StopCoroutine(blinkAimCoroutine());
        if (blinkAimIndicator)
        {
            transform.position = new Vector3(blinkAimIndicator.position.x,blinkAimIndicator.position.y + 1,blinkAimIndicator.position.z);

            Destroy(blinkAimIndicator.gameObject);
        }
        controller.enabled= true;

    }

    public void invisible()
    {
        if (canInvis)
        {
            canInvis= false;
            this.gameObject.layer = 8;
            StartCoroutine(invisibilityCoroutine());
            StartCoroutine(InvisCooldownCoroutine());
        }
    }

    IEnumerator invisibilityCoroutine()
    {
        yield return new WaitForSeconds(invisDuration);
        this.gameObject.layer = 3;
        StopCoroutine(invisibilityCoroutine());
    }

    IEnumerator InvisCooldownCoroutine()
    {
        yield return new WaitForSeconds(InvisCooldown);
        canInvis = true;
    }


    public bool canMove()
    {
        return CanMove;
    }

}

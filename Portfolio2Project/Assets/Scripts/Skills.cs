using System;
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
    public enum skill
    {
        Dash, HiJump, SlowFall, Blink, Invisibility
    }

    [Header("----- Values ------")]
    [Header("~Dash~")]
    [Range(1, 50)][SerializeField] float DashSpeed;
    [Range(0, 1)][SerializeField] float DashTime;
    [Range(1, 20)][SerializeField] float DashCooldown;
    bool canDash = true;

    [Header("~High Jump~")]
    [Range(1, 50)][SerializeField] float JumpForce;
    [Range(0, 1)][SerializeField] float JumpTime;
    [Range(1, 20)][SerializeField] float HiJumpCooldown;
    bool canHiJump = true;

    [Header("~Slow Fall~")]
    [Range(1, 50)][SerializeField] float NewGravityForce;
    [Range(1, 20)][SerializeField] float SlowFallCooldown;
    bool canSlowFall = true;

    [Header("~Blink~")]
    [Range(1, 50)][SerializeField] float BlinkDistance;
    [Range(1, 20)][SerializeField] float BlinkCooldown;
    bool canBlink = true;
    bool aiming;

    [Header("~Invisibility~")]
    [Range(1, 50)][SerializeField] float invisDuration;
    [Range(1, 20)][SerializeField] float InvisCooldown;
    bool canInvis = true;

    skill activeSkill1 = skill.Blink;
    skill activeSkill2 = skill.Dash;


    bool CanMove = true;
    float gravityOrig;
    Transform blinkAimIndicator;
    public void Dash()
    {
        if (canDash)
        {
            canDash = false;
            CanMove = false;
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
        CanMove = true;
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
            canHiJump = false;
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
            canSlowFall = false;
            StartCoroutine(slowFallCoroutine());
            StartCoroutine(slowFallCooldownCoroutine());
        }
    }

    IEnumerator slowFallCoroutine()
    {
        gravityOrig = playerController.changeGravity(NewGravityForce);
        while (!controller.isGrounded)
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
            canBlink = false;
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
            if (activeSkill1 == skill.Blink)
            {
                if (Input.GetAxis("Movement1") == 0)
                {
                    blinkFire();
                }
            }
            else
            {
                if (Input.GetAxis("Movement2") == 0)
                {
                    blinkFire();
                }
            }
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
        controller.enabled = false;
        aiming = false;
        StopCoroutine(blinkAimCoroutine());
        if (blinkAimIndicator)
        {
            transform.position = new Vector3(blinkAimIndicator.position.x, blinkAimIndicator.position.y + 1, blinkAimIndicator.position.z);

            Destroy(blinkAimIndicator.gameObject);
        }
        controller.enabled = true;

    }

    public void invisible()
    {
        if (canInvis)
        {
            canInvis = false;
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

    public void setSkill(int slot, skill skill)
    {
        if (slot == 1)
        {
            activeSkill1 = skill;
        }
        else if (slot == 2)
        {
            activeSkill2 = skill;
        }
    }

    public skill getSkill1()
    {
        return activeSkill1;
    }
    public skill getSkill2()
    {
        return activeSkill2;
    }

    public void useSkill(int slot)
    {
        if (slot == 1)
        {
            Action skill = testSkill(activeSkill1);
            skill();
        }
        else if (slot == 2)
        {
            Action skill = testSkill(activeSkill2);
            skill();
        }
    }

    public float getCooldown(skill sk)
    {
        switch (sk)
        {
            case (skill.Dash):
                {
                    return DashCooldown;
                }
            case (skill.HiJump):
                {
                    return HiJumpCooldown;
                }
            case (skill.SlowFall):
                {
                    return SlowFallCooldown;
                }
            case (skill.Blink):
                {
                    return BlinkCooldown;
                }
            case (skill.Invisibility):
                {
                    return InvisCooldown;
                }
        }
        return 0;
    }
    Action testSkill(skill sk)
    {
        switch (sk)
        {
            case (skill.Dash):
                {
                    return Dash;
                }
            case (skill.HiJump):
                {
                    return hiJump;
                }
            case (skill.SlowFall):
                {
                    return slowFall;
                }
            case (skill.Blink):
                {
                    return blinkAim;
                }
            case (skill.Invisibility):
                {
                    return invisible;
                }
        }
        return null;


    }


}

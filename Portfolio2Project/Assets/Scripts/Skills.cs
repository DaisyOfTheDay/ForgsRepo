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

    [Header("~High Jump~")]
    [Range(1, 50)][SerializeField] float JumpForce;
    [Range(0, 1)][SerializeField] float JumpTime;

    [Header("~Slow Fall~")]
    [Range(1, 50)][SerializeField] float NewGravityForce;

    [Header("~Blink~")]
    [Range(1, 50)][SerializeField] float BlinkDistance;
    bool aiming;

    [Header("~Invisibility~")]
    [Range(1, 50)][SerializeField] float invisDuration;



    bool CanMove = true;
    float gravityOrig;
    Transform blinkAimIndicator;
    public void Dash()
    {
        CanMove= false;
        playerController.changeJumpsUsed(1);
        StartCoroutine(dashCoroutine());

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

    public void hiJump()
    {
        CanMove = false;
        playerController.changeJumpsUsed(1);
        StartCoroutine(hiJumpCoroutine());

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

    public void slowFall()
    {

        StartCoroutine(slowFallCoroutine());

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

    public void blinkAim()
    {
        aiming = true;
        StartCoroutine(blinkAimCoroutine());

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
        this.gameObject.layer = 8;
        StartCoroutine(invisibilityCoroutine());
    }

    IEnumerator invisibilityCoroutine()
    {
        yield return new WaitForSeconds(invisDuration);
        this.gameObject.layer = 3;
        StopCoroutine(invisibilityCoroutine());
    }


    public bool canMove()
    {
        return CanMove;
    }

}

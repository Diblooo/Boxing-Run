using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class BoxingController : MonoBehaviour
{
    [Header("Parameters")]
    public float hitRange;
    public float heightRay;
    public float sidesRayDistance;

    private int hitableLayer = 1 << 7;
    private PlayerController playerController;
    private float timeToStopBoxing;

    private void Start()
    {
        playerController = PlayerController.playerControllerInstance;
    }

    private void Update()
    {
        DrawDebugRays();
        CheckIfSomethingToHit();
    }

    private void DrawDebugRays()
    {
        Debug.DrawRay(transform.position + transform.up * heightRay, transform.forward * hitRange, Color.black);
        Debug.DrawRay(transform.position + transform.up * heightRay + transform.right * sidesRayDistance, transform.forward * hitRange, Color.black);
        Debug.DrawRay(transform.position + transform.up * heightRay - transform.right * sidesRayDistance, transform.forward * hitRange, Color.black);
    }

    private void CheckIfSomethingToHit()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position + transform.up * heightRay, transform.forward, out hit, hitRange, hitableLayer) ||
            Physics.Raycast(transform.position + transform.up * heightRay + transform.right * sidesRayDistance, transform.forward, out hit, hitRange, hitableLayer) ||
            Physics.Raycast(transform.position + transform.up * heightRay - transform.right * sidesRayDistance, transform.forward, out hit, hitRange, hitableLayer))
        {
            playerController.charaAnimator.SetBool("Boxing", true);

            if(hit.transform.CompareTag("Bot"))
            {
                hit.collider.enabled = false;
                hit.transform.GetComponent<Bot>().BoxedByPlayer();
                GlovesStacking.glovesStackingInstance.GetGloveSizeDown();
            }
            timeToStopBoxing = 0;
        }
        else
        {
            timeToStopBoxing += Time.deltaTime;

            if(timeToStopBoxing >= 0.75f)
                playerController.charaAnimator.SetBool("Boxing", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public float impactForce = 10f;
    public Animator botAnimator;

    [HideInInspector]public Rigidbody[] ragdollRbs;

    private bool dead;

    private void Start()
    {
        ragdollRbs = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = true;
        }
    }

    public void BoxedByPlayer()
    {
        if(!dead)
        {
            botAnimator.enabled = false;

            Vector3 explosionPos = transform.position + transform.forward * Random.Range(-1f, 1f) + transform.right * Random.Range(-1f, 1f);

            foreach (Rigidbody rb in ragdollRbs)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(impactForce * Random.Range(1f, 1.5f), explosionPos, 2f, 1f);
            }

            MoneyManager.moneyManagerInstance.WinCoinDuringLevel();

            dead = true;
        }
    }
}

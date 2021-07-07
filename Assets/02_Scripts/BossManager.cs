using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class BossManager : MonoBehaviour
{
    public static BossManager bossManagerInstance;

    [Header("Parameters")]
    public float timeToGoToBoss = 1.5f;
    public float bossScaleDownByHit = 0.2f;
    public float cdBetweenHits = 0.3f;

    [Header("Variables")]
    public Transform playerTransform;
    public Transform newPosChara;
    public Transform bossVisual;
    public Animator playerAnimator;
    public Animator bossAnimator;
    public Bot boss;
    public ParticleSystem confettisFX;

    [HideInInspector]public bool fighting;
    private bool left;

    private float currentHitCd;

    public void Awake()
    {
        bossManagerInstance = this;
    }

    private void Update()
    {
        currentHitCd += Time.deltaTime;

        if(fighting)
        {
            if(Input.GetMouseButtonDown(0) && currentHitCd >= cdBetweenHits)
            {
                if(!left)
                {
                    playerAnimator.SetTrigger("Left");
                    left = true;
                }
                else
                {
                    playerAnimator.SetTrigger("Right");
                    left = false;
                }

                HitBoss();
            }
        }
    }

    public void HitBoss()
    {
        bossVisual.localScale -= new Vector3(bossScaleDownByHit, bossScaleDownByHit, bossScaleDownByHit);

        if(bossVisual.localScale.x <= 1f)
        {
            boss.BoxedByPlayer();
            fighting = false;
            Invoke("BossFinished", 1f);
            return;
        }

        MoneyManager.moneyManagerInstance.WinCoinDuringLevel();
        GlovesStacking.glovesStackingInstance.GetGloveSizeDown();
        bossAnimator.SetTrigger("Hit");
        currentHitCd = 0;
    }

    public void BossFinished()
    {
        LevelManager.levelManagerInstance.Win();
    }

    public void GoToBoss()
    {
        StartCoroutine("GoToBossCoroutine");
    }

    IEnumerator GoToBossCoroutine()
    {
        confettisFX.Play();
        playerAnimator.SetBool("Running", false);
        playerAnimator.SetBool("Victory", true);
        MMVibrationManager.Haptic(HapticTypes.Success);

        yield return new WaitForSeconds(1.4f);

        playerAnimator.SetBool("Victory", false);
        playerAnimator.SetBool("Walk", true);

        Vector3 initialPlayerPos = playerTransform.position;
        Vector3 newPos = new Vector3(newPosChara.position.x, playerTransform.position.y, newPosChara.position.z);
        var wait = new WaitForFixedUpdate();

        for(float f = 0; f < timeToGoToBoss; f += Time.deltaTime)
        {
            playerTransform.position = Vector3.Lerp(initialPlayerPos, newPos, f / timeToGoToBoss);
            yield return wait;
        }

        playerTransform.position = newPos;
        StartBossFight();
    }

    public void StartBossFight()
    {
        playerAnimator.SetBool("Walk", false);
        playerAnimator.SetBool("IdleBoxing", true);
        fighting = true;
    }
}

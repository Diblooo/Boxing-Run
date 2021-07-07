using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class GlovesStacking : MonoBehaviour
{
    public static GlovesStacking glovesStackingInstance;

    [Header("Parameters")]
    public float miniGlovesSize;
    public float maxGlovesSize;
    public float sizeUpPerCollectible;

    [Header("Variables")]
    public Transform rightGlove;
    public Transform leftGlove;
    public Transform fxUIRoot;

    private void Awake()
    {
        glovesStackingInstance = this;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.CompareTag("Glove"))
        {
            hit.gameObject.SetActive(false);

            GameObject fx = ObjectPooler.objectPoolerInstance.GetPooledObject("SwordHitPurple");
            fx.transform.position = hit.transform.position;
            fx.SetActive(true);

            GameObject fxUI = ObjectPooler.objectPoolerInstance.GetPooledObject("+1");
            float randomXPos = Random.Range(-Screen.width / 8f, Screen.width / 8f);
            float randomYPos = Random.Range(-Screen.height / 20f, Screen.height / 20f);
            fxUI.transform.position = fxUIRoot.position + new Vector3(randomXPos, randomYPos, 0);
            fxUI.SetActive(true);

            GetGloveSizeUp();
        }
    }

    private void GetGloveSizeUp()
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact);

        rightGlove.transform.localScale += new Vector3(sizeUpPerCollectible, sizeUpPerCollectible, sizeUpPerCollectible);
        leftGlove.transform.localScale += new Vector3(sizeUpPerCollectible, sizeUpPerCollectible, sizeUpPerCollectible);

        ClampGloveSize();
    }

    public void GetGloveSizeDown()
    {
        rightGlove.transform.localScale -= (new Vector3(sizeUpPerCollectible, sizeUpPerCollectible, sizeUpPerCollectible)) / 1.5f;
        leftGlove.transform.localScale -= (new Vector3(sizeUpPerCollectible, sizeUpPerCollectible, sizeUpPerCollectible)) / 1.5f;

        ClampGloveSize();
    }

    private void ClampGloveSize()
    {
        if(rightGlove.transform.localScale.x < miniGlovesSize)
        {
            rightGlove.transform.localScale = new Vector3(miniGlovesSize, miniGlovesSize, miniGlovesSize);
            leftGlove.transform.localScale = new Vector3(miniGlovesSize, miniGlovesSize, miniGlovesSize);

            if (!BossManager.bossManagerInstance.fighting)
                LevelManager.levelManagerInstance.Lose();
            else
            {
                BossManager.bossManagerInstance.fighting = false;
                LevelManager.levelManagerInstance.Win();
            }

        }
        else if(rightGlove.transform.localScale.x > maxGlovesSize)
        {
            rightGlove.transform.localScale = new Vector3(maxGlovesSize, maxGlovesSize, maxGlovesSize);
            leftGlove.transform.localScale = new Vector3(maxGlovesSize, maxGlovesSize, maxGlovesSize);
        }
    }
}

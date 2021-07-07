using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class OnMoneyWon : MonoBehaviour
{
    public Vector3 finalPos;

    void Start()
    {
        StartCoroutine("GoToCurrentMoneyIcon");
    }

    IEnumerator GoToCurrentMoneyIcon()
    {
        var wait = new WaitForEndOfFrame();

        float timeToWait = Random.Range(0.5f, 1f);
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

        for (float f = 0; f < timeToWait; f += Time.deltaTime)
        {
            transform.Translate(randomDirection.normalized * 30f * Time.deltaTime);
            yield return wait;
        }

        Vector3 initialPos = transform.position;
        float timeToGo = 0.5f;

        for (float f = 0; f < timeToGo; f += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(initialPos, finalPos, f / timeToGo);
            yield return wait;
        }

        transform.position = finalPos;

        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        MoneyManager.moneyManagerInstance.coinIconAnimator.SetTrigger("Pop");
        MoneyManager.moneyManagerInstance.VisualMoney++;

        gameObject.SetActive(false);
    }
}
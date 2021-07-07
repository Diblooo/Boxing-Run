using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidFreezeFirstFx : MonoBehaviour
{
    //This script is only to avoid the weird freeze when the first FX appears

    GameObject fx;
    GameObject fxUI;

    private void Start()
    {
        fx = ObjectPooler.objectPoolerInstance.GetPooledObject("SwordHitPurple");
        fx.SetActive(true);

        fxUI = ObjectPooler.objectPoolerInstance.GetPooledObject("+1");
        fxUI.SetActive(true);

        Invoke("DisableFX", 0.2f);
    }

    private void DisableFX()
    {
        fx.SetActive(false);
        fxUI.SetActive(false);
    }
}

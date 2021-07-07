using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance = null;

    [Header("Parameters")]
    public float timingDoubleClick = 0.25f;
    public bool resetOnDoubleClick = true;

    [Header("Variables")]
    public Animator blackScreenAnimator;

    private float lastCLickTime;
    private bool alreadyLoadingScene = false;

    private void Awake()
    {
        gameManagerInstance = this;
    }

    private void Update()
    {
        if(resetOnDoubleClick)
            ResetSceneOnDoubleClick();
    }

    void ResetSceneOnDoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastCLickTime < timingDoubleClick && !alreadyLoadingScene && Input.mousePosition.y > Screen.height - Screen.height / 4)
            {
                PlayerPrefs.DeleteAll();
                StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
                alreadyLoadingScene = true;
            }
            lastCLickTime = Time.time;
        }
    }

    public void ResetScene()
    {
        if(!alreadyLoadingScene)
        {
            StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
            alreadyLoadingScene = true;
        }
    }

    public void FullReset()
    {
        if (!alreadyLoadingScene)
        {
            PlayerPrefs.DeleteAll();
            StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
            alreadyLoadingScene = true;
        }
    }

    public IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asynLoadScene = SceneManager.LoadSceneAsync(sceneIndex);
        asynLoadScene.allowSceneActivation = false;
        blackScreenAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(0.75f);

        while(asynLoadScene.progress < 0.9f)
        {
            yield return null;
        }
        asynLoadScene.allowSceneActivation = true;
        alreadyLoadingScene = false;
    }
}

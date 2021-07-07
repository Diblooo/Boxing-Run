using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float speedCam = 5f;
    public Transform characterTransform;

    private Vector3 newPos;

    void FixedUpdate()
    {
        FocusCharacter();
    }

    void FocusCharacter()
    {
        newPos = new Vector3(characterTransform.position.x, characterTransform.position.y, characterTransform.position.z);
        gameObject.transform.position = Vector3.Lerp(transform.position, newPos, speedCam * Time.deltaTime);
    }
}

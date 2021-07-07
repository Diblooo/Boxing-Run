using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController playerControllerInstance;

    [Header("Parameters")]
    public float playerForwardSpeed;
    public float playerSideSpeed;
    public float playerGravity;
    public float clampValueSides;

    [Header("Variables")]
    public Animator charaAnimator;

    private CharacterController characterController;
    private Vector3 movement = new Vector3();
    private float progressiveForwardValue;

    private bool firstTouch;

    private void Awake()
    {
        playerControllerInstance = this;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!firstTouch)
        {
            if (Input.GetMouseButtonDown(0))
                FirstTouch();
        }
    }

    private void FixedUpdate()
    {
        if(firstTouch && !LevelManager.levelManagerInstance.levelFinished)
            CharacterMoves();
    }

    private void FirstTouch()
    {
        firstTouch = true;
        charaAnimator.SetBool("Running", true);
    }

    private void CharacterMoves()
    {
        //Si le joueur est au sol on reset complétement son déplacement à chaque frame, sinon on reset seulement les mouvements X et Z
        // --> On fait ça pour éviter d'être affecté par la physique
        if (characterController.isGrounded && movement.y < 0)
            movement = Vector3.zero;
        else
            movement = new Vector3(0, movement.y, 0);

        float yMovementValue = playerGravity * Time.fixedDeltaTime;
        movement += new Vector3(0, yMovementValue, 0);

        progressiveForwardValue = Mathf.Clamp01(progressiveForwardValue + Time.fixedDeltaTime * 2);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchMovement = touch.deltaPosition;

            movement += new Vector3(playerSideSpeed * touchMovement.x / Screen.width, 0, playerForwardSpeed * progressiveForwardValue * Time.fixedDeltaTime);
        }
        else
        {
            movement += new Vector3(0, 0, playerForwardSpeed * progressiveForwardValue * Time.fixedDeltaTime);
        }

        characterController.Move(movement);
        ClampPosition();
    }

    private void ClampPosition()
    {
        if (transform.position.x > clampValueSides)
            transform.position = new Vector3(clampValueSides, transform.position.y, transform.position.z);
        else if (transform.position.x < -clampValueSides)
            transform.position = new Vector3(-clampValueSides, transform.position.y, transform.position.z);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.CompareTag("End"))
        {
            hit.collider.enabled = false;      
            BossManager.bossManagerInstance.GoToBoss();
            enabled = false;
        }
    }
}

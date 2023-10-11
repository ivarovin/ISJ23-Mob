using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class Player_Interactor : MonoBehaviour
{
    public static Player_Interactor instance;

    [Header("[References]")] [SerializeField]
    private Player.SimpleMovement playerMovement;

    [Header("[Configuration]")] [SerializeField]
    private LayerMask interactableLayer;

    [SerializeField] private float rayLenght;
    [SerializeField] private bool interacting;

    private GameObject alertObj;

    private void Awake()
    {
        CreateSingleton();
    }

    private void CreateSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    private void Update()
    {
        if (!interacting && IsInteractionRequested())
        {
            Interact();
        }

        ActiveStuffs();
        DrawSight();
    }

    bool IsInteractionRequested() =>
        Input.GetKeyDown(KeyCode.Space) &&
        GameStateController.Instance.gameState == GameStateController.GameState.Gameplay;


    private void Interact()
    {
        if (!TryGetInteractable(out var interactable)) return;

        interacting = true;
        interactable.Interact();
    }

    bool TryGetInteractable(out IInteractable interactable)
    {
        interactable = ElementInFront().transform.gameObject.GetComponent<IInteractable>();
        return interactable != null;
    }

    RaycastHit2D ElementInFront()
    {
        return Physics2D.Raycast(gameObject.transform.position, FacingDirection(), rayLenght, interactableLayer);
    }

    void DrawSight()
    {
        Debug.DrawLine(transform.position, transform.position + (FacingDirection() * rayLenght), Color.red);
    }

    Vector3 FacingDirection()
    {
        return new Vector3(playerMovement.faceDirection.x, playerMovement.faceDirection.y);
    }

    private void ActiveStuffs()
    {
        if (ElementInFront().collider != null)
        {
            GameObject newAlertObj = ElementInFront().transform.GetChild(0).gameObject;

            if (!newAlertObj.activeSelf)
            {
                newAlertObj.SetActive(true);
                alertObj = newAlertObj; // Actualiza la referencia
            }
        }
        else
        {
            // Si el objeto está activado, desactívalo y actualiza la variable booleana
            if (alertObj != null && alertObj.activeSelf)
            {
                alertObj.SetActive(false);
            }
        }
    }

    public void EnableInteracting()
    {
        StartCoroutine(Coroutine_EnableInteracting());

        IEnumerator Coroutine_EnableInteracting()
        {
            yield return new WaitForSeconds(0.25f);
            interacting = false;
        }
    }
}
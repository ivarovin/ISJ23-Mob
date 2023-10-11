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

    private GameObject lastAlert;

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

        ActiveAlert();
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

    Vector3 FacingDirection() => playerMovement.faceDirection;

    private void ActiveAlert()
    {
        if (ElementInFront().collider != null)
        {
            HighlightNewAlert();
        }
        else
        {
            DisableCurrentAlert();
        }
    }

    void DisableCurrentAlert() => lastAlert?.SetActive(false);

    void HighlightNewAlert()
    {
        lastAlert = AlertInFront();
        lastAlert.SetActive(true);
    }

    GameObject AlertInFront() => ElementInFront().transform.GetChild(0).gameObject;

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
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
        if (interacting == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) &&
                GameStateController.Instance.gameState == GameStateController.GameState.Gameplay)
            {
                Interact();
            }
        }

        ActiveStuffs();
    }

    private GameObject alertObj;

    private void Interact()
    {
        var facingDirection = new Vector3(playerMovement.faceDirection.x, playerMovement.faceDirection.y);

        RaycastHit2D hit =
            Physics2D.Raycast(gameObject.transform.position, facingDirection, rayLenght, interactableLayer);

        if (hit.collider != null)
        {
            interacting = true;
            hit.transform.gameObject.GetComponent<IInteractable>().Interact();
        }

        Debug.DrawLine(transform.position, transform.position + (facingDirection * rayLenght), Color.red);
    }

    private void ActiveStuffs()
    {
        var facingDirection = new Vector3(playerMovement.faceDirection.x, playerMovement.faceDirection.y);

        RaycastHit2D hit =
            Physics2D.Raycast(gameObject.transform.position, facingDirection, rayLenght, interactableLayer);

        if (hit.collider != null)
        {
            GameObject newAlertObj = hit.transform.GetChild(0).gameObject;

            // Si el objeto aún no está activado, actívalo y actualiza la variable booleana
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

        Debug.DrawLine(transform.position, transform.position + (facingDirection * rayLenght), Color.red);
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
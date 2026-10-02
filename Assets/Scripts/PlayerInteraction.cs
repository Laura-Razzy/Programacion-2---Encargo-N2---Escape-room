using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private UIManager uiManager;
    private Item item;
    private LayerMask itemDetection;
    [SerializeField] private float playerRayDistance = 10f;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        uiManager = GetComponent<UIManager>();
        itemDetection = LayerMask.GetMask("Wall", "Item");
    }

    void Update()
    {
        // Detecta los layers "Player" y "Wall", luego revisa si se esta mirando a "Player";
        if (Physics.Raycast(playerMovement.camTransform.position, playerMovement.camTransform.forward, out RaycastHit hit, playerRayDistance, itemDetection))
        {
            Debug.DrawRay(playerMovement.camTransform.position, playerMovement.camTransform.forward, Color.green);
            if (hit.transform.gameObject.layer == 7) // 7 es el layer "Item"
            {
                uiManager.SetInteractPrompt(UIManager.interactionState.PickUp); // Cambia el texto del prompt a "Pick Up"
                if (Input.GetKeyDown("f")) // Si apreta f
                {
                    uiManager.itemSave = hit.transform.GetComponent<Item>().itemData;
                    GetComponent<PlayerInventory>().saveInInventory();
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        else
        {
            Debug.DrawRay(playerMovement.camTransform.position, playerMovement.camTransform.forward, Color.red);
            uiManager.SetInteractPrompt(UIManager.interactionState.None);
        }
    }
}

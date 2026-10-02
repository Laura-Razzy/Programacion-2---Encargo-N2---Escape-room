using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private UIManager uiManager;
    private Item item;
    private LayerMask itemDetection;
    public bool PotIsReady = false, lookingAtPot = false, potisReady = false;
    [SerializeField] private float playerRayDistance = 10f;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        uiManager = GetComponent<UIManager>();
        itemDetection = LayerMask.GetMask("Wall", "Item", "Interactable");
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
            else if (hit.transform.gameObject.layer == 8) // 8 es el layer "Interactable"
            {
                if (potisReady == true)
                {
                    uiManager.SetInteractPrompt(UIManager.interactionState.Interact); // Cambia el texto del prompt a "Interact"
                }
                else
                {
                    uiManager.SetInteractPrompt(UIManager.interactionState.None);
                }
                
                if (hit.transform.gameObject.name == "Pot")
                {
                    if (Input.GetKeyDown("f") && potisReady == true)
                    {
                        uiManager.interactionPrompt.text = "I did it! I made the potion!";
                    }
                    else if (Input.GetKeyDown("f") && potisReady == false)
                    {
                        uiManager.interactionPrompt.text = "I need to add 3 different potions to the cauldron.";
                    }
                    lookingAtPot = true;
                }
                else
                {
                    lookingAtPot = false;
                }
            }
            else
            {
                Debug.DrawRay(playerMovement.camTransform.position, playerMovement.camTransform.forward, Color.red);
                uiManager.SetInteractPrompt(UIManager.interactionState.None);
                lookingAtPot = false;
            }
        }
    }
}
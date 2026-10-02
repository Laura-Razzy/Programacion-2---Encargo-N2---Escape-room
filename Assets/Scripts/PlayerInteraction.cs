using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private UIManager uiManager;
    private Item item;
    private LayerMask itemDetection;
    [SerializeField] private GameObject KeyPrefab;
    public bool lookingAtPot = false, lookingAtDoor = false, potIsReady = false, UltimatePower = false;
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
                lookingAtPot = false;
                lookingAtDoor = false;
                uiManager.SetInteractPrompt(UIManager.interactionState.PickUp); // Cambia el texto del prompt a "Pick Up"
                if (Input.GetKeyDown("f")) // Si apreta f
                {
                    uiManager.itemSave = hit.transform.GetComponent<Item>().itemData;
                    GetComponent<PlayerInventory>().saveInInventory();
                    Destroy(hit.transform.gameObject);
                    uiManager.SetInteractPrompt(UIManager.interactionState.None); // Cambia el texto del prompt a "None"
                }
            }
            else if (hit.transform.gameObject.layer == 8) // 8 es el layer "Interactable"
            {
                uiManager.SetInteractPrompt(UIManager.interactionState.Interact);
                if (hit.transform.gameObject.name == "Pot") // Si el objeto que se esta mirando es la olla
                {
                    lookingAtPot = true;
                    lookingAtDoor = false;
                }
                else if (hit.transform.gameObject.name == "Door") // Si el objeto que se esta mirando es la puerta
                {
                    lookingAtDoor = true;
                    lookingAtPot = false;
                }
                else // Si no esta mirando ni la olla ni la puerta
                {
                    uiManager.SetInteractPrompt(UIManager.interactionState.None);
                    lookingAtPot = false;
                    lookingAtDoor = false;
                }
            }
            else // Si no esta mirando nada interactuable
            {
                Debug.DrawRay(playerMovement.camTransform.position, playerMovement.camTransform.forward, Color.red);
                uiManager.SetInteractPrompt(UIManager.interactionState.None);
                lookingAtPot = false;
                lookingAtDoor = false;
            }
        }
        else // Si no esta mirando nada
        {
            uiManager.SetInteractPrompt(UIManager.interactionState.None);
            lookingAtPot = false;
            lookingAtDoor = false;
        }
    }
    void OnTriggerEnter(Collider other)
    // Detecta si el jugador colisiona con el enemigo, si tiene la ultimate potion lo mata y spawnea la llave, si no, muere.
    {
        if (other.gameObject.name == "Enemy")
        {
            if (UltimatePower == true)
            {
                Instantiate(KeyPrefab, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
            else
            {
                uiManager.loseScreen.SetActive(true);
            }
        }
    }
}
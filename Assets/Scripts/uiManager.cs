using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class uiManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Item item;
    private LayerMask itemDetection;
    [SerializeField] private float playerRayDistance = 10f;
    [SerializeField] TextMeshProUGUI interactionPrompt;
    public ItemScriptable itemSave = null;

    void Awake()
    {
        interactionPrompt = GameObject.Find("Interact").GetComponent<TextMeshProUGUI>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        interactionPrompt.text = null;
        itemDetection = LayerMask.GetMask("Wall", "Item");
    }

    void Update()
    {
        if (Physics.Raycast(playerMovement.camTransform.position, playerMovement.camTransform.forward, out RaycastHit hit, playerRayDistance, itemDetection))
        {
            Debug.DrawRay(playerMovement.camTransform.position, playerMovement.camTransform.forward, Color.green);
            if (hit.transform.gameObject.layer == 7)
            {
                interactionPrompt.text = "[E] Pick Up";
                if (Input.GetKeyDown("e"))
                {
                    itemSave = hit.transform.GetComponent<Item>().itemData;
                    GetComponent<PlayerInventory>().saveInInventory();
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        else
        {
            Debug.DrawRay(playerMovement.camTransform.position, playerMovement.camTransform.forward, Color.red);
            interactionPrompt.text = null;
        }
    }
}
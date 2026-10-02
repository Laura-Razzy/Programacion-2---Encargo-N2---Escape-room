using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerInventory playerInventory;
    public TextMeshProUGUI interactionPrompt, textBox;
    public ItemScriptable itemSave = null;
    public GameObject pauseMenu;
    public enum interactionState {None, PickUp, Interact};
    public interactionState currentInteractionState = interactionState.None;

    void Awake()
    {
        pauseMenu.SetActive(true);
        interactionPrompt = GameObject.Find("Interact").GetComponent<TextMeshProUGUI>();
        textBox = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    void Start()
    {
        interactionPrompt.text = null;
        textBox.text = null;
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf == true)
            {
                GetInventory();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
            }
        }
    }

    public void SetInteractPrompt(interactionState uiStateUpdate) // Cambia el texto del prompt dependiendo de la accion que se pueda hacer.
    {
        switch (uiStateUpdate)
        {
            case interactionState.PickUp:
                interactionPrompt.text = "[F] Pick Up";
                currentInteractionState = interactionState.PickUp;
                break;
            case interactionState.Interact:
                interactionPrompt.text = "[F] Interact";
                currentInteractionState = interactionState.Interact;
                break;
            case interactionState.None:
                interactionPrompt.text = null;
                currentInteractionState = interactionState.None;
                break;
        }
    }

    public void GetInventory()
    {
        for (int i = 0; i < playerInventory.inventory.Length; i++)
        {
            string slotName = $"Slot {i + 1}";
            if (playerInventory.inventory[i] != null)
            {
                GameObject.Find(slotName).GetComponent<Image>().color = playerInventory.inventory[i].itemMaterial.color;
            }
            else
            {
                GameObject.Find(slotName).GetComponent<Image>().color = Color.black;
            }
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private PlayerInteraction playerInteraction;
    public TextMeshProUGUI interactionPrompt, textBox;
    public ItemScriptable itemSave = null, itemLoad = null;
    public GameObject pauseMenu, winScreen, loseScreen;
    public enum interactionState {None, PickUp, Interact};
    public interactionState currentInteractionState = interactionState.None;

    void Awake()
    {
        pauseMenu.SetActive(true);
        winScreen.SetActive(true);
        loseScreen.SetActive(true);
        interactionPrompt = GameObject.Find("Interact").GetComponent<TextMeshProUGUI>();
        textBox = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        playerInventory = GetComponent<PlayerInventory>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    void Start()
    {
        interactionPrompt.text = null;
        textBox.text = null;
        pauseMenu.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (winScreen.activeSelf == false && loseScreen.activeSelf == false)
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
        else if (winScreen.activeSelf == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else if (loseScreen.activeSelf == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
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
                interactionPrompt.text = "There's probably something you can do here...";
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
    public void ShowItemDescription(int slot)
    {
        if (playerInventory.inventory[slot] != null)
        {
            textBox.text = playerInventory.inventory[slot].itemDescription;
        }
        else
        {
            textBox.text = "An empty slot...";
        }
    }

    public void HideItemDescription()
    {
        textBox.text = null;
    }

    public void ClickItem(int index)
    {
        if (playerInventory.inventory[index] != null)
        {
            if (playerInteraction.lookingAtPot == true && playerInteraction.potIsReady == false)
            {
                itemLoad = playerInventory.inventory[index];
                GameObject.Find("Pot").GetComponent<PuzzleManager>().AddPotion(index);
                GameObject.Find($"Slot {index + 1}").GetComponent<Image>().color = Color.black;
            }
            else if (playerInventory.inventory[index].itemName == "Ultimate Potion")
            {
                playerInteraction.UltimatePower = true;
                playerInventory.inventory[index] = null;
                GetInventory();
            }
            else if (playerInventory.inventory[index].itemName == "Key" && playerInteraction.lookingAtDoor == true)
            {
                Destroy(GameObject.Find("Door"));
                playerInventory.inventory[index] = null;
                GetInventory();
                pauseMenu.SetActive(false);
                winScreen.SetActive(true);
            }
        }
    }
}
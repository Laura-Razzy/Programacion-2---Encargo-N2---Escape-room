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
        pauseMenu.SetActive(true); // prendo los 3 menus al despertar para que todos los componentes se asignen correctamente.
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
        loseScreen.SetActive(false); // apago los 3 menus cuando inicia el juego, los componentes estan puestos y guardados.
        Time.timeScale = 1f; // Bugfix: cuando empezaba una partida despues de que el juego paraba, terminaba conjelado.
    }

    void Update()
    {
        if (winScreen.activeSelf == false && loseScreen.activeSelf == false) // Revisa que el juego no este en otro menu para poder abrir el inventario y pausa.
        {
            if (Input.GetKeyDown("e")) // Abre el inventario/pause
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
        else if (winScreen.activeSelf == true) // Se activa desde linea 147 si abres la puerta con la llave.
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else if (loseScreen.activeSelf == true) // Se activa desde Playerinteractions (linea 85) si choca con el enemigo sin ultimate potion.
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

    public void GetInventory() // Actualiza el menu basado en el inventario
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
    // Esta funcion la llaman los GameObjects directamente desde el inspector de Unity cuando se les pone el mouse encima.
    // Si el slot tiene algo, se muestra su descripcion del scriptable object, si no, se muestra que esta vacio.
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
    // Esta funcion la llaman los GameObjects directamente desde el inspector de Unity cuando se les saca el mouse de encima.
    // Cuando el mouse no esta encima de uno de los slots... no aprace una descripcion!
    {
        textBox.text = null;
    }

    public void ClickItem(int index)
    // Esta funcion la llaman los GameObjects directamente desde el inspector de Unity cuando se clickean.
    // Esta funcion esta muy mal hecha, dios me deberia castigar por traer esta abominacion sobre la faz de la tierra.
    // Si crees en el bien y las cosas buenas, no programes asi.
    {
        if (playerInventory.inventory[index] != null)
        {
            if (playerInteraction.lookingAtPot == true && playerInteraction.potIsReady == false)
            // Si estoy mirando a la olla cuando clickeo y no se ha resuelto el puzle todavia.
            // Maneja cosas de meter pociones a la olla y sacarlas del inventario.
            {
                itemLoad = playerInventory.inventory[index];
                GameObject.Find("Pot").GetComponent<PuzzleManager>().AddPotion(index);
                GameObject.Find($"Slot {index + 1}").GetComponent<Image>().color = Color.black;
            }
            else if (playerInventory.inventory[index].itemName == "Ultimate Potion")
            // Si clickeo en la "Ultimate Potion" (Reviso si ese es el nombre del scriptable object en lo clikeado.)
            // Me da el bufo de Ultimate Power y la saca del inventario.
            {
                playerInteraction.UltimatePower = true;
                playerInventory.inventory[index] = null;
                GetInventory();
            }
            else if (playerInventory.inventory[index].itemName == "Key" && playerInteraction.lookingAtDoor == true)
            {
            // Si estoy mirando la puerta cuando clickeo en la llave
            // Destruye la puerta, saca la llave del inventario, y gano el juego!!
                Destroy(GameObject.Find("Door"));
                playerInventory.inventory[index] = null;
                GetInventory();
                pauseMenu.SetActive(false);
                winScreen.SetActive(true);
            }
        }
    }
}
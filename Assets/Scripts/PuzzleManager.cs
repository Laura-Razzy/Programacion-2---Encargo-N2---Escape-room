using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private UIManager uiManager;
    private PlayerInventory playerInventory;
    private PlayerInteraction playerInteraction;
    public GameObject UltimatePotionPrefab;
    public bool puzzleCompleted = false;
    public ItemScriptable[] potions = new ItemScriptable[3];

    void Start()
    {
        uiManager = GameObject.Find("Player").GetComponent<UIManager>();
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
        playerInteraction = GameObject.Find("Player").GetComponent<PlayerInteraction>();
    }

    public void AddPotion(int slot)
    {
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i] == null)
            {
                potions[i] = uiManager.itemLoad; // Mete una pocion en la olla
                break;
            }
        }
        playerInventory.inventory[slot] = null; // Saca la pocion que pusiste del inventario
        uiManager.GetInventory();
        if (potions[0] != null && potions[1] != null && potions[2] != null) // Si hay tres pociones en la olla...
        {
            if (potions[0] != potions[1] && potions[0] != potions[2] && potions[1] != potions[2]) // y son las tres distintas...
            {
                playerInteraction.potIsReady = true;
                GameObject UltimatePotion = Instantiate(UltimatePotionPrefab); // Creamos la ultimate potion
            }
            else // Si hay dos pociones iguales, se resetea el puzle de la olla.
            {
                for (int i = 0; i < potions.Length; i++)
                {
                    potions[i] = null;
                }
                uiManager.interactionPrompt.text = "I messed up the potion... I need to try again.";
            }
        }
    }
}

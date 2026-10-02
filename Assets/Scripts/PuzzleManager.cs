using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private UIManager uiManager;
    private PlayerInventory playerInventory;
    private PlayerInteraction playerInteraction;
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
                potions[i] = uiManager.itemLoad;
                break;
            }
        }
        playerInventory.inventory[slot] = null;
        uiManager.GetInventory();
        if (potions[0] != null && potions[1] != null && potions[2] != null)
        {
            if (potions[0] != potions[1] && potions[0] != potions[2] && potions[1] != potions[2])
            {
                playerInteraction.potisReady = true;
            }
            else
            {
                uiManager.interactionPrompt.text = "I messed up the potion... I need to try again.";
            }
        }
    }
}

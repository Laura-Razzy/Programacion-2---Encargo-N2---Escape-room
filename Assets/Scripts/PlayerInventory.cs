using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private LayerMask layerDetection;
    [SerializeField] private ItemScriptable[] inventory = new ItemScriptable[3];

    public void saveInInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = GetComponent<uiManager>().itemSave;
                break;
            }
        }
    }
}

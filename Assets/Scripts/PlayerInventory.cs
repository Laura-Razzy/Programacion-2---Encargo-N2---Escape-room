using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private LayerMask layerDetection;
    [SerializeField] private ItemScriptable[] inventory = new ItemScriptable[3];

    public void saveInInventory() // Busca la ultima variable guardada en uiManager.cs y la guarda en el slot mas bajo del inventario.
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

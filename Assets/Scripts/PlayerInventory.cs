using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public ItemScriptable[] inventory = new ItemScriptable[3];

    public void saveInInventory() // Busca la ultima variable guardada en UIManager.cs y la guarda en el slot mas bajo del inventario.
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = GetComponent<UIManager>().itemSave;
                break;
            }
        }
    }
}

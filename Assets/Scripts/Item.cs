using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemScriptable itemData;
    void Start() // Toma los datos de los Scriptable Objects y los aplica a las componentes del item.
    {
        gameObject.name = itemData.itemName;
        gameObject.GetComponent<MeshRenderer>().material = itemData.itemMaterial;
        gameObject.transform.position = itemData.itemPosition;
    }
}

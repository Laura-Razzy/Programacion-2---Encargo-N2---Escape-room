using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemScriptable itemData;
    public void Start()
    {
        gameObject.name = itemData.itemName;
        gameObject.GetComponent<MeshRenderer>().material = itemData.itemMaterial;
        gameObject.transform.position = itemData.itemPosition;
    }
}

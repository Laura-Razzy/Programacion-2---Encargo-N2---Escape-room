using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptables/Item")]
public class ItemScriptable : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Material itemMaterial;
    public Vector3 itemPosition;
}

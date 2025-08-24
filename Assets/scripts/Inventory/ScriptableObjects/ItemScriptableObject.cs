using UnityEngine;

public enum ItemType
{
    Default, Food
}
public class ItemScriptableObject : ScriptableObject
{
    public string ItemName;
    public int MaxAmount;
    public GameObject ItemPrefab;
    public Sprite Icon;
    public ItemType itemType;
}

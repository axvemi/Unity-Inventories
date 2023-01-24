using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Data class for an item
    /// </summary>
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Axvemi/Inventories/Inventory Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public string Id;
        
        [Header("Data")]
        public int MaxAmount;
        
        [Header("Visuals")]
        public Sprite Sprite;

        public override string ToString()
        {
            return "Id: " + Id;
        }
    }
}

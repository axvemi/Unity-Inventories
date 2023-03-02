using UnityEngine;
using UnityEngine.Serialization;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Data class for an item
    /// </summary>
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Axvemi/Inventories/Inventory Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        [FormerlySerializedAs("Id")] public string id;
        
        [FormerlySerializedAs("MaxAmount")] [Header("Data")]
        public int maxAmount;
        
        [FormerlySerializedAs("Sprite")] [Header("Visuals")]
        public Sprite sprite;

        public override string ToString()
        {
            return "Id: " + id;
        }
    }
}

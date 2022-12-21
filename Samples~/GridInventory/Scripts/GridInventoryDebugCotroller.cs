using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    public class GridInventoryDebugCotroller : MonoBehaviour
    {
        [SerializeField] private GridInventorySampleManager inventorySampleManager;

        private Inventory<Item> inventory => inventorySampleManager.Inventory;

        public void AddInventorySlot() 
        {
            inventory.CreateNewSlot();
        }

        public void RemoveInventorySlot()
        {
            if (inventory.Slots.Count == 0) return;
            inventory.RemoveSlot(inventory.Slots[inventory.Slots.Count - 1]);
        }

        public void AddItem(ItemScriptableObject itemScriptableObject)
        {
            inventory.AddItem(new Item(itemScriptableObject));
        }
    }
}

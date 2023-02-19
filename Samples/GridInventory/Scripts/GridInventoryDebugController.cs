using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    public class GridInventoryDebugController : MonoBehaviour
    {
        [SerializeField] private GridInventorySampleManager inventorySampleManager;

        private Inventory<Item> Inventory => inventorySampleManager.Inventory;

        public void AddInventorySlot() 
        {
            Inventory.CreateNewSlot();
        }

        public void RemoveInventorySlot()
        {
            if (Inventory.Slots.Count == 0) return;
            Inventory.RemoveSlot(Inventory.Slots[Inventory.Slots.Count - 1]);
        }

        public void AddItem(ItemScriptableObject itemScriptableObject)
        {
            Inventory.AddItem(new Item(itemScriptableObject));
        }
    }
}

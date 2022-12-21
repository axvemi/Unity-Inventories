using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Class that creates an slot gameObject for each slot in the inventory.
    /// </summary>
    public class GridInventoryUIController : MonoBehaviour
    {
        [SerializeField] private GameObject inventorySlotPrefab = null;
        [Header("UI")]
        [SerializeField] private Transform slotsContainer = null;

        private Inventory<Item> inventory = null;

        public Inventory<Item> Inventory { get => inventory; set => inventory = value; }

        private void Start() {
            //Create an slot for each inventory slot
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                GridInventorySlotUIController inventorySlotUIInstance = Instantiate(inventorySlotPrefab, slotsContainer).GetComponent<GridInventorySlotUIController>();
                inventorySlotUIInstance.Slot = inventory.Slots[i];
            }
        }
    }
}

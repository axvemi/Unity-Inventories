using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    public class GridInventorySampleManager : MonoBehaviour
    {
        public Inventory<Item> inventory;

        [Header("Data")]
        [SerializeField] private ItemScriptableObject swordScriptableObject;
        [SerializeField] private ItemScriptableObject redPotionScriptableObject;
        [SerializeField] private ItemScriptableObject bluePotionScriptableObject;
        [SerializeField] private ItemScriptableObject woodenLogPotionScriptableObject;
        [SerializeField] private ItemScriptableObject woolPotionScriptableObject;

        [Header("Inventory")]
        [SerializeField] private int slotAmount;

        [Header("UI")]
        [SerializeField] private GridInventoryUIController inventoryUIController;

        private void Awake()
        {
            inventory = new Inventory<Item>(slotAmount);
            inventoryUIController.Inventory = inventory;
        }

        private void Start()
        {
            //ADD DEMO ITEMS
            inventory.AddItem(new Item(swordScriptableObject));
            inventory.AddItem(new Item(swordScriptableObject));
            inventory.AddItem(new Item(redPotionScriptableObject), 5);
            inventory.AddItem(new Item(bluePotionScriptableObject), 2);
            inventory.AddItem(new Item(woolPotionScriptableObject), 30);
            inventory.AddItem(new Item(woodenLogPotionScriptableObject), 60);
        }
    }
}

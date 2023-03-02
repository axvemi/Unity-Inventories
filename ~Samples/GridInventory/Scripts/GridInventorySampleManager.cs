using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    public class GridInventorySampleManager : MonoBehaviour
    {
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

        public Inventory<Item> Inventory { get; private set; }

        private void Awake()
        {
            Inventory = new Inventory<Item>(slotAmount);
            inventoryUIController.Inventory = Inventory;
        }

        private void Start()
        {
            //ADD DEMO ITEMS
            Inventory.AddItem(new Item(swordScriptableObject));
            Inventory.AddItem(new Item(swordScriptableObject));
            Inventory.AddItem(new Item(redPotionScriptableObject), 5);
            Inventory.AddItem(new Item(bluePotionScriptableObject), 2);
            Inventory.AddItem(new Item(woolPotionScriptableObject), 30);
            Inventory.AddItem(new Item(woodenLogPotionScriptableObject), 60);
        }
    }
}

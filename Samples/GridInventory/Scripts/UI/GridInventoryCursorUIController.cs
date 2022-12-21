using UnityEngine;
using UnityEngine.UI;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Shows visually what the player has on the cursor slot
    /// </summary>
    public class GridInventoryCursorUIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject inventoryCursorUI;
        [SerializeField] private Image itemImage;

        private GridInventoryCursorController inventoryCursorController;

        private void Awake()
        {
            inventoryCursorController = GetComponent<GridInventoryCursorController>();
            inventoryCursorController.MouseInventorySlot.OnSlotUpdated += OnCursorSlotUpdated;
            inventoryCursorUI.SetActive(false);
        }

        /// <summary>
        /// Shows what the cursor's slot content is
        /// </summary>
        /// <param name="slot"></param>
        private void OnCursorSlotUpdated(InventorySlot<Item> slot)
        {
            if(slot.Item == null) 
            {
                inventoryCursorUI.SetActive(false);
            }
            else
            {
                inventoryCursorUI.SetActive(true);
                itemImage.sprite = slot.Item.ItemScriptableObject.Sprite;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Class that shows an inventory slot
    /// </summary>
    public class GridInventorySlotUIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;

        private InventorySlot<Item> slot;

        public InventorySlot<Item> Slot
        {
            get => slot;
            set
            {
                slot = value;
                slot.OnSlotUpdated += ShowSlot;
                ShowSlot(slot);
            }
        }

        /// <summary>
        /// Show the visuals for the inventory slot.
        /// If empty, do not show any content
        /// </summary>
        /// <param name="slot"></param>
        private void ShowSlot(InventorySlot<Item> slot) {
            if(slot.Item == null) {
                image.gameObject.SetActive(false);
                amountText.gameObject.SetActive(false);
            }
            else {
                image.gameObject.SetActive(true);
                image.sprite = slot.Item.ItemScriptableObject.Sprite;
                amountText.gameObject.SetActive(true);
                amountText.SetText(slot.Amount.ToString());
            }
        }

    }
}

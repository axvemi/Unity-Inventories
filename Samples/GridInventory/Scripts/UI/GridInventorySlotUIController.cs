using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        private InventorySlot<Item> _slot;

        public InventorySlot<Item> Slot
        {
            get => _slot;
            set
            {
                _slot = value;
                _slot.OnSlotUpdated += ShowSlot;
                ShowSlot(_slot);
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
                image.sprite = slot.Item.ItemScriptableObject.sprite;
                amountText.gameObject.SetActive(true);
                amountText.SetText(slot.Amount.ToString());
            }
        }

    }
}

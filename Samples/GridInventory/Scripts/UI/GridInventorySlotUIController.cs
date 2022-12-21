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
        [SerializeField] private Image image = null;
        [SerializeField] private TextMeshProUGUI ammountText = null;

        private InventorySlot<Item> slot = null;

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
                ammountText.gameObject.SetActive(false);
            }
            else {
                image.gameObject.SetActive(true);
                image.sprite = slot.Item.ItemScriptableObject.Sprite;
                ammountText.gameObject.SetActive(true);
                ammountText.SetText(slot.Amount.ToString());
            }
        }

    }
}

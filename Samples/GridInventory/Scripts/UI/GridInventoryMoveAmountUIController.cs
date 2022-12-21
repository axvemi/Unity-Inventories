using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Controls the UI for the move amount
    /// </summary>
    public class GridInventoryMoveAmountUIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_InputField inputField;

        private InventorySlot<Item> originSlot;
        private InventorySlot<Item> targetSlot;

        private int amountToTransfer;

        public void OpenMoveAmount(InventorySlot<Item> origin, InventorySlot<Item> target)
        {
            this.gameObject.SetActive(true);
            this.originSlot = origin;
            this.targetSlot = target;

            amountToTransfer = 1;
            slider.minValue = 1;
            slider.maxValue = origin.Amount;
            inputField.text = amountToTransfer.ToString();
        }

        public void OnInputFieldUpdated(string text)
        {
            int value = int.Parse(text);
            amountToTransfer = Mathf.Clamp(value, 0, originSlot.Amount);
            slider.value = amountToTransfer;
        }

        public void OnSliderUpdated(float value)
        {
            amountToTransfer = (int)value;
            inputField.text = amountToTransfer.ToString();
        }

        /// <summary>
        /// Transfers the items between the slots
        /// </summary>
        public void DoTransferItems()
        {
            originSlot.MoveBetweenSlots(targetSlot, amountToTransfer);
            this.gameObject.SetActive(false);
        }
    }
}

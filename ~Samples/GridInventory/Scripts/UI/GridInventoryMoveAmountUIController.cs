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

        private InventorySlot<Item> _originSlot;
        private InventorySlot<Item> _targetSlot;

        private int _amountToTransfer;

        public void OpenMoveAmount(InventorySlot<Item> origin, InventorySlot<Item> target)
        {
            gameObject.SetActive(true);
            _originSlot = origin;
            _targetSlot = target;

            _amountToTransfer = 1;
            slider.minValue = 1;
            slider.maxValue = origin.Amount;
            inputField.text = _amountToTransfer.ToString();
        }

        public void OnInputFieldUpdated(string text)
        {
            int value = int.Parse(text);
            _amountToTransfer = Mathf.Clamp(value, 0, _originSlot.Amount);
            slider.value = _amountToTransfer;
        }

        public void OnSliderUpdated(float value)
        {
            _amountToTransfer = (int)value;
            inputField.text = _amountToTransfer.ToString();
        }

        /// <summary>
        /// Transfers the items between the slots
        /// </summary>
        public void DoTransferItems()
        {
            _originSlot.MoveBetweenSlots(_targetSlot, _amountToTransfer);
            gameObject.SetActive(false);
        }
    }
}

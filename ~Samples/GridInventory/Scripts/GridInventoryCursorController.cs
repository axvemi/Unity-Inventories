using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axvemi.Inventories.Samples
{
    public class GridInventoryCursorController : MonoBehaviour
    {
        [SerializeField] private GridInventoryMoveAmountUIController moveAmountUIController;

        public InventorySlot<Item> MouseInventorySlot { get; set; } = new(null);

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClickPerformed();
            }
        }

        private void OnClickPerformed()
        {
            InventorySlot<Item> hoverSlot = GetSlotAtMousePosition(Input.mousePosition);
            if (hoverSlot == null) return;

            moveAmountUIController.gameObject.SetActive(false);

            //Dialog show X amount
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //Hover slot to cursor
                if (MouseInventorySlot.Item == null)
                {
                    moveAmountUIController.OpenMoveAmount(hoverSlot, MouseInventorySlot);
                }
                //Cursor to hover slot
                else
                {
                    moveAmountUIController.OpenMoveAmount(MouseInventorySlot, hoverSlot);
                }
            }
            //Transfer all amount
            else
            {
                //Move from the hover slot to the cursor
                if (MouseInventorySlot.Item == null)
                {
                    hoverSlot.MoveBetweenSlots(MouseInventorySlot, hoverSlot.Amount);
                }
                //Move from the cursor to the slot
                else
                {
                    MouseInventorySlot.MoveBetweenSlots(hoverSlot, MouseInventorySlot.Amount);
                }
            }
        }


        /// <summary>
        /// Gets the Inventory Slot that its at the mouse position
        /// </summary>
        /// <param name="position">Position in the canvas</param>
        /// <returns>Inventory Slot</returns>
        private static InventorySlot<Item> GetSlotAtMousePosition(Vector2 position)
        {
            PointerEventData pointerEventData = new PointerEventData(null)
            {
                position = position
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                GridInventorySlotUIController slotUIController = result.gameObject.GetComponent<GridInventorySlotUIController>();
                if (slotUIController != null)
                {
                    return slotUIController.Slot;
                }
            }

            return null;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axvemi.Inventories.Samples
{
    public class GridInventoryCursorController : MonoBehaviour
    {
        [SerializeField] private GridInventoryMoveAmountUIController moveAmountUIController;

        private InventorySlot<Item> mouseInventorySlot = new InventorySlot<Item>(null);

        public InventorySlot<Item> MouseInventorySlot { get => mouseInventorySlot; set => mouseInventorySlot = value; }

        void Update()
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
                if (mouseInventorySlot.Item == null)
                {
                    moveAmountUIController.OpenMoveAmount(hoverSlot, mouseInventorySlot);
                }
                //Cursor to hover slot
                else
                {
                    moveAmountUIController.OpenMoveAmount(mouseInventorySlot, hoverSlot);
                }
            }
            //Transfer all amount
            else
            {
                //Move from the hover slot to the cursor
                if (mouseInventorySlot.Item == null)
                {
                    hoverSlot.MoveBetweenSlots(mouseInventorySlot, hoverSlot.Amount);
                }
                //Move from the cursor to the slot
                else
                {
                    mouseInventorySlot.MoveBetweenSlots(hoverSlot, mouseInventorySlot.Amount);
                }
            }
        }


        /// <summary>
        /// Gets the Inventory Slot that its at the mouse position
        /// </summary>
        /// <param name="position">Position in the canvas</param>
        /// <returns>Inventory Slot</returns>
        private InventorySlot<Item> GetSlotAtMousePosition(Vector2 position)
        {
            PointerEventData pointerEventData = new PointerEventData(null);
            pointerEventData.position = position;

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

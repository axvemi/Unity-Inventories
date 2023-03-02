using UnityEngine;

namespace Axvemi.Inventories.Samples
{
    /// <summary>
    /// Removes whatever is stored on the cursor inventory slot
    /// </summary>
    public class GridInventoryTrashController : MonoBehaviour
    {
        [SerializeField] private GridInventoryCursorController cursorController;

        public void ClearCursor()
        {
            cursorController.MouseInventorySlot.Clear();
        }
    }
}

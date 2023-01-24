using System;

namespace Axvemi.Inventories
{
    public class FailedToStoreException : Exception
    {
        public FailedToStoreException()
        {
        }

        public FailedToStoreException(string message) : base(message)
        {
        }
    }

    public class FailedToMoveItemException : Exception
    {
        public FailedToMoveItemException()
        {
        }

        public FailedToMoveItemException(string message) : base(message)
        {
        }
    }

    public class FailedToFindSlotException : Exception
    {
        public FailedToFindSlotException()
        {
        }

        public FailedToFindSlotException(string message) : base(message)
        {
        }
    }
}
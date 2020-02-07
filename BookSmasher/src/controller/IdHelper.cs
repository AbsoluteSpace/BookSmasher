using System.Collections.Generic;

namespace BookSmasher.src.controller
{
    public static class IdHelper
    {
        // TODO sometimes have to list ids just to send to this which feels extra
        public static bool IdAlreadyAdded(string id, List<string> addedIds)
        {
            return addedIds.Contains(id);
        }

        // TODO add more conditions to check if id is good
        public static bool IsValid(string id)
        {
            return !string.IsNullOrWhiteSpace(id);
        }
    }
}

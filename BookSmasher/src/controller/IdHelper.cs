using System.Collections.Generic;

namespace BookSmasher.src.controller
{
    // Class to help check id information.
    public static class IdHelper
    {
        // Return true if the id is already added in addedIds.
        public static bool IdAlreadyAdded(string id, List<string> addedIds)
        {
            return addedIds.Contains(id);
        }

        // Returns true if the id isn't null or just whitespace.
        public static bool IsValid(string id)
        {
            return !string.IsNullOrWhiteSpace(id);
        }
    }
}

using System.Collections.Generic;
using System.Text;

namespace BookSmasher.src.controller
{
    // Class to help check id information.
    public static class IdUtil
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

        // Generates id of book collection.
        public static string CreateBookCollectionName(List<string> ids)
        {
            if (ids == null)
            {
                return "";
            }

            var builder = new StringBuilder();

            foreach (var id in ids)
            {
                builder.Append(id + " ");
            }

            // Names with whitespaces are hard.
            return builder.ToString().TrimEnd().Replace(' ', '_');
        }
    }
}

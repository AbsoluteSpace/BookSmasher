using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifier.src.controller
{
    public static class IdHelper
    {
        // TODO check if ID already added
        public static bool IdAlreadyAdded(string id)
        {
            return false;
        }

        // TODO add more conditions to check if id is good
        public static bool IsValid(string id)
        {
            return !string.IsNullOrWhiteSpace(id);
        }
    }
}

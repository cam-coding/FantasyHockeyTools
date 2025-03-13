using HtmlAgilityPack;

namespace Utilities
{
    public static class HtmlParsingHelper
    {
        public static HtmlNode FindNodeById(HtmlNode node, string id)
        {
            // Check if the current node has the specified id
            if (node.Attributes["id"] != null && node.Attributes["id"].Value == id)
            {
                return node;
            }

            // Recursively check each child node
            foreach (HtmlNode child in node.ChildNodes)
            {
                HtmlNode found = FindNodeById(child, id);
                if (found != null)
                {
                    return found;
                }
            }

            // Return null if the node with the specified id is not found
            return null;
        }

        public static HtmlNode FindNodesByClassName(HtmlNode node, string className)
        {
            if (node.Attributes["class"] != null && node.Attributes["class"].Value.Contains(className))
            {
                return node;
            }

            // Recursively check each child node
            foreach (HtmlNode child in node.ChildNodes)
            {
                var found = FindNodesByClassName(child, className);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }
    }
}

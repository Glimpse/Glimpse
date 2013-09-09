using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Ado.SimilarQueries
{
    /// <summary>
    /// A simple (naive) implementation of a generalised suffix tree.
    /// This algorithm is simple but is quadratic with respect to construction and memory.
    /// It should be switched for Ukkonen's algorithm, which is O(n).
    /// </summary>
    public class SuffixTree
    {
        public Node rootNode { get; set; }

        public SuffixTree()
        {
            rootNode = new Node(new char[0]);
        }

        /// <summary>
        /// Adds a new string to the suffix tree
        /// </summary>
        /// <param name="str">The string to add</param>
        public void AddString(string str)
        {
            str = string.Intern(str);
            var wordCharacters = str.ToCharArray();

            for (int startChar = 0; startChar < str.Length; startChar++)
            {
                var searchResult = SearchTree(wordCharacters, startChar);
                var currentNode = searchResult.Node;

                if (searchResult.FurthestMatchedIntoNode < searchResult.Node.Contents.Length)
                {
                    currentNode.SplitNode(searchResult.FurthestMatchedIntoNode);
                }

                if (searchResult.FinalCharacterIndex < wordCharacters.Length)
                {
                    var newNode = new Node(wordCharacters.Skip(searchResult.FinalCharacterIndex).ToArray());
                    currentNode.AddChild(newNode);
                    currentNode = newNode;
                }

                currentNode.AddTerminates(str);
            }
        }

        /// <summary>
        /// Finds the strings in the tree which contain a given string
        /// </summary>
        /// <param name="str">The string to search the tree with</param>
        /// <returns>The strings </returns>
        public IEnumerable<string> ContainedIn(string str)
        {
            var wordCharacters = str.ToCharArray();

            var searchResult = SearchTree(wordCharacters, 0);

            if (!searchResult.SuffixIsInTree) return Enumerable.Empty<string>();

            return GetDescendantTerminatedStrings(searchResult.Node);
        }

        /// <summary>
        /// Aggregates all the strings which are terminated in descendants of a given node
        /// </summary>
        /// <param name="startNode">The node to start at</param>
        /// <returns>All the strings which are terminated in descendants of a given node</returns>
        private static IEnumerable<string> GetDescendantTerminatedStrings(Node startNode)
        {
            var childNodesToProcess = new Queue<Node>();
            var strings = new HashSet<string>();

            childNodesToProcess.Enqueue(startNode);

            while (childNodesToProcess.Count > 0)
            {
                var currentNode = childNodesToProcess.Dequeue();

                foreach (var terminatedString in currentNode.Terminates)
                {
                    strings.Add(terminatedString);
                }

                foreach (var child in currentNode.Children)
                {
                    childNodesToProcess.Enqueue(child);
                }
            }

            return strings;
        }

        /// <summary>
        /// Searches the suffix tree to find the best match for a character array
        /// </summary>
        /// <param name="characters">The character array to base the search on</param>
        /// <param name="startCharIndex">The character index to start at through the array</param>
        /// <returns>A TreeSearchResult containing the results of the search</returns>
        private TreeSearchResult SearchTree(char[] characters, int startCharIndex)
        {
            Node currentNode;
            Node nextNode = rootNode;
            int currentNodeContentsIndex;
            var currentCharIndex = startCharIndex;

            do
            {
                currentNode = nextNode;
                nextNode = null;

                currentNodeContentsIndex = 1;
                while (currentNodeContentsIndex < currentNode.Contents.Length
                       && currentCharIndex < characters.Length
                       && currentNode.Contents[currentNodeContentsIndex] == characters[currentCharIndex])
                {
                    currentNodeContentsIndex++;
                    currentCharIndex++;
                }

                if (currentCharIndex + 1 > characters.Length) break;

                foreach (var child in currentNode.Children)
                {
                    if (child.Contents[0] == characters[currentCharIndex])
                    {
                        nextNode = child;
                        currentCharIndex++;
                        break;
                    }
                }
            } while (nextNode != null);

            return new TreeSearchResult(currentNode, currentCharIndex, currentNodeContentsIndex, (currentCharIndex >= characters.Length));
        }

        /// <summary>
        /// Represents a node in the suffix tree, the contents of the edge that leads to it, and any strings it terminates
        /// </summary>
        public class Node
        {
            private List<Node> children = new List<Node>();
            private HashSet<string> terminates = new HashSet<string>();

            public char[] Contents { get; set; }
            public IEnumerable<Node> Children { get { return children; } }
            public IEnumerable<string> Terminates { get { return terminates; } }

            /// <summary>
            /// Create a new node with specified contents
            /// </summary>
            /// <param name="contents">The contents of this node</param>
            public Node(char[] contents)
            {
                Contents = contents;
            }

            /// <summary>
            /// Create a new node split from an existing node
            /// </summary>
            private Node(char[] contents, List<Node> inheritChildren, HashSet<string> inheritTerminates)
            {
                Contents = contents;
                children = inheritChildren;
                terminates = inheritTerminates;
            }

            /// <summary>
            /// Adds a new child to the node
            /// </summary>
            /// <param name="node">The new child node</param>
            public void AddChild(Node node)
            {
                children.Add(node);
            }

            /// <summary>
            /// Add a string which is terminated at this node
            /// </summary>
            /// <param name="terminatesString">The string that terminates here</param>
            public void AddTerminates(string terminatesString)
            {
                terminates.Add(terminatesString);
            }

            /// <summary>
            /// Splits a node at a given character position
            /// </summary>
            /// <param name="retainedCharacters">The number of characters which should remain on the original node</param>
            /// <returns>The new node which has been created</returns>
            public Node SplitNode(int retainedCharacters)
            {
                var rest = Contents.Skip(retainedCharacters).ToArray();
                Contents = Contents.Take(retainedCharacters).ToArray();

                var newNode = new Node(rest, children, terminates);

                children = new List<Node> { newNode };
                terminates = new HashSet<string>();

                return newNode;
            }

            public override string ToString()
            {
                return new string(Contents); // +" (" + string.Join(", ", terminates) + ")";
            }
        }

        /// <summary>
        /// Represents the results of a walk through the tree
        /// </summary>
        private class TreeSearchResult
        {
            public Node Node { get; private set; }
            public int FinalCharacterIndex { get; private set; }
            public bool SuffixIsInTree { get; private set; }
            public int FurthestMatchedIntoNode { get; private set; }

            public TreeSearchResult(Node node, int finalCharacterIndex, int furthestMatched, bool matchedToEnd)
            {
                Node = node;
                FinalCharacterIndex = finalCharacterIndex;
                FurthestMatchedIntoNode = furthestMatched;
                SuffixIsInTree = matchedToEnd;
            }
        }
    }
}

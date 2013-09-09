using System.Collections.Generic;

namespace Glimpse.Ado.SimilarQueries
{
    /// <summary>
    /// Checks whether a given query is similar to any of the queries that have been previously added
    /// </summary>
    public class SimilarQueryProvider
    {
        /// <summary>
        /// The string similarity threshold above which two queries are considered similar
        /// </summary>
        private const double similarityThreshold = 0.5;

        private readonly HashSet<string> queries; 
        private readonly SuffixTree tree;

        public SimilarQueryProvider()
        {
            queries = new HashSet<string>();
            tree = new SuffixTree();
        }

        /// <summary>
        /// Adds a new query and returns a similarity score from 0 to 1
        /// </summary>
        /// <param name="query">The new query string to add</param>
        /// <returns>True if </returns>
        public StringSimilarity AddPotentiallySimilarQuery(string query)
        {
            query = query.ToLowerInvariant();

            if (queries.Contains(query))
            {
                return StringSimilarity.Identical;
            }

            var similarStringFound = SimilarStringInTree(query);

            queries.Add(query);
            tree.AddString(query);

            return similarStringFound ? StringSimilarity.Similar : StringSimilarity.None;
        }

        private bool SimilarStringInTree(string query)
        {
            var checkedMatches = new HashSet<string>();

            for (int startCharacter = 0; startCharacter + 10 < query.Length; startCharacter++)
            {
                var substring = query.Substring(startCharacter, 10);
                var matches = tree.ContainedIn(substring);

                foreach (var match in matches)
                {
                    if (checkedMatches.Contains(match)) continue;

                    if (GappedStringComparer.StringSimilarity(query, match, substring) > similarityThreshold) return true;
                }
            }

            return false;
        }
    }
}

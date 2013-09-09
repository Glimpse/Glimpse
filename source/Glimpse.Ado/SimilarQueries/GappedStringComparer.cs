using System;

namespace Glimpse.Ado.SimilarQueries
{
    /// <summary>
    /// Compares pairs of strings based on a common seed which is known to match both, taking into account
    /// potential gaps in the data (for which an alignment score penality is applied when they are opened and as they continue)
    /// </summary>
    public static class GappedStringComparer
    {
        /// <summary>
        /// Calculate a modified percentage similarity score between two strings which have a known common substring.
        /// Accommodates potential gaps in the data, which result in a penalty to the alignment score when they are opened and extended.
        /// Matching will be aborted if many gaps or large gaps are found.
        /// </summary>
        /// <param name="firstString">The first string to compare</param>
        /// <param name="secondString">The second string to compare</param>
        /// <param name="substringSeed">A string known to be contained in both</param>
        /// <returns>A value from 0 (no shared characters) to 1 (identical)</returns>
        public static double StringSimilarity(string firstString, string secondString, string substringSeed)
        {
            var firstStringWindowStart = firstString.LastIndexOf(substringSeed, System.StringComparison.Ordinal);
            var secondStringWindowStart = secondString.LastIndexOf(substringSeed, System.StringComparison.Ordinal);
            var windowSize = substringSeed.Length;

            var firstStringState = new StringState(firstString.ToCharArray(), firstStringWindowStart);
            var secondStringState = new StringState(secondString.ToCharArray(), secondStringWindowStart);
            var strings = new StringComparison(firstStringState, secondStringState, windowSize);

            return strings.GetSimilarityScore();
        }

        /// <summary>
        /// Maintains the overall state of the comparison between a pair of strings
        /// </summary>
        private class StringComparison
        {
            private StringState First { get; set; }
            private StringState Second { get; set; }
            private int Window { get; set; }
            private double score;

            public StringComparison(StringState first, StringState second, int initialWindowSize)
            {
                First = first;
                Second = second;
                Window = initialWindowSize;
                CurrentScore = initialWindowSize;
            }

            /// <summary>
            /// The current working alignment score for this pair
            /// </summary>
            private double CurrentScore
            {
                get { return score; }

                set
                {
                    score = value;
                    if (score > MaxScore) MaxScore = score;
                }
            }

            /// <summary>
            /// The maximum alignment score for this pair
            /// </summary>
            private double MaxScore { get; set; }

            /// <summary>
            /// Calculates a modified percentage similarity score for the two strings
            /// </summary>
            /// <returns>A value from 0 to 1, where 0 = no shared characters and 1 = strings are identical</returns>
            public double GetSimilarityScore()
            {
                if (First.Start < 0 || Second.Start < 0) return 0;

                // Extend left
                while (First.Start - 1 >= 0 && Second.Start - 1 >= 0 && CurrentScore > 0)
                {
                    if (First.Characters[First.Start - 1] == Second.Characters[Second.Start - 1])
                    {
                        ApplyMatch(false);
                    }
                    else
                    {
                        if (!TryInsertGap(false)) break;
                    }
                }

                CurrentScore = MaxScore;

                // Extend right
                while (First.Start + First.Gap + Window + 1 < First.Characters.Length
                       && Second.Start + Second.Gap + Window + 1 < Second.Characters.Length
                       && CurrentScore > 0)
                {
                    if (First.Characters[First.Start + Window + First.Gap + 1] ==
                        Second.Characters[Second.Start + Window + Second.Gap + 1])
                    {
                        ApplyMatch(true);
                    }
                    else
                    {
                        if (!TryInsertGap(true)) break;
                    }
                }

                return MaxScore / Math.Max(First.Characters.Length, Second.Characters.Length);
            }

            /// <summary>
            /// Increments the score and moves the position on
            /// </summary>
            /// <param name="forward">Whether to move forwards (true) or backwards (false)</param>
            private void ApplyMatch(bool forward)
            {
                CurrentScore++;
                Window++;

                if (!forward)
                {
                    First.Start--;
                    Second.Start--;
                }
            }

            /// <summary>
            /// Attempts to insert a gap in the comparison in the specified direction
            /// </summary>
            /// <param name="forward">Whether to search forwards (true) or backwards (false)</param>
            /// <returns>True if a gap was added, or false if no match was found</returns>
            private bool TryInsertGap(bool forward)
            {
                var modifier = forward ? 1 + Window : -1;
                var firstStringIndex = First.Start + modifier;
                var secondStringIndex = Second.Start + modifier;

                if (forward)
                {
                    firstStringIndex += First.Gap;
                    secondStringIndex += Second.Gap;
                }

                var shortestGapFirstString = GapSize(First.Characters, firstStringIndex, Second.Characters[secondStringIndex], forward);
                var shortestGapSecondString = GapSize(Second.Characters, secondStringIndex, First.Characters[firstStringIndex], forward);

                if (shortestGapFirstString == -1 && shortestGapSecondString == -1) return false;

                if (shortestGapFirstString > shortestGapSecondString)
                {
                    ApplyGap(First, shortestGapFirstString, forward);
                }
                else
                {
                    ApplyGap(Second, shortestGapSecondString, forward);
                }

                return true;
            }

            /// <summary>
            /// Applies a gap to a particular string in the comparison
            /// </summary>
            /// <param name="targetString">The string to apply the gap to</param>
            /// <param name="gap">The size of the gap to add</param>
            /// <param name="forward">Whether to search forwards (true) or backwards (false)</param>
            private void ApplyGap(StringState targetString, int gap, bool forward)
            {
                const double openGapPenalty = 1;
                const double continueGapPenalty = 0.5;

                CurrentScore -= openGapPenalty;

                if (!forward)
                {
                    targetString.Start -= gap;
                }

                targetString.Gap += gap;
                CurrentScore -= continueGapPenalty * (gap - 1);
            }

            /// <summary>
            /// Returns the shortest distance between a start point in a character array
            /// and the previous or next character in the array that matches another character,
            /// or -1 if it cannot be found
            /// </summary>
            /// <param name="targetCharacters">The character array to search</param>
            /// <param name="startPoint">The starting index in the array</param>
            /// <param name="lookFor">The character to find</param>
            /// <param name="forward">Whether to search forwards (true) or backwards (false)</param>
            /// <returns>The number of characters before a match is found, or -1</returns>
            private static int GapSize(char[] targetCharacters, int startPoint, char lookFor, bool forward)
            {
                for (int index = startPoint; index >= 0 && index < targetCharacters.Length; index += (forward) ? 1 : -1)
                {
                    if (targetCharacters[index] == lookFor) return Math.Abs(startPoint - index);
                }

                return -1;
            }
        }

        /// <summary>
        /// Maintains state for a specific string in the comparison
        /// </summary>
        private class StringState
        {
            public char[] Characters { get; private set; }
            public int Start { get; set; }
            public int Gap { get; set; }

            public StringState(char[] characters, int startPosition)
            {
                Characters = characters;
                Start = startPosition;
                Gap = 0;
            }
        }
    }
}

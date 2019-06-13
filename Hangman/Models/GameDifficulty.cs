using System.Collections.Generic;

namespace Hangman.Models
{
    /// <summary>
    /// Manages possible game difficulties
    /// </summary>
    public class GameDifficulty
    {

        #region "Properties"

        /// <summary>
        /// Index of difficulty level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Name of difficulty level
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Minimum length of the word to guess
        /// It depends on difficulty
        /// </summary>
        public int MinimumWordLength { get; set; }

        /// <summary>
        /// List of gallows parts depending on difficulty level
        /// </summary>
        public List<string> GallowsParts { get; set; } = 
            new List<string>();
        
        #endregion


        #region "Methods"
        #endregion

    }

}

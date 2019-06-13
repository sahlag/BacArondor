using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hangman.Models
{
    /// <summary>
    /// Manages the mystery word
    /// </summary>
    public static class Word
    {

        #region "Properties"

        /// <summary>
        /// Mystery word (to be guessed)
        /// </summary>
        public static string MysteryWord { get; set; }

        /// <summary>
        /// Word the player is currently guessing
        /// Hidden at first, characters will be revealed as player find them
        /// </summary>
        public static string HiddenWord { get; set; }

        #endregion


        #region "Methods"

        /// <summary>
        /// Loads the list of possible words from a file 
        /// in \Resources folder
        /// </summary>
        /// <returns>List of possible words</returns>
        private static List<string> LoadPossibleWords()
        {
            // define list to store file content
            List<string> PossibleWords = new List<string>();

            // define file path
            string FilePath = Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Directory.GetCurrentDirectory()))
                + @"\Resources\Words";
            // read Words file
            PossibleWords = File.ReadAllLines(
                FilePath)
                .ToList();

            // return list
            return PossibleWords;
        }

        /// <summary>
        /// Choose a random word from list
        /// with a minimum number of characters
        /// </summary>
        /// <param name="MinimumWordLength">The minimum word length for current difficulty</param>
        public static void ChooseRandomWord(
            int MinimumWordLength = 0)
        {
            // get all possible words
            Game.PossibleWords = LoadPossibleWords();
            // filter list for matching criterias
            // using linq and lambda expression
            List<string> Words =
                Game.PossibleWords.FindAll(
                    p => p.ToString().Length >= MinimumWordLength);

            // get random word in filtered list
            Random RandomGenerator = new Random();
            int RandomNumber = RandomGenerator.Next(Words.Count());
            MysteryWord = Words[RandomNumber].ToUpper();

            // define hidden word 
            // (same length as MysteryWord with placeholders)
            HiddenWord = new string('■', MysteryWord.Length);
        }

        /// <summary>
        /// Display hidden word
        /// </summary>
        public static void DisplayHiddenWord()
        {
            // write message
            Utilities.WriteRichText(
                "Mot mystère : ",
                2,
                11 + Game.Difficulties.Count);
            Console.SetCursorPosition(
                1,
                12 + Game.Difficulties.Count);

            // browse each character in hidden word
            foreach (char Character in HiddenWord)
            {
                if (Character == '■')
                    // character is a placeholder (not found yet)
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    // character was found
                    Console.ForegroundColor = ConsoleColor.Blue;

                // write character
                Console.Write(" {0} ", Character);
            }
        }

        #endregion

    }

}

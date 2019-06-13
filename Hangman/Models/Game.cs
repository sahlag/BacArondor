using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Manages the game
/// </summary>
namespace Hangman.Models
{
    public static class Game
    {

        #region "Properties and variables"

        /// <summary>
        /// List of all possible mystery words
        /// </summary>
        public static List<string> PossibleWords { get; set; }

        /// <summary>
        /// Possible game difficulties
        /// </summary>
        public static List<GameDifficulty> Difficulties { get; set; } = new List<GameDifficulty>();
        /// <summary>
        /// Current game difficulty
        /// </summary>
        public static GameDifficulty CurrentDifficulty { get; set; }

        /// <summary>
        /// Current number of errors made by the player
        /// </summary>
        private static int NumberOfErrors { get; set; }

        /// <summary>
        /// Possible characters player can enter (variable)
        /// </summary>
        private static List<string> PossibleCharacters =
            new List<string>()
                {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"," ","-"};
        /// <summary>
        /// Characters player has already entered (variable)
        /// </summary>
        private static List<string> UsedCharacters =
            new List<string>();

        #endregion


        #region "Methods"

        /// <summary>
        /// Load game resources
        /// </summary>
        public static void LoadGameResources()
        {
            // load resources
            Gallows.LoadGallowsDrawing();

            // define game difficulties
            Difficulties.Add(new GameDifficulty
            {
                Level = 1,
                Name = "Mineur",
                MinimumWordLength = 0,
                GallowsParts = new List<string>()
                    { "1","2","3","4","a","b","c","d","e","f" }
            });
            Difficulties.Add(new GameDifficulty
            {
                Level = 2,
                Name = "Majeur",
                MinimumWordLength = 5,
                GallowsParts = new List<string>()
                    { "12","34","a","b","cd","ef" }
            });
            Difficulties.Add(new GameDifficulty
            {
                Level = 3,
                Name = "Terrible",
                MinimumWordLength = 6,
                GallowsParts = new List<string>()
                    { "1234","a","bcd","ef" }
            });
        }

        /// <summary>
        /// Initialize game parameters
        /// </summary>
        public static void InitializeGame()
        {
            // show game screen
            Utilities.ShowGameScreen();

            // get player data
            Player.GetPlayerData();

            // choose mystery word
            Word.ChooseRandomWord(
                CurrentDifficulty.MinimumWordLength);

            // reset game data
            NumberOfErrors = 0;
            UsedCharacters = new List<string>();
        }

        /// <summary>
        /// Displays possible characters
        /// in different colors
        /// depending if they are already used or not
        /// </summary>
        public static void DisplayPossibleCharacters()
        {
            // write message
            Utilities.WriteRichText(
                "Caractères disponibles : ",
                2,
                Console.WindowHeight - 3);

            // set cursor position
            Console.SetCursorPosition(
                2, 
                Console.WindowHeight - 2);

            // for each possible characters
            foreach (string Character in PossibleCharacters)
            {
                // set character color
                Console.ForegroundColor = ConsoleColor.Black;
                if (UsedCharacters.Contains(Character))
                    // character has already been used
                    Console.BackgroundColor = ConsoleColor.Yellow;
                else
                    // character is still available
                    Console.BackgroundColor = ConsoleColor.Green;

                // write character in console
                Console.Write(
                    " {0} ",
                    Character);

                // add blank space between each character
                Console.ResetColor();
                Console.Write(
                    " ");
            }
        }

        /// <summary>
        /// Get character submitted by player
        /// </summary>
        /// <returns>The character</returns>
        public static string GetSubmittedCharacter()
        {
            // set cursor position
            string PlayerMessage = "Veuillez proposer un caractère : ";
            Console.SetCursorPosition(
                2 + PlayerMessage.Length,
                14 + Difficulties.Count);

            // wait for player entry
            // entry must be in possible character list
            ConsoleKeyInfo SubmittedCharacter = new ConsoleKeyInfo();
            while (!PossibleCharacters
                .Contains(Convert.ToChar(SubmittedCharacter.KeyChar)
                .ToString()
                .ToUpper()))
            {
                // set cursor position
                Console.SetCursorPosition(
                    2 + PlayerMessage.Length,
                    14 + Difficulties.Count);
                // wait for player entry
                SubmittedCharacter = Console.ReadKey();
            }

            // return player entry as upper case character
            return Convert.ToChar(SubmittedCharacter.KeyChar)
                .ToString()
                .ToUpper(); 
        }

        /// <summary>
        /// Checks if character submitted by player is in mystery word 
        /// and if end of game (victory or defeat) is triggered
        /// </summary>
        /// <param name="Character">Character submitted by player</param>
        /// <returns>True or False (end of game)</returns>
        public static bool CheckCharacterAndEndOfGame(
            string Character)
        {
            // add submitted character to used character list
            // if not already in list
            if (!UsedCharacters.Contains(Character))
                UsedCharacters.Add(Character);

            // check if submitted character is in mystery word 
            if (Word.MysteryWord.Contains(Character))
            {
                // character is in mystery word
                // replace each character in hidden word
                // with matching character submitted by player

                // transform words in char arrays
                char[] CharactersInMysteryWord = Word.MysteryWord.ToCharArray();
                char[] CharactersInHiddenWord = Word.HiddenWord.ToCharArray();

                // browse chhosen word char array to compare with submitted character
                for (int Index = 0; Index < Word.MysteryWord.Length; Index++)
                {
                    // character exists at that place in mystery word
                    // so put it at the same place in hidden word
                    if (CharactersInMysteryWord[Index] == char.Parse(Character))
                        CharactersInHiddenWord[Index] = char.Parse(Character);
                }

                // rebuild HiddenWord string from char array
                Word.HiddenWord = string.Join("", CharactersInHiddenWord);

                // check victory
                if (CheckVictory())
                {
                    // victory is triggered
                    // show mysterious word
                    Word.DisplayHiddenWord();

                    // show message
                    Utilities.WriteRichText(
                         "Bravo, vous êtes innocent, vous échappez donc à la potence !",
                         2,
                         20,
                         ConsoleColor.Green);

                    // save score
                    Utilities.SaveScore("GAGNÉ");

                    // this is the end of the game
                    return true;
                }
            }
            else
            {
                // character is not in mystery word

                // increment number of errors
                NumberOfErrors++;

                // draw gallow part matching number of errors
                Gallows.DrawGallows(
                    CurrentDifficulty.GallowsParts[NumberOfErrors - 1]);

                // check defeat
                if (CheckDefeat())
                {
                    // defeat is triggered
                    // show mysterious word
                    Word.HiddenWord = Word.MysteryWord;
                    Word.DisplayHiddenWord();

                    // show message
                    Utilities.WriteRichText(
                         "Dommage, retenez la leçon pour votre prochaine vie !",
                         2,
                         20,
                         ConsoleColor.Red);

                    // save score
                    Utilities.SaveScore("PERDU");

                    // this is the end of the game
                    return true;
                }
            }

            // refresh screen by displaying game data
            Word.DisplayHiddenWord();
            DisplayPossibleCharacters();

            // this is not the end of the game
            return false;
        }

        /// <summary>
        /// Checks if number of errors if greater or equal
        /// to maximum number of allowed errors
        /// -> defeat
        /// </summary>
        /// <returns>True or False</returns>
        private static bool CheckDefeat()
        {
            return (NumberOfErrors == CurrentDifficulty.GallowsParts.Count());
        }

        /// <summary>
        /// Checks if Hidden word equals mystery word
        /// -> victory
        /// </summary>
        /// <returns>True or False</returns>
        private static bool CheckVictory()
        {
            return (Word.MysteryWord == Word.HiddenWord);
        }

        #endregion

    }

}

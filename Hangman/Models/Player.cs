using System;
using System.Linq;
using System.Text;

namespace Hangman.Models
{
    /// <summary>
    /// Manages player data
    /// </summary>
    public static class Player
    {

        #region "Properties"

        /// <summary>
        /// Player name
        /// </summary>
        public static string Name { get; set; } = "";

        #endregion


        #region "Methods"

        /// <summary>
        /// Gets player data before starting game
        /// </summary>
        public static void GetPlayerData()
        {
            // ask for player name (3 characters minimum)
            Name = "";
            while (Name.Length < 3)
            {
                Utilities.WriteRichText(
                    "Accusé, merci d'entrer votre nom : ",
                    2,
                    3);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Name = Console.ReadLine();
            }

            // say hello with player name in yellow
            Utilities.WriteRichText(
                string.Format(
                    "Bonjour {0}, vous devez prouver votre innocence" +
                    "\nen trouvant le mot mystère.",
                    Name),
                2,
                3);
            Utilities.WriteRichText(
                Name,
                2 + "Bonjour ".Length,
                3,
                ConsoleColor.Yellow);

            // ask for difficulty level
            // define difficulty message with a string builder
            StringBuilder DifficultyMessage = new StringBuilder();
            DifficultyMessage.Append("Vous devez auparavant définir la gravité de votre crime.");
            string GetDifficulty = "Veuillez indiquez le numéro correspondant : ";
            int CursorPosition = GetDifficulty.Length + 2;
            DifficultyMessage.Append("\n" + GetDifficulty);
            foreach (GameDifficulty Difficulty in Game.Difficulties)
            {
                if (Difficulty.MinimumWordLength>0)
                    DifficultyMessage.Append(
                        string.Format(
                            "\n{0} - {1} (mots de {2} lettres ou plus et {3} erreurs possibles)",
                            Difficulty.Level,
                            Difficulty.Name,
                            Difficulty.MinimumWordLength,
                            Difficulty.GallowsParts.Count));
                else
                    DifficultyMessage.Append(
                        string.Format(
                            "\n{0} - {1} ({2} erreurs possibles)",
                            Difficulty.Level,
                            Difficulty.Name,
                            Difficulty.GallowsParts.Count));
            }

            // write message
            Utilities.WriteRichText(
                DifficultyMessage.ToString(),
                2,
                7);

            // get player choice
            // from 1 (ascii code 49) to max difficulty level
            ConsoleKeyInfo DifficultyLevel = new ConsoleKeyInfo();
            while ((int)(DifficultyLevel.KeyChar) < 49
                || (int)(DifficultyLevel.KeyChar) > (49 + Game.Difficulties.Count - 1))
            {
                // position cursor
                Console.SetCursorPosition(CursorPosition, 8);
                // wait for player choice
                DifficultyLevel = Console.ReadKey();
            }

            // assign difficulty level matching user choise
            // (using linq + lambda expression)
            Game.CurrentDifficulty = Game.Difficulties.First(
                p => p.Level == (int)(DifficultyLevel.KeyChar) - 48);

            // show difficulty name in place of submitted number
            Utilities.WriteRichText(
                Game.CurrentDifficulty.Name,
                CursorPosition,
                8,
                ConsoleColor.Yellow);
        }

        #endregion

    }

}

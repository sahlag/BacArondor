using Hangman.Models;

namespace Hangman
{
    /// <summary>
    /// Main class
    /// </summary>
    class Program
    {

        /// <summary>
        /// Programm entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // initialize environment
            Utilities.InitializeWindow();
            Game.LoadGameResources();

            // application loop
            bool NewGame = true;
            while(NewGame)
            {
                // initialize game
                Game.InitializeGame();

                // show game data
                Word.DisplayHiddenWord();
                Game.DisplayPossibleCharacters();

                // ask player to enter a character
                Utilities.WriteRichText(
                    "Veuillez proposer un caractère : ",
                    2,
                    14 + Game.Difficulties.Count);

                // main game loop 
                // true until end of game is triggered
                bool EndOfGame = false;
                while (!EndOfGame)
                {
                    // wait for player entry
                    string SubmittedCharacter = Game.GetSubmittedCharacter();

                    // check for end of game with submitted character as parameter
                    EndOfGame = Game.CheckCharacterAndEndOfGame(
                        SubmittedCharacter
                        .ToString()
                        .ToUpper());
                }

                // show score history and ask for new game
                NewGame = Utilities.ShowScoreHistory();
            }

        }

    }

}

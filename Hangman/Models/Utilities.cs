using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Models
{
    /// <summary>
    /// Methods that do not enter in other classes
    /// </summary>
    public static class Utilities
    {

        #region "Properties"
        #endregion


        #region "Methods"

        /// <summary>
        /// Define window (Console)
        /// </summary>
        public static void InitializeWindow()
        {
            // set initial console size
            int WindowWidth = 70;
            int WindowHeight = 11;
            Console.SetWindowSize(WindowWidth, WindowHeight);

            // hide cursor
            Console.CursorVisible = false;

            // show title
            WriteRichText(
                " LE JEU DU PENDU ",
                -1,
                1,
                ConsoleColor.Black,
                ConsoleColor.White);

            // show initial messages
            WriteRichText(
                "Merci de passer cette fenêtre" +
                "\nen plein écran avant de continuer.",
                -1,
                3,
                ConsoleColor.Yellow);
            WriteRichText(
                "Note : vous pouvez changer la taille de la police de caractères" +
                "\nen cliquant sur l'icône en haut à gauche puis Propriétés/Police." +
                "\n\nAppuyez sur une touche une fois cela fait.",
                -1,
                6);

            // wait for the player to hit a key
            Console.ReadKey(true);
        }

        /// <summary>
        /// Displays game screen (title and borders)
        /// </summary>
        public static void ShowGameScreen()
        {
            // clear console
            Console.Clear();

            // display window borders
            // define blank string of console width
            string BlankString = new String(' ', Console.WindowWidth);
            // display borders
            WriteRichText(
                BlankString,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.White);
            for (int LineNumber = 0; LineNumber <= Console.WindowHeight; LineNumber++)
            {
                WriteRichText(
                    " ",
                    0,
                    LineNumber,
                    ForegroundColor: ConsoleColor.Black,
                    BackgroundColor: ConsoleColor.White);
                WriteRichText(
                    " ",
                    Console.WindowWidth - 1,
                    LineNumber,
                    ForegroundColor: ConsoleColor.Black,
                    BackgroundColor: ConsoleColor.White);
            }
            WriteRichText(
                BlankString,
                PositionY: Console.WindowHeight,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.White);

            // display title
            WriteRichText(
                "LE JEU DU PENDU",
                -1,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.White);
        }

        /// <summary>
        /// Write text in console with arguments
        /// Can use \n to jump line as in standard console
        /// </summary>
        /// <param name="Text">Text to write</param>
        /// <param name="PositionX">Column (X - left) starting position (enter negative number to center text)</param>
        /// <param name="PositionY">Line (Y - top) starting position</param>
        /// <param name="ForegroundColor">Foreground color</param>
        /// <param name="BackgroundColor">Background color</param>
        public static void WriteRichText(
            string Text,
            int PositionX = 0,
            int PositionY = 0,
            ConsoleColor ForegroundColor = ConsoleColor.White,
            ConsoleColor BackgroundColor = ConsoleColor.Black)
        {
            // set colors
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;

            // split text by line
            string[] TextLines = Text.Split(
                new string[] { "\n" },
                StringSplitOptions.None);

            // check if text needs to be centered
            bool CenterText = (PositionX < 0);
            foreach (string TextLine in TextLines)
            {
                // set cursor position
                if (CenterText)
                    // horizontally centered text
                    PositionX = (Console.WindowWidth - TextLine.Length) / 2;
                Console.SetCursorPosition(PositionX, PositionY);

                // write text
                Console.Write(TextLine);

                // increment line for next line
                PositionY++;
            }
        }

        /// <summary>
        /// Save current game score in Scores file after victory or defeat
        /// </summary>
        /// <param name="Result">Result of the game (Victory or Defeat)</param>
        public static void SaveScore(
            string Result)
        {
            // retrieve Scores file path
            string FilePath = Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Directory.GetCurrentDirectory()))
                + @"\Resources\Scores";

            // open Scores file in append mode
            using (StreamWriter ScoreFile =
                new StreamWriter(
                    FilePath, true))
            {
                // write new game score (data are separated by ,)
                // {0} -> player name
                // {1} -> result (victory or defeat)
                // {2} -> date and time of the game
                // {3} -> difficulty level
                // {4} -> mystery word
                ScoreFile.WriteLine(
                    "{0},{1},{2},{3},{4}",
                    Player.Name,
                    Result,
                    DateTime.Now.ToString(@"dd/MM/yy \à HH:mm"),
                    Game.CurrentDifficulty.Name,
                    Word.MysteryWord);
            }

            // ask to press enter to continue
            string GoToHistoryMessage = "Appuyez sur Entrée pour voir l'historique des procès.";
            WriteRichText(
                GoToHistoryMessage,
                2,
                21);
            // wait for player entry
            ConsoleKeyInfo SubmittedCharacter = new ConsoleKeyInfo();
            while (SubmittedCharacter.Key != ConsoleKey.Enter)
            {
                Console.SetCursorPosition(
                    2 + GoToHistoryMessage.Length,
                    21);
                SubmittedCharacter = Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Displays score history
        /// </summary>
        /// <returns>Play a new game (true or false)</returns>
        public static bool ShowScoreHistory()
        {
            // clear console
            Console.Clear();
            Console.CursorVisible = false;
            ShowGameScreen();

            // write title
            WriteRichText(
                new String(' ', Console.WindowWidth),
                PositionY: 6,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.White);
            WriteRichText(
                "Historique des procès",
                -1,
                6,
                ForegroundColor: ConsoleColor.Black,
                BackgroundColor: ConsoleColor.White);

            // define first line to write scores
            int ScoreLine = 8;

            // define a list to store scores
            List<string> Scores = new List<string>();

            // read Scores file from relative \Resources folder
            // retrieve file path
            string FilePath = Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Directory.GetCurrentDirectory()))
                + @"\Resources\Scores";
            // read file
            Scores = File.ReadAllLines(
                FilePath)
                .ToList();
            // reverse list order (newer first)
            Scores.Reverse();

            // browse each score and format it before displaying
            // {0} -> player name
            // {1} -> result (victory or defeat)
            // {2} -> date and time of the game
            // {3} -> difficulty level
            // {4} -> mystery word
            foreach (string CurrentScore in Scores)
            {
                // split score string by its separator (,)
                string[] CurrentScoreData = CurrentScore.Split(',');
                // define color depending on result (with a ternary)
                ConsoleColor ScoreColor =
                    (CurrentScoreData[1] == "GAGNÉ" ?
                        ConsoleColor.Green :
                        ConsoleColor.Red);
                // write formatted string
                string ScoreString = string.Format(
                    "{0} a {1} le {2} au niveau {3} avec le mot {4}",
                    CurrentScoreData[0],
                    CurrentScoreData[1],
                    CurrentScoreData[2],
                    CurrentScoreData[3],
                    CurrentScoreData[4]);
                WriteRichText(
                    ScoreString,
                    -1,
                    ScoreLine,
                    ScoreColor);
                // write name in white
                WriteRichText(
                    CurrentScoreData[0],
                    (Console.WindowWidth - ScoreString.Length) / 2,
                    ScoreLine,
                    ConsoleColor.White);
                
                // increment line number
                ScoreLine++;
            }

            // ask player for a new game
            string NewGameMessage = "Voulez-vous un nouveau procès ? (O)ui ou (N)on : ";
            WriteRichText(
                NewGameMessage,
                -1,
                3);
            Console.SetCursorPosition(
                ((Console.WindowWidth - NewGameMessage.Length) / 2) + NewGameMessage.Length,
                3);

            // wait for player entry
            ConsoleKeyInfo SubmittedCharacter = new ConsoleKeyInfo();
            while (SubmittedCharacter.Key != ConsoleKey.O
                && SubmittedCharacter.Key != ConsoleKey.N)
            {
                Console.SetCursorPosition(
                    ((Console.WindowWidth - NewGameMessage.Length) / 2) + NewGameMessage.Length,
                    3);
                SubmittedCharacter = Console.ReadKey(true);
            }
            
            // return result
            return (SubmittedCharacter.Key == ConsoleKey.O);

        }

        #endregion

    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hangman.Models
{
    /// <summary>
    /// Manages the gallows
    /// </summary>
    public static class Gallows
    {

        #region "Properties and variables"

        /// <summary>
        /// Matrix (x/y coordinates) that contains gallows drawing
        /// </summary>
        private static string[,] GallowsArray { get; set; }

        #endregion


        #region "Methods"

        /// <summary>
        /// Loads gallows drawing from GallowsDrawing text file
        /// in \Resources folder
        /// then create a matrix GallowsArray with the drawing
        /// File contains digits for gallow parts and letters for hangman parts
        /// </summary>
        public static void LoadGallowsDrawing()
        {
            // list used to store temporarily the content of the file
            List<string> GallowsLines = new List<string>();

            // read file from relative \Resources folder
            string FilePath = Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Directory.GetCurrentDirectory()))
                + @"\Resources\GallowsDrawing";
            int MaxWidth = 0;
            foreach (string FileLine in File.ReadLines(FilePath))
            {
                GallowsLines.Add(FileLine);
                //define longuest gallows line (x dimension of matrix)
                MaxWidth = Math.Max(MaxWidth, FileLine.Length);
            }

            // create matrix (2 dimensions array)
            // to store drawing in x,y coordinates
            GallowsArray = new string[MaxWidth, GallowsLines.Count()];
            for (int y = 0; y < GallowsLines.Count(); y++)
            {
                // lines represent y axe
                string GallowLine = GallowsLines[y];
                for (int x = 0; x < GallowLine.Length; x++)
                {
                    // characters in line represent x axe
                    string GallowCharacter = GallowLine.Substring(x, 1);
                    // store current character in matrix
                    GallowsArray[x, y] = GallowCharacter;
                }
            }
        }

        /// <summary>
        /// Draw gallows in Console
        /// </summary>
        /// <param name="GallowParts">Characters corresponding to gallow parts to draw</param>
        public static void DrawGallows(
            string GallowParts = " ")
        {
            // browse matrix to draw parts
            for (int y = 0; y < GallowsArray.GetLength(1); y++)
            {
                for (int x = 0; x < GallowsArray.GetLength(0); x++)
                {
                    // get part identifier at x,y position  (with a ternary)
                    char CurrentCharacter = (
                        char.TryParse(GallowsArray[x, y], out _) ?
                        char.Parse(GallowsArray[x, y]) :
                        ' ');

                    if (CurrentCharacter == ' ')
                        // no part at this position (empty)
                        Console.BackgroundColor = ConsoleColor.Black;
                    else if (char.IsDigit(CurrentCharacter))
                        // gallow part at this position
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    else
                        // hangman part at this position
                        Console.BackgroundColor = ConsoleColor.White;

                    // draw part at current x,y position
                    // if part identifier matches parameter
                    // or if no part is specified (in which case all parts are drawn)
                    if (GallowParts.Contains(CurrentCharacter)
                        || GallowParts == " ")
                    {
                        // define offset of drawing
                        int GallowsOffsetX = Console.BufferWidth - 52;
                        int GallowsOffsetY = 2;
                        // set current character position on screen
                        Console.SetCursorPosition(
                            GallowsOffsetX + (x * 2),
                            GallowsOffsetY + y);
                        // draw character (2 spaces)
                        Console.Write("  ");
                    }
                }

            }

            Console.ResetColor();
        }

        #endregion

    }

}

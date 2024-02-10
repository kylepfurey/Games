using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Xml;

namespace Lab_2
{
    internal class Program
    {
        // NOTE: For pseudo code, 'letter' means the character apart of the chosen word, and 'character' means the character apart of the user's input.

        static void Main(string[] args)
        {
            HangmanDiagram(0);

            while (true)
            {
                int mistakes = 6;                                                               // Max mistakes (must be separate from guesses)

                int currentGuess = 0;                                                           // Current guess

                Console.WriteLine("\nHangman!\nby Kyle Furey\n");                   // Title

                Console.WriteLine("Enter a word. Please do not use special characters.");       // Prompt the user to enter a word (without special characters)
                Console.WriteLine("Type exit to close.\n");

                string choice = StringCheck();                                                  // Check if the chosen word is valid

                if (choice == "exit")                                                           // Exit check
                {
                    Console.WriteLine("\nProgram closed. Thank you!\n\n");
                    break;
                }

                char[] guessedLetters = new char[choice.Length + 5];                            // Number of guesses and what has been guessed

                char[] secretWord = new char[choice.Length];
                for (int lettercount = 0; lettercount < choice.Length; lettercount++)           // https://www.dotnetperls.com/string-char SOURCE
                {
                    secretWord[lettercount] = choice[lettercount];                              // Each letter of the chosen word
                }

                char[] wordToDisplay = new char[choice.Length];
                for (int lettercount = 0; lettercount < choice.Length; lettercount++)
                {
                    wordToDisplay[lettercount] = '_';                                           // The number of characters displayed
                }

                for (int lettercount = 0; lettercount < choice.Length; lettercount++)
                {
                    if (secretWord[lettercount] == ' ')                                         // Check if there are spaces
                    {
                        wordToDisplay[lettercount] = ' ';                                       // Display spaces apart of the word's revealed characters
                    }
                }

                                                                                    // Game starts
                
                Console.Clear();
                HangmanDiagram(mistakes);
                Console.WriteLine();
                Console.WriteLine(wordToDisplay);                                               // Print the word and its revealed letters
                Console.WriteLine("\nGuessed letters: \n");
                Console.WriteLine(mistakes + " guesses left.\n\n");                             // Number of guesses remaining

                while (mistakes > 0 && !Enumerable.SequenceEqual(secretWord, wordToDisplay))    // Repeat until 6 mistakes are made or until the word is complete
                {
                                                                                                // https://stackoverflow.com/questions/4423318/how-to-compare-arrays-in-c SOURCE

                    Console.WriteLine("\nEnter a letter to guess.\n");                          // Ask the user their guess

                    char letter = CharacterCheck();                                             // Check if the chosen character is valid

                    if (letter == 'π')                                                          // Exit check (pi is used as exit character)
                    {
                        choice = "exit";
                        Console.Clear();
                        break;
                    }

                    Console.WriteLine();

                    if (guessedLetters.Contains(letter) == false)                               // https://stackoverflow.com/questions/1818611/how-to-check-if-a-particular-character-exists-within-a-character-array SOURCE
                    {
                        guessedLetters[currentGuess] = letter;                                  // Check if the chosen character has been guessed

                        if (secretWord.Contains(letter))                                        // Check if the chosen character is apart of the word
                        {
                            for (int lettercount = 0; lettercount < choice.Length; lettercount++)
                            {
                                if (secretWord[lettercount] == letter)                          // Check if this letter is the chosen character
                                {
                                    wordToDisplay[lettercount] = letter;                        // Display this character apart of the word's revealed characters
                                }
                            }

                            Console.Clear();
                            HangmanDiagram(mistakes);
                            Console.WriteLine();
                            Console.WriteLine(wordToDisplay);                                   // Print the word and its revealed letters
                            Console.WriteLine();
                            Console.Write("Guessed letters: ");
                            Console.Write(guessedLetters);                                      // Guessed characters
                            Console.WriteLine("\n");
                            Console.WriteLine(mistakes + " guesses left.\n");                   // Number of guesses remaining
                            Console.WriteLine("Correct guess!");                                // Correct guess
                        }
                        else
                        {
                            mistakes--;                                                         // +1 Mistake
                            Console.Clear();
                            HangmanDiagram(mistakes);
                            Console.WriteLine();
                            Console.WriteLine(wordToDisplay);                                   // Print the word and its revealed letters
                            Console.WriteLine();
                            Console.Write("Guessed letters: ");            
                            Console.Write(guessedLetters);                                      // Guessed characters
                            Console.WriteLine("\n");
                            Console.WriteLine(mistakes + " guesses left.\n");                   // Number of guesses remaining
                            Console.WriteLine("Incorrect guess!");                              // Wrong guess
                        }

                        currentGuess++;                                                         // +1 Guess
                    }
                    else
                    {
                        Console.Clear();
                        HangmanDiagram(mistakes);
                        Console.WriteLine();
                        Console.WriteLine(wordToDisplay);                                       // Print the word and its revealed letters
                        Console.WriteLine();
                        Console.Write("Guessed letters: ");
                        Console.Write(guessedLetters);                                          // Guessed characters
                        Console.WriteLine("\n");
                        Console.WriteLine(mistakes + " guesses left.\n");                       // Number of guesses remaining
                        Console.WriteLine("You already chose that letter.");                    // Wrong guess
                    }

                                                                                    // Game ends

                    if (Enumerable.SequenceEqual(secretWord, wordToDisplay) == true)
                    {
                        Console.Clear();
                        HangmanDiagram(mistakes);
                        Console.Write("\nYou win! The correct word was " + choice + ".\n\n");   // Player wins
                        Console.WriteLine();
                    }

                    if (mistakes == 0)
                    {
                        Console.Clear();
                        HangmanDiagram(mistakes);
                        Console.Write("\nGame over! The correct word was " + choice + ".\n\n"); // Player loses
                        Console.WriteLine();

                    }
                }
            }
                                                                                    // Methods

            static string StringCheck()                                             // Input is a string and not a number METHOD
            {
                string input;                                                                   // Assign variables
                double output;

                while (true)
                {
                    input = Console.ReadLine();                                                 // Receive input

                    input = input.ToLower();                                                    // Make input all lowercase

                    if (Double.TryParse(input, out output) == false)                            // Check that it's not a number
                    {
                        if (input.Length > 0 && input != null && input != " ")                  // Check that it's not null or empty
                        {
                            return input;
                        }
                    }

                    Console.WriteLine("\nThat is not a word. Please try again.\n\n");           // Bad input
                }
            }

            static char CharacterCheck()                                            // Input is a character and not a number METHOD
            {
                string input;
                double output;
                char conversion;

                while (true)
                {
                    input = Console.ReadLine();                                                 // Receive input

                    input = input.ToLower();                                                    // Make input all lowercase

                    if (input == "exit")                                                        // Exit check
                    {
                        input = "π";
                        Char.TryParse(input, out conversion);                                   // Convert string to char
                        return conversion;
                    }

                    if (Double.TryParse(input, out output) == false)                            // Check that it's not a number
                    {
                        if (input.Length > 0 && input != null && input.Length < 2)              // Check that it's not null, empty, or  a single character
                        {
                            if (input != " ")
                            {
                                Char.TryParse(input, out conversion);                           // Convert string to char
                                return conversion;
                            }
                        }
                    }

                    Console.WriteLine("\nThat is not a single letter. Please try again.");      // Bad input
                    Console.WriteLine("Type exit to go back. Type exit again to close.\n\n");
                    Console.WriteLine("Enter a single letter to guess.\n");
                }
            }

            static void HangmanDiagram(int input)
            {
                switch (input)                                                      // Hangman diagram METHOD
                {
                    case 6:
                        Console.WriteLine("   ________");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("_____\n");
                        break;
                    case 5:
                        Console.WriteLine("   ________");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |       0");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("_____\n");
                        break;
                    case 4:
                        Console.WriteLine("   ________");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |       0");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("_____\n");
                        break;
                    case 3:
                        Console.WriteLine("   ________");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |       0");
                        Console.WriteLine("  |      /|");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("_____\n");
                        break;
                    case 2:
                        Console.WriteLine("   ________");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |       0");
                        Console.WriteLine("  |      /|\\");
                        Console.WriteLine("  |        ");
                        Console.WriteLine("_____\n");
                        break;
                    case 1:
                        Console.WriteLine("   ________");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |       0");
                        Console.WriteLine("  |      /|\\");
                        Console.WriteLine("  |      / ");
                        Console.WriteLine("_____\n");
                        break;
                    case 0:
                        Console.WriteLine("   ________");
                        Console.WriteLine("  |       |");
                        Console.WriteLine("  |       0");
                        Console.WriteLine("  |      /|\\");
                        Console.WriteLine("  |      / \\");
                        Console.WriteLine("_____\n");
                        break;
                }
            }
        }
    }
}
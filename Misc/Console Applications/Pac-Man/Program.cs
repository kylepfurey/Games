using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace PacMan
{
    internal class Play
    {
        static void Main(string[] args)
        {
            string highScoreString = "";
            int highScore = 0;

            while (true)
            {
                Console.OutputEncoding = Encoding.Unicode;
                Console.BackgroundColor = ConsoleColor.Black;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\n<");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(" MAC-PAN ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(">");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\nby Kyle Furey\n\n");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(highScoreString);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to start.");

                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();

                Console.Beep();
                Console.Clear();


                string map;

                // Find a map
                // If no map is found, generate the default map

                try
                {
                    // Replace 'map.txt' with 'maze.txt' for your silly maze.

                    map = System.IO.File.ReadAllText(@"C:\Users\kylef\Documents\Visual Studio 2022\gam110-kyle-furey\Final Project\map.txt");
                }
                catch
                {
                    System.IO.File.WriteAllText(@"C:\Users\kylef\Documents\Visual Studio 2022\gam110-kyle-furey\Final Project\map.txt",

                        "# # # # # # # # # # # # # # #\n" +
                        "# E O O O O O O O O O O O E #\n" +
                        "# O # O # # # # # # # O # O #\n" +
                        "# O # O O O O O O O O O # O #\n" +
                        "# O # O # # # O # # # O # O #\n" +
                        "# O # O O O # O # O O O # O #\n" +
                        "# O # # # O # O # O # # # O #\n" +
                        "T O # # # O # X # O # # # O T\n" +
                        "# O O O O O # # # O O O O O #\n" +
                        "# O # # # O # # # O # # # O #\n" +
                        "# O # # # O O O O O # # # O #\n" +
                        "# O # O O O # # # O O O # O #\n" +
                        "# O # O # # # # # # # O # O #\n" +
                        "# O O O O O O P O O O O O O #\n" +
                        "# # # # # # # # # # # # # # #");

                    map = System.IO.File.ReadAllText(@"C:\Users\kylef\Documents\Visual Studio 2022\gam110-kyle-furey\Final Project\map.txt");
                }

                if (map ==

                        "# # # # # # # # # # # # # # #\n" +
                        "# E O O O O O O O O O O O E #\n" +
                        "# O # O # # # # # # # O # O #\n" +
                        "# O # O O O O O O O O O # O #\n" +
                        "# O # O # # # O # # # O # O #\n" +
                        "# O # O O O # O # O O O # O #\n" +
                        "# O # # # O # O # O # # # O #\n" +
                        "T O # # # O # X # O # # # O T\n" +
                        "# O O O O O # # # O O O O O #\n" +
                        "# O # # # O # # # O # # # O #\n" +
                        "# O # # # O O O O O # # # O #\n" +
                        "# O # O O O # # # O O O # O #\n" +
                        "# O # O # # # # # # # O # O #\n" +
                        "# O O O O O O P O O O O O O #\n" +
                        "# # # # # # # # # # # # # # #")
                {
                    map = map.Replace(" ", string.Empty);
                }

                // Replace possible map errors

                map = map.ToLower();
                map = map.Replace("\r", string.Empty);
                map = map.Replace("k", "o");
                map = map.Replace("d", "x");
                map = map.Replace("_", " ");

                // Map Key

                // n = next line
                // # = wall
                // O = key
                // P = player
                // E = enemy
                // X = exit
                // T = teleporters


                // Determine the box size from the map string

                int xLength = 0;
                int yLength = 1;

                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i] == '\n')
                    {
                        break;
                    }

                    xLength++;
                }

                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i] == '\n')
                    {
                        yLength++;
                    }
                }

                // DEBUG
                // Console.WriteLine(map + "\n");
                // Console.WriteLine(xLength + " " + yLength + "\n");


                // Determine number of keys and enemies from map string

                int totalKeys = 0;

                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i] == 'o')
                    {
                        totalKeys++;
                    }
                }

                int totalEnemies = 0;

                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i] == 'e')
                    {
                        totalEnemies++;
                    }
                }

                // DEBUG
                // Console.WriteLine(totalKeys + " Keys");
                // Console.WriteLine(totalEnemies + " Enemies\n");


                // Determine position of walls, keys, player, and enemies from map string


                bool[][,] Wall = new bool[1][,];

                for (int i = 0; i < Wall.Length; i++)
                {
                    Wall[i] = new bool[xLength, yLength];
                }


                Key[] Key = new Key[totalKeys + 1];

                for (int i = 0; i < Key.Length; i++)
                {
                    Key[i] = new Key();
                }

                Key[Key.Length - 1].X = -99;
                Key[Key.Length - 1].Y = -99;

                // In order to prevent a crash and overlapping, the program has to add an extra key to prevent exceeding the boundaries of the key array.


                Player Player = new Player();


                Enemy[] Enemy = new Enemy[totalEnemies];

                for (int i = 0; i < Enemy.Length; i++)
                {
                    Enemy[i] = new Enemy();

                    if (i / 2 == i - i / 2)
                    {
                        Enemy[i].EnemyType = true;
                    }
                }

                // By checking if the enemy's number is odd or even, I can add two different types of enemy movement scripts to it.


                Exit Exit = new Exit();

                Teleporter[] Teleporter = new Teleporter[2];

                for (int i = 0; i < Teleporter.Length; i++)
                {
                    Teleporter[i] = new Teleporter();
                }


                // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/jagged-arrays SOURCE
                // https://stackoverflow.com/questions/3301678/how-to-declare-an-array-of-objects-in-c-sharp SOURCE


                int xCheck = 0;
                int yCheck = 0;
                int keyCheck = 0;
                int enemyCheck = 0;
                int teleCheck = 0;

                for (int i = 0; i < map.Length; i++)
                {
                    if (xCheck > xLength)
                    {
                        xCheck = 0;
                    }

                    if (map[i] == '\n')
                    {
                        yCheck++;

                        // DEBUG
                        // Console.WriteLine();
                    }

                    if (map[i] == '#')
                    {
                        Wall[0][xCheck, yCheck] = true;
                    }

                    if (map[i] == 'o')
                    {
                        Key[keyCheck].X = xCheck;
                        Key[keyCheck].Y = yCheck;

                        keyCheck++;
                    }

                    if (map[i] == 'p')
                    {
                        Player.X = xCheck;
                        Player.Y = yCheck;
                    }

                    if (map[i] == 'e')
                    {
                        Enemy[enemyCheck].X = xCheck;
                        Enemy[enemyCheck].Y = yCheck;

                        enemyCheck++;
                    }

                    if (map[i] == 'x')
                    {
                        Exit.X = xCheck;
                        Exit.Y = yCheck;
                    }

                    if (map[i] == 't')
                    {
                        Teleporter[teleCheck].X = xCheck;
                        Teleporter[teleCheck].Y = yCheck;
                        teleCheck++;
                    }

                    // DEBUG
                    // Console.WriteLine("(" + xCheck + ", " + yCheck + ", " + Wall[i][xCheck, yCheck] + ") ");

                    xCheck++;
                }


                // Game Start


                bool end = false;
                while (end == false)
                {
                    Render(xLength, yLength, Wall, Key, Player, Enemy, Exit, totalKeys);

                    int[] coords = Player.Move();

                    if (coords[0] == -100 && coords[1] == -100)
                    {
                        Console.Clear();
                        break;
                    }

                    if (coords[0] != -99 && coords[1] != -99 && Wall[0][coords[0], coords[1]] == false)
                    {
                        Player.X = coords[0];
                        Player.Y = coords[1];

                        if (Teleporter[0].Teleport(Player) == true)
                        {
                            Player.X = Teleporter[1].X - 1;
                            Player.Y = Teleporter[1].Y;
                        }

                        if (Teleporter[1].Teleport(Player) == true)
                        {
                            Player.X = Teleporter[0].X + 1;
                            Player.Y = Teleporter[0].Y;
                        }

                        for (int i = 0; i < Key.Length; i++)
                        {
                            Key[i].Collect(Player);
                        }

                        for (int i = 0; i < Enemy.Length; i++)
                        {
                            if (Enemy[i].Lose(Player) == true)
                            {
                                end = true;
                            }

                            coords = Enemy[i].Move(Player, Wall);

                            Enemy[i].X = coords[0];
                            Enemy[i].Y = coords[1];

                            for (int j = 0; j < Enemy.Length; j++)
                            {
                                if (i == j)
                                {
                                    break;
                                }

                                // If enemies merge, teleport them to a specific spot.
                                if ((Enemy[i].X == Enemy[j].X) && (Enemy[i].Y == Enemy[j].Y))
                                {
                                    Enemy[i].X = xLength / 2;
                                    Enemy[i].Y = yLength / 5;
                                }
                            }

                            if (Enemy[i].Lose(Player) == true)
                            {
                                end = true;
                            }
                        }

                        if (Exit.Win(Player, totalKeys) == true)
                        {
                            end = true;
                        }
                    }

                    // https://stackoverflow.com/questions/45286/how-can-i-overwrite-the-same-portion-of-the-console-in-a-windows-native-c-cons SOURCE
                    // Clear the console without flickering (by setting the cursor to the top and overwriting what was rendered with what is currently rendered).

                    Console.SetCursorPosition(0, 0);
                }

                Console.Clear();

                if (Exit.Win(Player, totalKeys) == true)
                {
                    Player.Score += 100;
                    Console.Beep();

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nYou win!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to return to the main menu.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nYou lose!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to return to the main menu.");
                }

                Console.ReadKey();
                if (Player.Score > highScore)
                {
                    highScoreString = "High Score: " + Player.Score;
                }
                Console.Clear();
            }
        }


        // Render the map and objects

        static void Render(int xLength, int yLength, bool[][,] Wall, Key[] Key, Player Player, Enemy[] Enemy, Exit Exit, int totalKeys)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Score: " + Player.Score + "\n");

            char wallIcon = '#';

            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    bool occupied = false;

                    if (Wall[0][x, y] == true && x != Exit.X | y != Exit.Y)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(wallIcon + " ");
                        occupied = true;
                    }

                    for (int i = 0; i < Key.Length; i++)
                    {
                        for (int j = 0; j < Enemy.Length; j++)
                        {
                            if (Key[i].X == Enemy[j].X && Key[i].Y == Enemy[j].Y && i < Key.Length - 1)
                            {
                                i++;
                            }
                        }


                        if (Key[i].X == x && Key[i].Y == y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Key[i].Icon + " ");
                            occupied = true;
                            break;
                        }

                        // In order to prevent a crash and overlapping, the program has to add an extra key to prevent exceeding the boundaries of the key array.
                    }

                    if (Player.X == x && Player.Y == y)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(Player.Icon + " ");
                        occupied = true;
                    }

                    for (int i = 0; i < Enemy.Length; i++)
                    {
                        if (Enemy[i].X == x && Enemy[i].Y == y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Enemy[i].Icon + " ");
                            occupied = true;
                            break;
                        }
                    }

                    if (Exit.X == x && Exit.Y == y && Exit.Appear(Player, totalKeys) == true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write(Exit.Icon + " ");
                        Wall[0][Exit.X, Exit.Y] = false;
                        occupied = true;
                    }
                    else if (Exit.X == x && Exit.Y == y)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write(wallIcon + " ");
                        Wall[0][Exit.X, Exit.Y] = true;
                        occupied = true;
                    }

                    if (occupied == false)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("  ");
                    }

                }

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nW A S D to move.\nSpace to wait.\nQ to return to the main menu.\nCollect all orbs to unlock the exit.");
        }
    }



    // Object classes

    public class Key
    {
        public int X = 0;
        public int Y = 0;
        public char Icon = 'O';


        // When the player is touching a key, add to the total keys, score, and remove the key

        public void Collect(Player Player)
        {
            if (X == Player.X && Y == Player.Y)
            {
                X = -99;
                Y = -99;
                Player.Score += 100;
                Player.Keys += 1;
                Console.Beep();
            }
        }
    }



    public class Player
    {
        public int X = 0;
        public int Y = 0;
        public char Icon = '<';
        public int Keys = 0;
        public int Score = 0;


        // Take the input of the player and determine where the next coordinates are after the player moves, if not waiting or exiting

        public int[] Move()
        {
            char input = Console.ReadKey().KeyChar;

            switch (input)
            {
                case 'w':
                    Icon = 'V';
                    return new int[2] { X, Y - 1 };

                case 'a':
                    Icon = '>';
                    return new int[2] { X - 1, Y };

                case 's':
                    Icon = 'Ʌ';
                    return new int[2] { X, Y + 1 };

                case 'd':
                    Icon = '<';
                    return new int[2] { X + 1, Y };
                case 'W':
                    Icon = 'V';
                    return new int[2] { X, Y - 1 };

                case 'A':
                    Icon = '>';
                    return new int[2] { X - 1, Y };

                case 'S':
                    Icon = 'Ʌ';
                    return new int[2] { X, Y + 1 };

                case 'D':
                    Icon = '<';
                    return new int[2] { X + 1, Y };

                case 'q':
                    return new int[2] { -100, -100 };

                case 'Q':
                    return new int[2] { -100, -100 };

                case ' ':
                    return new int[2] { X, Y };

                default:
                    return new int[2] { -99, -99 };
            }
        }
    }



    public class Enemy
    {
        public int X = 0;
        public int Y = 0;
        public char Icon = '@';
        public bool EnemyType = false;


        // Determine which direction is fastest to the player

        public int[] Move(Player Player, bool[][,] Wall)
        {
            if (EnemyType == true)
            {
                // Check each direction, determine if it is the least distance to the player, and set it to that direction only if it is not into a wall.
                // This enemy type prioritizes vertical distance.

                int leastDistance = 100;

                if (leastDistance >= Player.Y - Y && (Wall[0][X, Y - 1] == false))
                {
                    leastDistance = Player.Y - Y;
                }

                if (leastDistance >= Y - Player.Y && (Wall[0][X, Y + 1] == false))
                {
                    leastDistance = Y - Player.Y;
                }

                if (leastDistance >= Player.X - X && (Wall[0][X - 1, Y] == false))
                {
                    leastDistance = Player.X - X;
                }

                if (leastDistance >= X - Player.X && (Wall[0][X + 1, Y] == false))
                {
                    leastDistance = X - Player.X;
                }

                if (leastDistance == Y - Player.Y && (Wall[0][X, Y + 1] == false))
                {
                    return new int[2] { X, Y + 1 };
                }

                if (leastDistance == Player.Y - Y && (Wall[0][X, Y - 1] == false))
                {
                    return new int[2] { X, Y - 1 };
                }

                if (leastDistance == X - Player.X && (Wall[0][X + 1, Y] == false))
                {
                    return new int[2] { X + 1, Y };
                }

                if (leastDistance == Player.X - X && (Wall[0][X - 1, Y] == false))
                {
                    return new int[2] { X - 1, Y };
                }
            }
            else
            {
                // Check each direction, determine if it is the least distance to the player, and set it to that direction only if it is not into a wall.
                // This enemy type prioritizes horizontal distance.

                int leastDistance = 100;

                if (leastDistance >= X - Player.X && (Wall[0][X + 1, Y] == false))
                {
                    leastDistance = X - Player.X;
                }

                if (leastDistance >= Player.X - X && (Wall[0][X - 1, Y] == false))
                {
                    leastDistance = Player.X - X;
                }

                if (leastDistance >= Y - Player.Y && (Wall[0][X, Y + 1] == false))
                {
                    leastDistance = Y - Player.Y;
                }

                if (leastDistance >= Player.Y - Y && (Wall[0][X, Y - 1] == false))
                {
                    leastDistance = Player.Y - Y;
                }

                if (leastDistance == X - Player.X && (Wall[0][X + 1, Y] == false))
                {
                    return new int[2] { X + 1, Y };
                }

                if (leastDistance == Player.X - X && (Wall[0][X - 1, Y] == false))
                {
                    return new int[2] { X - 1, Y };
                }

                if (leastDistance == Y - Player.Y && (Wall[0][X, Y + 1] == false))
                {
                    return new int[2] { X, Y + 1 };
                }

                if (leastDistance == Player.Y - Y && (Wall[0][X, Y - 1] == false))
                {
                    return new int[2] { X, Y - 1 };
                }
            }

            return new int[2] { X, Y };
        }


        // Check if the player is touching an enemy

        public bool Lose(Player Player)
        {
            if (X == Player.X && Y == Player.Y)
            {
                return true;
            }

            return false;
        }
    }



    public class Exit
    {
        public int X = 0;
        public int Y = 0;
        public char Icon = 'X';


        // Check whether to appear as a wall or the goal

        public bool Appear(Player Player, int totalKeys)
        {
            if (Player.Keys == totalKeys)
            {
                return true;
            }

            return false;
        }


        // Check the player is in the win position with the total keys needed

        public bool Win(Player Player, int totalKeys)
        {
            if (X == Player.X && Y == Player.Y && Player.Keys == totalKeys)
            {
                return true;
            }

            return false;
        }
    }

    public class Teleporter
    {
        public int X = 0;
        public int Y = 0;

        public bool Teleport(Player Player)
        {
            if (X == Player.X && Y == Player.Y)
            {
                return true;
            }

            return false;
        }
    }
}
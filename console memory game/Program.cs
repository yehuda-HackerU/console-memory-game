using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_memory_game
{
    class Program
    {
        private static (int, int) GetUserChoice(int[,] table, int round, bool player_1_turn, int max, int x_axle_1 = -1, int y_axle_1 = -1)
        {
            int x, y;
            while (true)
            {
                Console.Write($"Player {(player_1_turn ? "1" : "2")}, enter {round}{(round == 1 ? "st" : "nd")} x axle choice (1-{max}): ");
                char input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                while (true)
                {
                    if (Int32.TryParse(input.ToString(), out x) && x >= 1 && x <= max)
                    {
                        break;
                    }

                    Console.Write($"Wrong input, enter a number between 1 to  {max}: ");
                    input = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                }

                Console.Write($"Player {(player_1_turn ? "1" : "2")}, enter {round}{(round == 1 ? "st" : "nd")} y axle choice (1-{max}): ");
                input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                while (true)
                {
                    if (Int32.TryParse(input.ToString(), out y) && y >= 1 && y <= max)
                    {
                        break;
                    }
                    else
                    {
                        Console.Write($"Wrong input, enter a number between 1 to  {max}: ");
                        input = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                    }
                }

                if (x == x_axle_1 && y == y_axle_1)
                {
                    Console.WriteLine($"You have selected {x}, {y} for 1st choice, try again :(.");
                }
                else if (table[y - 1, x - 1] == 0)
                {
                    Console.WriteLine($"{x}, {y} is empty, try again.");
                }
                else
                {
                    break;
                }
            }

            (int x, int y) user_input = (x, y);
            return user_input;
        }

        private static int[,] CreateTable(int table_size)
        {
            int[,] table = new int[table_size, table_size];

            int[] cards = new int[table.Length];
            int card = 1;
            for (int i = 0; i < cards.Length; i += 2)
            {
                cards[i] = card;
                cards[i + 1] = card++;
            }
            int[] shuffled_cards = cards.OrderBy(n => Guid.NewGuid()).ToArray();

            int index = 0;
            for (int i = 0; i < table_size; i++)
            {
                for (int j = 0; j < table_size; j++)
                {
                    table[i, j] = shuffled_cards[index++];
                }
            }

            return table;
        }

        private static void PrintTable(int[,] table, int x_axle_1 = -1, int y_axle_1 = -1, int x_axle_2 = -1, int y_axle_2 = -1)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if ((i == y_axle_1 - 1 && j == x_axle_1 - 1) || (i == y_axle_2 - 1 && j == x_axle_2 - 1))
                    {
                        Console.Write($"{table[i, j],3}");
                    }
                    else
                    {
                        if (table[i, j] == 0)
                        {
                            Console.Write("   ");
                        }
                        else
                        {
                            Console.Write("  X");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        private static bool GameOver(int[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        static void Main(string[] args)
        {
            char[] options = { '1', '2', '3' };
            char size_input;
            do
            {
                Console.Write("choose table size(1-3): (1: 2*2, 2: 4*4, 3: 8*8) ");
                size_input = Console.ReadKey().KeyChar;
                Console.WriteLine();
            } while (Array.IndexOf(options, size_input) == -1);

            int table_size = 0;
            switch (size_input)
            {
                case '1':
                    table_size = 2;
                    break;
                case '2':
                    table_size = 4;
                    break;
                case '3':
                    table_size = 8;
                    break;
            }
            int[,] table = CreateTable(table_size);

            bool player_1_turn = true;
            int player_1_score = 0;
            int player_2_score = 0;
            while (!GameOver(table))
            {
                Console.WriteLine($"\nPlayer 1: {player_1_score}.\tPlayer 2: {player_2_score}");

                PrintTable(table);

                (int x, int y) user_input = GetUserChoice(table, 1, player_1_turn, table_size);
                int x_axle_1 = user_input.x;
                int y_axle_1 = user_input.y;
                PrintTable(table, x_axle_1, y_axle_1);

                user_input = GetUserChoice(table, 2, player_1_turn, table_size, x_axle_1, y_axle_1);
                int x_axle_2 = user_input.x;
                int y_axle_2 = user_input.y;
                PrintTable(table, x_axle_1, y_axle_1, x_axle_2, y_axle_2);

                if (table[--y_axle_1, --x_axle_1] == table[--y_axle_2, --x_axle_2])
                {
                    table[y_axle_1, x_axle_1] = 0;
                    table[y_axle_2, x_axle_2] = 0;

                    if (player_1_turn)
                    {
                        player_1_score++;
                    }
                    else
                    {
                        player_2_score++;
                    }
                }
                else
                {
                    player_1_turn = !player_1_turn;
                }
                Console.WriteLine($"Player {(player_1_turn ? "1" : "2")}, Press any key to play.");
                Console.ReadKey();
                Console.Clear();
            }

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();

            //by: t.me/yehuda100
        }
    }
}

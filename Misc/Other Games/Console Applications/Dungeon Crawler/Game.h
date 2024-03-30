#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <map>

#include "Player.h"
#include "Room.h"
#include "MonsterBase.h"
#include "Monsters.h"
#include "ItemBase.h"
#include "Items.h"
#include "Inventory.h"

using namespace std;

// Title screen and player input
#define CLEAR std::system("CLS");
#define TITLE CLEAR std::cout << "\n   . . . DUNGEON SCRIPTER . . .\n\n          by Kyle Furey\n\nPress a key and hit enter to play.\n__________________________________\n\n\n";
#define INPUT(input) cout << "INPUT: "; cin >> input;

// Room count
#define TOTAL_ROOMS 12

// Game Class
class game
{
public:

	// ROOM LAYOUT GENERATION

	// Room layout array
	room rooms[TOTAL_ROOMS];
	room* current_room;

	// Item dictionary
	map<room*, item*> chest_items;

	// Monster dictionary
	map<room*, monster> monsters;

	// Inventory
	inventory player_inventory;

	// Map items
	apple item1;
	totem item2;
	potion item3;
	axe item4;


	// Center Screen Variable
	string center_screen;

	// Sets all of the rooms and their connections
	void generate_rooms()
	{
		//       MAP                INDEX
		// 
		//  K       M		    10      11
		//  M   N   F   C		 6   7   8   9
		//          N		             5
		//  C   N   F   W		 1   2   3   4
		//      W			         0

		// Initialize items by assigning a chest room type to a room, and giving that corresponding room an item in the dictionary.
		// Initialize monsters by assigning the desired monster room type to a room, and giving that corresponding room the same monster class in the dictionary.

		// Set all room types
		rooms[0] = room(room_type::wall);
		rooms[1] = room(room_type::chest);
		rooms[2] = room(room_type::nothing);
		rooms[3] = room(room_type::fairy);
		rooms[4] = room(room_type::wall);
		rooms[5] = room(room_type::nothing);
		rooms[6] = room(room_type::mimic);
		rooms[7] = room(room_type::nothing);
		rooms[8] = room(room_type::fairy);
		rooms[9] = room(room_type::chest);
		rooms[10] = room(room_type::knight);
		rooms[11] = room(room_type::mimic);

		// Set all room connections
		rooms[0].up_room = &rooms[2];
		rooms[1].right_room = &rooms[2];
		rooms[2].down_room = &rooms[0];
		rooms[2].left_room = &rooms[1];
		rooms[2].right_room = &rooms[3];
		rooms[3].up_room = &rooms[5];
		rooms[3].left_room = &rooms[2];
		rooms[3].right_room = &rooms[4];
		rooms[4].left_room = &rooms[3];
		rooms[5].up_room = &rooms[8];
		rooms[5].down_room = &rooms[3];
		rooms[6].up_room = &rooms[10];
		rooms[6].right_room = &rooms[7];
		rooms[7].left_room = &rooms[6];
		rooms[7].right_room = &rooms[8];
		rooms[8].up_room = &rooms[11];
		rooms[8].down_room = &rooms[5];
		rooms[8].left_room = &rooms[7];
		rooms[8].right_room = &rooms[9];
		rooms[9].left_room = &rooms[8];
		rooms[10].down_room = &rooms[6];
		rooms[11].down_room = &rooms[8];

		// Reset room
		current_room = &rooms[2];

		// Set chest items
		chest_items[&rooms[1]] = &item1;
		chest_items[&rooms[6]] = &item2;
		chest_items[&rooms[9]] = &item3;
		chest_items[&rooms[11]] = &item4;

		// Set monsters
		monsters[&rooms[3]] = foo_fairy();
		monsters[&rooms[6]] = malloc_mimic();
		monsters[&rooms[8]] = foo_fairy();
		monsters[&rooms[10]] = null_knight();
		monsters[&rooms[11]] = malloc_mimic();
	}


	// CONSTRUCTORS AND DESTRUCTORS

	// Constructor
	game() { current_room = nullptr; }

	// Deconstructor
	~game() { }


	// STRING CONCATENATION

	// Returns a string of a given char for another string's length
	string string_of_chars(string data, char character)
	{
		string new_string = string();

		for (int i = 0; i < data.length(); i++)
		{
			new_string += character;
		}

		return new_string;
	}

	// Returns a string of a given char for another object's length
	template<class DataType> string string_of_chars(DataType data, char character)
	{
		string old_string = to_string(data);
		string new_string = string();

		for (int i = 0; i < old_string.length(); i++)
		{
			new_string += character;
		}

		return new_string;
	}

	// Returns a string of a given string for another string's length
	string string_of_strings(string data, string words)
	{
		string new_string = string();

		for (int i = 0; i < data.length(); i++)
		{
			new_string += words;
		}

		return new_string;
	}

	// Returns a string of a given string for another object's length
	template<class DataType> static string string_of_strings(DataType data, string words)
	{
		string old_string = to_string(data);
		string new_string = string();

		for (int i = 0; i < old_string.length(); i++)
		{
			new_string += words;
		}

		return new_string;
	}

	// Returns a string of a given char for a given length
	string string_of_chars(char character, int length)
	{
		string new_string = string();

		for (int i = 0; i < length; i++)
		{
			new_string += character;
		}

		return new_string;
	}

	// Returns a string of a given string for a given length
	string string_of_strings(string words, int length)
	{
		string new_string = string();

		for (int i = 0; i < length; i++)
		{
			new_string += words;
		}

		return new_string;
	}


	// RENDERING BOARD

	// Render the screen with dynamic x scaling
	void refresh()
	{
		CLEAR;

		center_screen = string_of_chars(' ', ((to_string(player_inventory.high_score).length() + to_string(player_inventory.score).length())) / 2 - 1);

		cout << "\n" << center_screen << ". . . DUNGEON SCRIPTER . . .\n";

		string scores = "SCORE: " + to_string(player_inventory.score) + "      " + string_of_chars(' ', to_string(player_inventory.high_score).length() - 18 + (10 - to_string(player_health).length())) + "HIGH SCORE: " + to_string(player_inventory.high_score);

		cout << string_of_chars('_', scores.length()) << "\n\n";

		cout << scores << "\n";

		cout << string_of_chars('_', scores.length()) << "\n\n";

		cout << "HEALTH: " << player_health << string_of_chars(' ', scores.length() - to_string(player_inventory.money).length() - 18 + (10 - to_string(player_health).length())) << "MONEY: " << player_inventory.money << "\n\n";

		cout << "INVENTORY:\n";

		player_inventory.print_items();

		cout << string_of_chars(scores, '_') << "\n\n";

		switch (current_room->type)
		{
		case nothing:
			cout << center_screen << "___________________________\n";
			cout << center_screen << "| _______________________ |\n";
			cout << center_screen << "||                       ||\n";
			cout << center_screen << "| |                     | |\n";
			cout << center_screen << "|  |                   |  |\n";
			cout << center_screen << "|   |                 |   |\n";
			cout << center_screen << "|    |               |    |\n";
			cout << center_screen << "|     |  _   _   _  |     |\n";
			cout << center_screen << "|   -  -   -   -   -  -   |\n";
			cout << center_screen << "|  -  -  -  -  -  -  - -  |\n";
			cout << center_screen << "| - -  -  -  -  -  -  - - |\n";
			cout << center_screen << "|_________________________|\n\n";
			break;

		case chest:
			cout << center_screen << "___________________________\n";
			cout << center_screen << "| _______________________ |\n";
			cout << center_screen << "||                       ||\n";
			cout << center_screen << "| |                     | |\n";
			cout << center_screen << "|  |                   |  |\n";
			cout << center_screen << "|   | _______________ |   |\n";
			cout << center_screen << "|    /______|v|______\\    |\n";
			cout << center_screen << "|   |-----------------|   |\n";
			cout << center_screen << "|   |_________________|   |\n";
			cout << center_screen << "|  -|      |   |      |-  |\n";
			cout << center_screen << "| - |______|___|______| - |\n";
			cout << center_screen << "|_________________________|\n\n";
			break;

		case wall:
			cout << center_screen << "___________________________\n";
			cout << center_screen << "||_|___|_|___|_|___|_|___||\n";
			cout << center_screen << "|___|_|___|_|___|_|___|_|_|\n";
			cout << center_screen << "||_|___|_|___|_|___|_|___||\n";
			cout << center_screen << "|___|_|___|_|___|_|___|_|_|\n";
			cout << center_screen << "||_|___|_|___|_|___|_|___||\n";
			cout << center_screen << "|___|_|___|_|___|_|___|_|_|\n";
			cout << center_screen << "||_|___|_|___|_|___|_|___||\n";
			cout << center_screen << "|-  -  -   -   -   -  -  -|\n";
			cout << center_screen << "|- -  -  -  -  -  - -  - -|\n";
			cout << center_screen << "| - -  -  -  -  -  -  - - |\n";
			cout << center_screen << "|_________________________|\n\n";
			break;

		case fairy:
			cout << center_screen << "___________________________\n";
			cout << center_screen << "| _______________________ |\n";
			cout << center_screen << "||   *               *   ||\n";
			cout << center_screen << "| |       * \\/ *        | |\n";
			cout << center_screen << "|  |   *  |\\()/|  *    |  |\n";
			cout << center_screen << "|   | *   |-||-|   *  |   |\n";
			cout << center_screen << "|    |   *|/!!\\|*    |    |\n";
			cout << center_screen << "|     |  _   _   _  |     |\n";
			cout << center_screen << "|   -  -   -   -   -  -   |\n";
			cout << center_screen << "|  -  -  -  -  -  -  - -  |\n";
			cout << center_screen << "| - -  -  -  -  -  -  - - |\n";
			cout << center_screen << "|_________________________|\n\n";
			break;

		case mimic:
			cout << center_screen << "___________________________\n";
			cout << center_screen << "| _______________________ |\n";
			cout << center_screen << "||                       ||\n";
			cout << center_screen << "| |                     | |\n";
			cout << center_screen << "|  |                   |  |\n";
			cout << center_screen << "|   | _______________ |   |\n";
			cout << center_screen << "|    /___0__|v|__0___\\    |\n";
			cout << center_screen << "|   |||||||||||||||||||   |\n";
			cout << center_screen << "|   |WWWWWWWWWWWWWWWWW|   |\n";
			cout << center_screen << "|  -|      |   |      |-  |\n";
			cout << center_screen << "| - |______|___|______| - |\n";
			cout << center_screen << "|_________________________|\n\n";
			break;

		case knight:
			cout << center_screen << "___________________________\n";
			cout << center_screen << "| _______________________ |\n";
			cout << center_screen << "||     |\\  _____  /|     ||\n";
			cout << center_screen << "| |  ^  \\\\/     \\//  ^  | |\n";
			cout << center_screen << "|  | \\\\  | |\\ /| |  // |  |\n";
			cout << center_screen << "|   | \\\\  \\ _n_ /  // |   |\n";
			cout << center_screen << "|    |  /==|=|=|==\\  |    |\n";
			cout << center_screen << "|     |  _ |/_\\|_   |     |\n";
			cout << center_screen << "|   -  -   -   -   -  -   |\n";
			cout << center_screen << "|  -  -  -  -  -  -  - -  |\n";
			cout << center_screen << "| - -  -  -  -  -  -  - - |\n";
			cout << center_screen << "|_________________________|\n\n";
			break;
		}

		cout << string_of_chars(scores, '_') << "\n";
	}


	// GAME LOOP FUNCTIONS

	// Obtaining a new item
	void item_check(string& input)
	{
		// Give the item to the player if an item is present and they have room
		if (current_room->type == room_type::chest)
		{
			if (player_inventory.add_item(chest_items[current_room]))
			{
				refresh();

				cout << "\nYOU GOT ONE " << chest_items[current_room]->name << "!\n\n";

				INPUT(input);

				current_room->type = room_type::nothing;

				player_inventory.score += 100;

				// Update high score
				if (player_inventory.score > player_inventory.high_score)
				{
					player_inventory.high_score = player_inventory.score;
				}
			}
		}
	}

	// Moving
	bool move(string& input)
	{
		// Move prompt
		cout << "\n" << center_screen << "  MOVE WHICH DIRECTION?\n\n";

		cout << center_screen << ((current_room->up_room != nullptr) ? "        W -  UP\n\n" : "        W - ____\n\n");

		cout << center_screen << ((current_room->left_room != nullptr) ? "A - LEFT    +    " : "A - ____    +    ");

		cout << center_screen << ((current_room->right_room != nullptr) ? "D - RIGHT\n\n" : "D - ____\n\n");

		cout << center_screen << ((current_room->down_room != nullptr) ? "        S - DOWN\n" : "        S - ____\n");

		cout << "\n";

		// Input
		INPUT(input);

		// Exit condition
		if (input == "exit")
		{
			return true;
		}

		// Movement
		switch (tolower(input[0]))
		{
		case 'w':

			if (current_room->up_room != nullptr)
			{
				current_room = current_room->up_room;

				item_check(input);
			}

			break;

		case 'a':

			if (current_room->left_room != nullptr)
			{
				current_room = current_room->left_room;

				item_check(input);
			}

			break;

		case 's':

			if (current_room->down_room != nullptr)
			{
				current_room = current_room->down_room;

				item_check(input);
			}

			break;

		case 'd':

			if (current_room->right_room != nullptr)
			{
				current_room = current_room->right_room;

				item_check(input);
			}

			break;
		}

		return false;
	}

	// Attacking
	bool attack(string& input, int item_index)
	{
		refresh();

		cout << "\n";

		// Use the item
		player_inventory.use_item(player_inventory.items[item_index], &monsters[current_room]);

		// Was the monster defeated
		if (monsters[current_room].health <= 0)
		{
			cout << "YOU DEFEATED THE " << monsters[current_room].name << "!\n";

			player_inventory.score += 200;

			// Update high score
			if (player_inventory.score > player_inventory.high_score)
			{
				player_inventory.high_score = player_inventory.score;
			}

			// Game end
			if (monsters[current_room].is_boss)
			{
				player_inventory.score += 800;

				// Update high score
				if (player_inventory.score > player_inventory.high_score)
				{
					player_inventory.high_score = player_inventory.score;
				}

				current_room->type = room_type::nothing;

				refresh();

				cout << "\nYOU DEFEATED THE " << monsters[current_room].name << "!\n\n";

				cout << "YOU ESCAPED! FOR NOW . . .\n\n";

				INPUT(input);

				return true;
			}

			// Give an item
			if (monsters[current_room].drops_item)
			{
				cout << "\n";

				INPUT(input);

				current_room->type = room_type::chest;

				item_check(input);
			}
			else
			{
				current_room->type = room_type::nothing;
			
				cout << "\n";

				INPUT(input);
			}
		}
		else
		{
			// Take damage
			damage(monsters[current_room].attack);

			// Did the player die
			if (player_health <= 0)
			{
				refresh();

				cout << "\n";

				player_inventory.use_item(player_inventory.items[item_index], &monsters[current_room]);

				damage(monsters[current_room].attack);

				cout << "\nYOU DIED! BETTER LUCK NEXT TIME . . .\n\n";

				INPUT(input);

				return true;
			}

			cout << "\n";

			INPUT(input);
		}

		return false;
	}

	// Battling
	bool battle(string& input)
	{
		// Battle prompt
		cout << "\nYOU ENCOUNTERED A " << monsters[current_room].name << "!\n\n";

		cout << center_screen << ((player_inventory.items.size() > 0) ? "        W - " + player_inventory.items[0]->name + "\n\n" : "        W - ____\n\n");

		cout << center_screen << ((player_inventory.items.size() > 1) ? "A - " + player_inventory.items[1]->name + "   +    " : "A - ____    +    ");

		cout << center_screen << ((player_inventory.items.size() > 3) ? "D - " + player_inventory.items[3]->name + "\n\n" : "D - ____\n\n");

		cout << center_screen << ((player_inventory.items.size() > 2) ? "        S - " + player_inventory.items[2]->name + "\n" : "        S - ____\n");

		cout << "\n";

		// Input
		INPUT(input);

		// Exit condition
		if (input == "exit")
		{
			return true;
		}

		// Battle actions
		switch (tolower(input[0]))
		{
		case 'w':

			if (player_inventory.items.size() > 0)
			{
				if (attack(input, 0))
				{
					return true;
				}
			}

			break;

		case 'a':

			if (player_inventory.items.size() > 1)
			{
				if (attack(input, 1))
				{
					return true;
				}
			}

			break;

		case 's':

			if (player_inventory.items.size() > 2)
			{
				if (attack(input, 2))
				{
					return true;
				}
			}

			break;

		case 'd':

			if (player_inventory.items.size() > 3)
			{
				if (attack(input, 3))
				{
					return true;
				}
			}

			break;
		}

		return false;
	}

	// Starts the game loop
	void play()
	{
		// Game loop
		while (true)
		{
			// Title screen
			TITLE;

			// Input
			string input;
			INPUT(input);

			// Exit condition
			if (input == "exit")
			{
				break;
			}

			// Regenerate rooms
			generate_rooms();

			// Initialize inventory
			player_inventory.reset();

			// Turn loop
			while (true)
			{
				// Render board
				refresh();

				// Check if entering battle
				if ((current_room->type != room_type::fairy) && (current_room->type != room_type::mimic) && (current_room->type != room_type::knight))
				{
					if (move(input))
					{
						break;
					}
				}
				else
				{
					if (battle(input))
					{
						break;
					}
				}
			}
		}

		cout << "\n\nProgram closed.\n\nSee you soon . . .\n";
	}
};

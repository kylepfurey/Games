#include <iostream>
#include <string>
#include "Inventory.h"
#include "Item.h"

using namespace std;

// Shop keeper expression
enum expression { neutral, happy, upset, annoyed, satisfied };
// SOURCE: https://www.programiz.com/cpp-programming/enumeration

void title(string text, expression expression)
{
	// Clears the console
	system("CLS");

	// Title text
	cout << "WELCOME TO THE ITEM SHOP!\n";
	cout << "by Kyle Furey\n\n\n";

	switch (expression)
	{

	case neutral:
		cout << "     \"" << text << "\"" << endl;
		cout << "                    V            " << endl;
		cout << "                                 " << endl;
		cout << "                   ___           " << endl;
		cout << "                  (•_•)          " << endl;
		cout << "                 \\/ | \\        " << endl;
		cout << "   c||  c||         | /    c||   " << endl;
		cout << "----------------------------------" << endl;
		cout << "==================================" << endl;
		cout << "  ||   /\\      /||\\      /\\   ||" << endl;
		cout << "  ||  /  \\    / || \\    /  \\  ||" << endl;
		cout << "  || /    \\  /  ||  \\  /    \\ ||" << endl;
		cout << "  ||/      \\/   ||   \\/      \\||" << endl;
		cout << "\n\n";
		break;

	case happy:
		cout << "     \"" << text << "\"" << endl;
		cout << "                    V            " << endl;
		cout << "                                 " << endl;
		cout << "                   ___           " << endl;
		cout << "                  (^_^)          " << endl;
		cout << "                 \\/ | \\/       " << endl;
		cout << "   c||  c||         |      c||   " << endl;
		cout << "----------------------------------" << endl;
		cout << "==================================" << endl;
		cout << "  ||   /\\      /||\\      /\\   ||" << endl;
		cout << "  ||  /  \\    / || \\    /  \\  ||" << endl;
		cout << "  || /    \\  /  ||  \\  /    \\ ||" << endl;
		cout << "  ||/      \\/   ||   \\/      \\||" << endl;
		cout << "\n\n";
		break;

	case upset:
		cout << "     \"" << text << "\"" << endl;
		cout << "                    V            " << endl;
		cout << "                                 " << endl;
		cout << "                   ___           " << endl;
		cout << "                  (•~•)          " << endl;
		cout << "                  / | \\         " << endl;
		cout << "   c||  c||       \\ | /    c||  " << endl;
		cout << "----------------------------------" << endl;
		cout << "==================================" << endl;
		cout << "  ||   /\\      /||\\      /\\   ||" << endl;
		cout << "  ||  /  \\    / || \\    /  \\  ||" << endl;
		cout << "  || /    \\  /  ||  \\  /    \\ ||" << endl;
		cout << "  ||/      \\/   ||   \\/      \\||" << endl;
		cout << "\n\n";
		break;

	case annoyed:
		cout << "     \"" << text << "\"" << endl;
		cout << "                    V            " << endl;
		cout << "                                 " << endl;
		cout << "                   ___           " << endl;
		cout << "                  (T_T)          " << endl;
		cout << "                  / | \\/        " << endl;
		cout << "   c||  c||       \\ |      c||  " << endl;
		cout << "----------------------------------" << endl;
		cout << "==================================" << endl;
		cout << "  ||   /\\      /||\\      /\\   ||" << endl;
		cout << "  ||  /  \\    / || \\    /  \\  ||" << endl;
		cout << "  || /    \\  /  ||  \\  /    \\ ||" << endl;
		cout << "  ||/      \\/   ||   \\/      \\||" << endl;
		cout << "\n\n";
		break;

	case satisfied:
		cout << "     \"" << text << "\"" << endl;
		cout << "                    V            " << endl;
		cout << "                                 " << endl;
		cout << "                   ___           " << endl;
		cout << "                  (v_v)          " << endl;
		cout << "                  / | \\         " << endl;
		cout << "   c||  c||       \\ | /    c||  " << endl;
		cout << "----------------------------------" << endl;
		cout << "==================================" << endl;
		cout << "  ||   /\\      /||\\      /\\   ||" << endl;
		cout << "  ||  /  \\    / || \\    /  \\  ||" << endl;
		cout << "  || /    \\  /  ||  \\  /    \\ ||" << endl;
		cout << "  ||/      \\/   ||   \\/      \\||" << endl;
		cout << "\n\n";
		break;

	}
}

int main(int argc, char* argv[])
{
	// Randomize the random seed
	srand(time(0));
	// SOURCE: https://en.cppreference.com/w/cpp/numeric/random/srand

	// Initalize variables
	int input = 0;
	Inventory player;
	Inventory shop;
	player.Inventory::Inventory();
	shop.Inventory::Inventory();

	// Randomize shop items
	for (int i = 0; i < 5; i++)
	{
		shop.inventory[i].type = (itemType)((rand() % 7) + 1);
		shop.inventory[i].cost = (itemType)((rand() % 9) + 1);
	}

	// Randomize player and shop gold
	player.gold = ((rand() % 15) + 5);
	shop.gold = ((rand() % 15) + 5);

	// Game loop
	while (true)
	{
		// Refresh screen and give prompt
		title("What can I do for ye?", neutral);

		// Refresh input
		input = 0;

		// Input check
		while (input < 1 || input > 3)
		{
			// Prompt 1
			cout << "1. Buy\n";
			cout << "2. Sell\n";
			cout << "3. Exit\n\n";
			cout << "INPUT: ";
			cin >> input;

			if (input > 0 && input < 4)
			{
				break;
			}

			cout << "\nThat be an invalid input.\n\n\n";
		}

		// End game
		if (input == 3)
		{
			break;
		}

		switch (input)
		{

			// Sell
		case 2:

			title("Ugh... What are ye offering?", annoyed);

			// Check for items
			for (int i = 0; i < 5; i++)
			{
				if (player.inventory[i].type != None)
				{
					break;
				}

				if (player.inventory[4].type == None)
				{
					title("You have nothin to yer name!", upset);
				}
			}

			// Refresh screen and give prompt

			// Selling loop
			while (true)
			{
				// Refresh input
				input = 0;

				// Input check
				while (input < 1 || input > 6)
				{
					// Prompt 2
					cout << "YOUR GOLD: " << player.gold << "\n";
					cout << "SHOPKEERER'S GOLD: " << shop.gold << "\n\n";
					player.DisplayInventory();
					cout << "6. Back\n\n";
					cout << "INPUT: ";
					cin >> input;

					if (input > 0 && input < 7)
					{
						break;
					}

					cout << "\nThat be an invalid input.\n\n\n";
				}

				// End game
				if (input == 6)
				{
					break;
				}

				// Selling nothing
				if (player.inventory[input - 1].type == None)
				{
					title("Yer playing me for a fool!", annoyed);
					continue;
				}

				// Too expensive
				if (shop.gold < player.inventory[input - 1].Cost())
				{
					title("I can't afford that!", upset);
					continue;
				}

				// Sell the item
				player.gold += player.inventory[input - 1].Cost();
				shop.gold -= player.inventory[input - 1].Cost();
				player.inventory[input - 1].resell = true;
				shop.AddItem(player.inventory[input - 1]);
				player.RemoveItem(player.inventory[input - 1]);

				title("I suppose I can take that off your hands...", satisfied);
			}

			break;

			// Buy
		case 1:
			title("What are we interested in today?", satisfied);

			// Check for items
			for (int i = 0; i < 5; i++)
			{
				if (shop.inventory[i].type != None)
				{
					break;
				}

				if (shop.inventory[4].type == None)
				{
					title("I seem to be out of stock...", upset);
				}
			}

			// Refresh screen and give prompt

			// Buying loop
			while (true)
			{
				// Refresh input
				input = 0;

				// Input check
				while (input < 1 || input > 6)
				{
					// Prompt 2
					cout << "YOUR GOLD: " << player.gold << "\n";
					cout << "SHOPKEERER'S GOLD: " << shop.gold << "\n\n";
					shop.DisplayInventory();
					cout << "6. Back\n\n";
					cout << "INPUT: ";
					cin >> input;

					if (input > 0 && input < 7)
					{
						break;
					}

					cout << "\nThat be an invalid input.\n\n\n";
				}

				// End game
				if (input == 6)
				{
					break;
				}

				// Buying nothing
				if (shop.inventory[input - 1].type == None)
				{
					title("That be a whole lotta squat!", upset);
					continue;
				}

				// Too expensive
				if (player.gold < shop.inventory[input - 1].Cost())
				{
					title("You be more broke then me own heart!", annoyed);
					continue;
				}

				// Buy the item
				shop.gold += shop.inventory[input - 1].Cost();
				player.gold -= shop.inventory[input - 1].Cost();
				shop.inventory[input - 1].resell = true;
				player.AddItem(shop.inventory[input - 1]);
				shop.RemoveItem(shop.inventory[input - 1]);

				title("Pleasure doing business!", happy);
			}

			break;
		}
	}

	title("Safe travels.", happy);
}

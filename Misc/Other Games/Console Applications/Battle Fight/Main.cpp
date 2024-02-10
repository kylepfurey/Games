#include <iostream>
#include <string>
#include "Game.h"

using namespace std;

int main(int argc, char* argv[])
{
	while (true)
	{
		// Title screen
		system("CLS");

		cout << "\nBATTLE FIGHT!\n";
		cout << "by Kyle Furey\n\n\n";

		int input = 0;

		while (true)
		{
			// Prompt
			cout << "Start the game?\n1. Start\n2. Exit\n\n\n";

			cout << "INPUT: ";
			cin >> input;

			if (input == 1 || input == 2)
			{
				break;
			}

			cout << "\nThat is not a valid selection.\n\n\n";
		}

		if (input == 2)
		{
			break;
		}

		// Creates and plays game (the MAGIC is in the game class)
		Game game = Game();
		game.Play();
		game.~Game();
	}

	cout << "\nThanks for playing!\n";

	return 0;
}

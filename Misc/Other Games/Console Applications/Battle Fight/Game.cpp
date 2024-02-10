#include "Game.h"
#include <iostream>

// Game Functions
Game::Game()
{
	// Constructor
	playerCount = 0;
	currentPlayerPtr = nullptr;
	allPlayers = "";
	remainingPlayers = 0;
	allTurns = "";
	currentTurn = 1;
}

Game::~Game()
{
	// Deconstructor
	delete[] currentPlayerPtr;
	currentPlayerPtr = nullptr;
}

void Game::RefreshScreen()
{
	// Refreshes the screen and prints the title
	system("CLS");

	std::cout << "\nBATTLE FIGHT!\n";
	std::cout << "by Kyle Furey\n\n\n";
	std::cout << allPlayers;
}

void Game::SetPlayers()
{
	// Sets the number of players in the game
	RefreshScreen();

	allPlayers = "";

	int input = 0;

	while (true)
	{
		std::cout << "Please enter your total number of players (must be at least two).\n\n\n";

		std::cout << "INPUT: ";
		std::cin >> input;

		if (input >= 2)
		{
			break;
		}

		std::cout << "\nThat is not a valid number, you need at least two players!\n\n\n";
	}

	MakePlayers(input);
}

void Game::MakePlayers(int numberOfPlayers)
{
	// Creates all players in the game
	playerCount = numberOfPlayers;

	currentPlayerPtr = new Player[playerCount];

	for (int i = 0; i < playerCount; i++)
	{
		currentPlayerPtr->SetName(std::to_string(i + 1));

		currentPlayerPtr++;
	}

	currentPlayerPtr -= playerCount;
}

void Game::SetName(Player& player)
{
	// Set a player's name
	std::cout << "Please enter Player " << player.GetName() << "'s name.\n\n\n";

	std::string input;

	std::cout << "INPUT: ";
	std::cin >> input;

	player.SetName(input);

	std::cout << "\n\n";
}

void Game::SetNames()
{
	// Set all player's names
	RefreshScreen();

	allPlayers = "\n================================================================================================\n\n\nF I G H T !\n\n\nREMAINING PLAYERS:\n";

	for (int i = 0; i < playerCount; i++)
	{
		SetName(*currentPlayerPtr);

		allPlayers += std::to_string(i + 1) + ". " + currentPlayerPtr->GetName() + "\n";

		currentPlayerPtr++;
	}

	currentPlayerPtr -= playerCount;

	allPlayers += "\n\n================================================================================================\n\n\n";

	allTurns = "================================================================================================\n\n\nALL TURNS:\n\n";
}

void Game::Turn(int currentTurn)
{
	// Plays the current player's turn
	RefreshScreen();

	std::cout << currentPlayerPtr->GetName() << "'s Turn\n";
	std::cout << "HEALTH: " << currentPlayerPtr->Health() << "\n\n\n================================================================================================\n\n\n";

	int input = 0;

	while (true)
	{
		std::cout << "Please choose an action (by number).\n\nACTIONS:\n";
		std::cout << "1. Attack   (Damage an opponent for " << currentPlayerPtr->Stats(Attack) << " health)\n";
		std::cout << "2. Defend   (When hit, block " << currentPlayerPtr->Stats(Attack) * currentPlayerPtr->Stats(Defend) << " incoming damage, reflect back " << currentPlayerPtr->Stats(Attack) * (1 - currentPlayerPtr->Stats(Defend)) << " damage)\n";
		std::cout << "3. Heal     (Recover " << currentPlayerPtr->Stats(Heal) << " health, up to " << currentPlayerPtr->MaxHealth() << " max health)\n\n\n";

		std::cout << "INPUT: ";
		std::cin >> input;

		if (input == 1 || input == 2)
		{
			break;
		}
		else if (input == 3)
		{
			if (currentPlayerPtr->Health() < currentPlayerPtr->MaxHealth())
			{
				break;
			}
			else
			{
				std::cout << "\nYou are at max health.\n\n\n";
				continue;
			}
		}

		std::cout << "\nThat is not a valid action.\n\n\n";
	}

	currentPlayerPtr->SetAction((Action)input);

	if (input == 1)
	{
		RefreshScreen();

		std::cout << currentPlayerPtr->GetName() << "'s Turn\n";
		std::cout << "HEALTH: " << currentPlayerPtr->Health() << "\n\n\n================================================================================================\n\n\n";

		while (true)
		{
			input = 0;

			while (true)
			{
				std::cout << "Please choose a player to attack (by number).\n\n\n";

				std::cout << "INPUT: ";
				std::cin >> input;

				if (input == currentTurn + 1)
				{
					std::cout << "\nYou cannot attack yourself!\n\n\n";
					continue;
				}
				else if (input > 0 && input <= playerCount)
				{
					break;
				}

				std::cout << "\nThat is not a valid player. Check the player list for each player's number.\n\n\n";
			}

			currentPlayerPtr -= currentTurn;

			currentPlayerPtr += input - 1;

			bool isAlive = currentPlayerPtr->IsAlive();

			currentPlayerPtr -= input - 1;

			currentPlayerPtr += currentTurn;


			if (isAlive)
			{
				currentPlayerPtr->SetTarget(input);
				break;
			}

			std::cout << "\nThat player is no longer alive.\n\n\n";
		}
	}
}

std::string Game::EndRound()
{
	// Processes all actions and restarts the round with remaining players
	std::string playerName = "No Player";

	for (int i = 0; i < playerCount; i++)
	{
		if (!currentPlayerPtr->IsAlive())
		{
			currentPlayerPtr++;
			continue;
		}

		playerName = currentPlayerPtr->GetName();

		if (currentPlayerPtr->GetAction() == Attack)
		{
			int target = currentPlayerPtr->GetTarget();

			currentPlayerPtr -= i;

			currentPlayerPtr += target - 1;

			Player* enemy = currentPlayerPtr;

			currentPlayerPtr -= target - 1;

			currentPlayerPtr += i;

			currentPlayerPtr->Attack(*enemy);

			if (!enemy->IsDefending())
			{
				if (!enemy->IsAlive())
				{
					allTurns += std::to_string(currentTurn) + ". ";

					allTurns += playerName + " attacked and killed " + enemy->GetName() + ".\n\n";

					allTurns += std::to_string(currentTurn) + ". ";

					allTurns += enemy->GetName() + " died.\n\n";
				}
				else
				{					
					allTurns += std::to_string(currentTurn) + ". ";

					allTurns += playerName + " attacked " + enemy->GetName() + ".\n\n";

					allTurns += std::to_string(currentTurn) + ". ";

					allTurns += enemy->GetName() + " now has " + std::to_string(enemy->Health()) + " health.\n\n";
				}
			}
			else
			{
				allTurns += std::to_string(currentTurn) + ". ";

				allTurns += playerName + " attacked " + enemy->GetName() + " but " + enemy->GetName() + " was defending.\n\n";

				if (!enemy->IsAlive())
				{
					allTurns += std::to_string(currentTurn) + ". ";

					allTurns += enemy->GetName() + " died.\n\n";
				}
				else
				{
					allTurns += std::to_string(currentTurn) + ". ";

					allTurns += enemy->GetName() + " now has " + std::to_string(enemy->Health()) + " health.\n\n";

					if (currentPlayerPtr->IsAlive())
					{
						allTurns += std::to_string(currentTurn) + ". ";

						allTurns += currentPlayerPtr->GetName() + " now has " + std::to_string(currentPlayerPtr->Health()) + " health.\n\n";
					}
					else
					{
						allTurns += std::to_string(currentTurn) + ". ";

						allTurns += currentPlayerPtr->GetName() + " died.\n\n";
					}
				}
			}

			currentTurn++;
		}
		else if (currentPlayerPtr->GetAction() == Defend)
		{
			allTurns += std::to_string(currentTurn) + ". ";

			allTurns += currentPlayerPtr->GetName() + " defended themself.\n\n";

			currentTurn++;
		}
		else if (currentPlayerPtr->GetAction() == Heal)
		{
			currentPlayerPtr->Heal();

			allTurns += std::to_string(currentTurn) + ". ";

			allTurns += currentPlayerPtr->GetName() + " healed themself.\n\n";

			allTurns += std::to_string(currentTurn) + ". ";

			allTurns += currentPlayerPtr->GetName() + " now has " + std::to_string(currentPlayerPtr->Health()) + " health.\n\n";

			currentTurn++;
		}

		currentPlayerPtr++;
	}

	currentPlayerPtr -= playerCount;

	return playerName;
}

void Game::StartRound()
{
	// Starts a round
	remainingPlayers = playerCount;

	int totalRounds = 0;

	std::string winningPlayer = "";

	while (remainingPlayers > 1)
	{
		allTurns += "\nROUND " + std::to_string((totalRounds + 1));
		allTurns += "\n\n";

		for (int i = 0; i < playerCount; i++)
		{
			Turn(i);

			currentPlayerPtr++;
		}

		currentPlayerPtr -= playerCount;

		winningPlayer = EndRound();

		allPlayers = "\n================================================================================================\n\n\nF I G H T !\n\n\nREMAINING PLAYERS:\n";

		for (int i = 0; i < playerCount; i++)
		{
			if (!currentPlayerPtr->IsAlive())
			{
				remainingPlayers--;
				currentPlayerPtr++;
			}
			else
			{
				allPlayers += std::to_string(i + 1) + ". " + currentPlayerPtr->GetName() + "\n";

				currentPlayerPtr++;
			}
		}

		allPlayers += "\n\n================================================================================================\n\n\n";

		currentPlayerPtr -= playerCount;

		totalRounds++;
	}

	allPlayers = "";

	allTurns += "\n================================================================================================\n\n\n";

	RefreshScreen();

	std::cout << allTurns;

	std::cout << "THE WINNER IS " << winningPlayer << "!\n\n";

	std::cout << winningPlayer << " WON AFTER " << totalRounds << " ROUNDS!\n\n\n";

	std::cout << "INPUT: ";

	std::cin >> winningPlayer;
}

void Game::Play()
{
	// Starts and plays the game
	SetPlayers();

	SetNames();

	RefreshScreen();

	StartRound();
}

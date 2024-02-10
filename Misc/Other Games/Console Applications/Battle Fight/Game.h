#pragma once
#include "Player.h"

// Game Class
class Game
{
public:

	// Public variables and functions
	Game();									// Constructor
	~Game();								// Deconstructor

	void Play();							// Starts and plays the game

private:

	// Private variables and functions
	int playerCount;
	Player* currentPlayerPtr;
	std::string allPlayers;
	int remainingPlayers;
	std::string allTurns;
	int currentTurn;

	void RefreshScreen();					// Refreshes the screen and prints the title

	void MakePlayers(int numberOfPlayers);	// Creates all players in the game
	void SetPlayers();						// Sets the number of players in the game

	void SetName(Player& player);			// Set a player's name
	void SetNames();						// Set all player's names

	void Turn(int turnNumber);				// Plays the current player's turn
	std::string EndRound();					// Restarts the round after all turns end
	void StartRound();						// Starts a round
};

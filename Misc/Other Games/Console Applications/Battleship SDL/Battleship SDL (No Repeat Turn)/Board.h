#pragma once
#include "Player.h"
#include "SDL Button.h"

class Board {
public:
	Board();
	~Board();
	enum SpaceType { //This enum represents the different types of spaces that will exist in the game
		water,
		Carrier,
		Battleship,
		Cruiser,
		Submarine,
		Destroyer,
		damagedWater,
		damagedShip,
		damagedCarrier,
		damagedBattleship,
		damagedCruiser,
		damagedSubmarine,
		damagedDestroyer
	};
	bool IsSpaceOccupied(int x, int y, int boardOwner);
	SpaceType GetTypeFromChar(char space);
	char GetCharFromType(SpaceType type);
	SDL_Color SpaceToColor(char space);
	void DisplayBoard(int currentPlayer, bool showAllies, std::vector<std::vector<SDL_Button*>> buttons);
	SpaceType GetShipOnSpace(int x, int y, int boardOwner);
	bool PlaceShip(int x1, int y1, int x2, int y2, int shipLength, SpaceType shipType, int boardOwner);
	bool ChangeBoardSpace(int x, int y, int boardOwner, Player& player);

private:
	char gameBoard1[10][10];
	char gameBoard2[10][10];
};

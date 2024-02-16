#pragma once
#include "Player.h"

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
	bool IsSpaceOccupied(int x, char y, int boardOwner);
	int GetIntegerY(char y);
	char GetCharacterY(int y);
	SpaceType GetTypeFromChar(char space);
	char GetCharFromType(SpaceType type);
	void DisplayBoard(int currentPlayer, bool showAllies);
	SpaceType GetShipOnSpace(int x, char y, int boardOwner);
	bool PlaceShip(int x1, char y1, int x2, char y2, int shipLength, SpaceType shipType, int boardOwner);
	bool ChangeBoardSpace(int x, char y, int boardOwner, Player& player);

private:
	char gameBoard1[10][10];
	char gameBoard2[10][10];
};

#include <iostream>
#include "Board.h"

Board::Board()
{
	/*
	* Initialize the game boards
	*/

	for (int y = 0; y < 10; y++)
	{
		for (int x = 0; x < 10; x++)
		{
			gameBoard1[x][y] = '.';
			gameBoard2[x][y] = '.';
		}
	}
}

Board::~Board()
{

}

bool Board::IsSpaceOccupied(int x, char y, int boardOwner)
{
	/*
	* Check if space is valid, then return true or false if there is a ship there or not
	*/
	if (boardOwner == 1)
	{
		return gameBoard1[x][y] != '.';
	}
	else
	{
		return gameBoard2[x][y] != '.';
	}
}

int Board::GetIntegerY(char y)
{
	/*
	* Return an integer for each appropriate character used by the Y-axis
	* If invalid, return -1
	*/
	char input = std::toupper(y);

	switch (input)
	{
	default:

		return -1;

	case 'A':

		return 9;

	case 'B':

		return 8;

	case 'C':

		return 7;

	case 'D':

		return 6;

	case 'E':

		return 5;

	case 'F':

		return 4;

	case 'G':

		return 3;

	case 'H':

		return 2;

	case 'I':

		return 1;

	case 'J':

		return 0;
	}
}

char Board::GetCharacterY(int y)
{
	/*
	* Return an integer for each appropriate character used by the Y-axis
	* If invalid, return -1
	*/
	switch (y)
	{
	default:

		return '\0';

	case 1:

		return 'A';

	case 2:

		return 'B';

	case 3:

		return 'C';

	case 4:

		return 'D';

	case 5:

		return 'E';

	case 6:

		return 'F';

	case 7:

		return 'G';

	case 8:

		return 'H';

	case 9:

		return 'I';

	case 10:

		return 'J';
	}
}

Board::SpaceType Board::GetTypeFromChar(char space)
{
	/*
	* Given the space parameter, return the appropriate SpaceType
	*/

	switch (space)
	{
	default:

		return water;

	case '.':

		return water;

	case 'X':

		return damagedWater;

	case 'C':

		return Carrier;

	case 'B':

		return Battleship;

	case 'S':

		return Submarine;

	case 'R':

		return Cruiser;

	case 'D':

		return Destroyer;

	case 'O':

		return damagedShip;

	case 'c':

		return damagedCarrier;

	case 'b':

		return damagedBattleship;

	case 's':

		return damagedSubmarine;

	case 'r':

		return damagedCruiser;

	case 'd':

		return damagedDestroyer;
	}
}

char Board::GetCharFromType(SpaceType type)
{
	/*
	* Given the type parameter, return the appropriate char symbol
	*/

	switch (type)
	{
	default:

		return '.';

	case water:

		return '.';

	case damagedWater:

		return 'X';

	case Carrier:

		return 'C';

	case Battleship:

		return 'B';

	case Submarine:

		return 'S';

	case Cruiser:

		return 'R';

	case Destroyer:

		return 'D';

	case damagedShip:

		return 'O';

	case damagedCarrier:

		return 'c';

	case damagedBattleship:

		return 'b';

	case damagedSubmarine:

		return 's';

	case damagedCruiser:

		return 'r';

	case damagedDestroyer:

		return 'd';
	}
}

void Board::DisplayBoard(int currentPlayer, bool showAllies)
{
	/*
	* Depending on who's turn it is you need to display the board data in a certain way
	* Remember that the top board should always have less data shown than the bottom!
	* Clearing the console using system("CLS") might also be a good idea
	*/

	std::system("CLS");

	std::cout << "PLAYER " << currentPlayer << std::endl;

	if (!showAllies)
	{
		currentPlayer -= 1;
	}

	if (currentPlayer == 1)
	{
		for (int y = 9; y > -1; y--)
		{
			std::cout << GetCharacterY(10 - y) << " ";

			for (int x = 0; x < 10; x++)
			{
				if (showAllies)
				{
					std::cout << gameBoard1[x][y] << " ";
				}
				else if (gameBoard1[x][y] != 'C' && gameBoard1[x][y] != 'B' && gameBoard1[x][y] != 'S' && gameBoard1[x][y] != 'R' && gameBoard1[x][y] != 'D')
				{
					std::cout << gameBoard1[x][y] << " ";
				}
				else
				{
					std::cout << ". ";
				}
			}

			std::cout << std::endl;
		}
	}
	else
	{
		for (int y = 9; y > -1; y--)
		{
			std::cout << GetCharacterY(10 - y) << " ";

			for (int x = 0; x < 10; x++)
			{
				if (showAllies)
				{
					std::cout << gameBoard2[x][y] << " ";
				}
				else if (gameBoard2[x][y] != 'C' && gameBoard2[x][y] != 'B' && gameBoard2[x][y] != 'S' && gameBoard2[x][y] != 'R' && gameBoard2[x][y] != 'D')
				{
					std::cout << gameBoard2[x][y] << " ";
				}
				else
				{
					std::cout << ". ";
				}
			}

			std::cout << std::endl;
		}
	}

	std::cout << "  ";

	for (int i = 0; i < 10; i++)
	{
		std::cout << (i + 1) << ' ';
	}

	std::cout << std::endl;
}

Board::SpaceType Board::GetShipOnSpace(int x, char y, int boardOwner)
{
	/*
	* Return the type of space that is found at the given coordinates for the indicated board
	*/

	if (boardOwner == 1)
	{
		return GetTypeFromChar(gameBoard1[x][y]);
	}
	else
	{
		return GetTypeFromChar(gameBoard2[x][y]);
	}
}

enum Direction { Up, Down, Left, Right };

bool Board::PlaceShip(int x1, char y1, int x2, char y2, int shipLength, Board::SpaceType shipType, int boardOwner)
{
	/*
	* This function is a big boy (very important)
	*
	* Given the coordinates, ship length, ship type, and who owns the board, place the ship segments onto the grid.
	* Return true if ship is placed successfully
	* Return false if ship cannot be placed successfully
	*/

	x1 -= 1;

	x2 -= 1;

	y1 = GetIntegerY(y1);

	y2 = GetIntegerY(y2);

	Direction Direction;

	if (y1 == y2 && x1 < x2)
	{
		Direction = Right;
	}
	else if (y1 == y2 && x1 > x2)
	{
		Direction = Left;
	}
	else if (x1 == x2 && y1 < y2)
	{
		Direction = Up;
	}
	else if (x1 == x2 && y1 > y2)
	{
		Direction = Down;
	}
	else
	{
		return false;
	}

	if (boardOwner == 1)
	{
		switch (Direction)
		{
		case Up:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard1[x1][y1 + i] != '.' || y1 + i > 9)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard1[x1][y1 + i] = GetCharFromType(shipType);
			}

			return true;

		case Down:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard1[x1][y1 - i] != '.' || y1 - i < 0)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard1[x1][y1 - i] = GetCharFromType(shipType);
			}

			return true;

		case Left:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard1[x1 - i][y1] != '.' || x1 - i < 0)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard1[x1 - i][y1] = GetCharFromType(shipType);
			}

			return true;

		case Right:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard1[x1 + i][y1] != '.' || x1 + i > 9)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard1[x1 + i][y1] = GetCharFromType(shipType);
			}

			return true;
		}
	}
	else
	{
		switch (Direction)
		{
		case Up:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard2[x1][y1 + i] != '.' || y1 + i > 9)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard2[x1][y1 + i] = GetCharFromType(shipType);
			}

			return true;

		case Down:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard2[x1][y1 - i] != '.' || y1 - i < 0)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard2[x1][y1 - i] = GetCharFromType(shipType);
			}

			return true;

		case Left:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard2[x1 - i][y1] != '.' || x1 - i < 0)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard2[x1 - i][y1] = GetCharFromType(shipType);
			}

			return true;

		case Right:

			for (int i = 0; i < shipLength; i++)
			{
				if (gameBoard2[x1 + i][y1] != '.' || x1 + i > 9)
				{
					return false;
				}
			}

			for (int i = 0; i < shipLength; i++)
			{
				gameBoard2[x1 + i][y1] = GetCharFromType(shipType);
			}

			return true;
		}

	}

	return false;
}

bool Board::ChangeBoardSpace(int x, char y2, int boardOwner, Player& player)
{
	/*
	* This function is a big boy (very important)
	*
	* Given the coordinates and board owner, make a change (from an attack) to the grid
	* Use the player parameter to make changes to the player's stats
	*/

	int y = GetIntegerY(y2);

	if (boardOwner == 1)
	{
		if (gameBoard2[x][y] != 'X' && gameBoard2[x][y] != 'O' && gameBoard2[x][y] != 'c' && gameBoard2[x][y] != 'b' && gameBoard2[x][y] != 's' && gameBoard2[x][y] != 'r' && gameBoard2[x][y] != 'd')
		{
			if (gameBoard2[x][y] == '.')
			{
				gameBoard2[x][y] = 'X';
				return false;
			}
			else
			{
				//gameBoard2[x][y] = std::tolower(gameBoard2[x][y]);
				player.ShipDamaged(gameBoard2[x][y]);
				gameBoard2[x][y] = 'O';
				return true;
			}
		}
	}
	else
	{
		if (gameBoard1[x][y] != 'X' && gameBoard1[x][y] != 'O' && gameBoard1[x][y] != 'c' && gameBoard1[x][y] != 'b' && gameBoard1[x][y] != 's' && gameBoard1[x][y] != 'r' && gameBoard1[x][y] != 'd')
		{
			if (gameBoard1[x][y] == '.')
			{
				gameBoard1[x][y] = 'X';
				return false;
			}
			else
			{
				//gameBoard1[x][y] = std::tolower(gameBoard1[x][y]);
				player.ShipDamaged(gameBoard1[x][y]);
				gameBoard1[x][y] = 'O';
				return true;
			}
		}
	}
}

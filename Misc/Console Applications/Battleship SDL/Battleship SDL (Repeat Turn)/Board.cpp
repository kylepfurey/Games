#pragma once
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

bool Board::IsSpaceOccupied(int x, int y, int boardOwner)
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

SDL_Color Board::SpaceToColor(char space)
{
	switch (space)
	{
	default:

		// Blue
		return NewColor(0, 0, 255);

	case '.':

		// Blue
		return NewColor(0, 0, 255);

	case 'X':

		// Red
		return NewColor(255, 0, 0);

	case 'C':

		// Purple
		return NewColor(255, 0, 255);

	case 'B':

		// Purple
		return NewColor(255, 0, 255);

	case 'S':

		// Purple
		return NewColor(255, 0, 255);

	case 'R':

		// Purple
		return NewColor(255, 0, 255);

	case 'D':

		// Purple
		return NewColor(255, 0, 255);

	case 'O':

		// Green
		return NewColor(0, 255, 0);

	case 'c':

		// Green
		return NewColor(0, 255, 0);

	case 'b':

		// Green
		return NewColor(0, 255, 0);

	case 's':

		// Green
		return NewColor(0, 255, 0);

	case 'r':

		// Green
		return NewColor(0, 255, 0);

	case 'd':

		// Green
		return NewColor(0, 255, 0);
	}
}

void Board::DisplayBoard(int currentPlayer, bool showAllies, std::vector<std::vector<SDL_Button*>> buttons)
{
	if (!showAllies)
	{
		currentPlayer -= 1;
	}

	if (currentPlayer == 1)
	{
		for (int y = 0; y < buttons.size(); y++)
		{
			for (int x = 0; x < buttons[y].size(); x++)
			{
				if (showAllies)
				{
					buttons[y][x]->SetColor(SpaceToColor(gameBoard1[x][y]), buttons[y][x]->GetOutlined());
				}
				else if (gameBoard1[x][y] != 'C' && gameBoard1[x][y] != 'B' && gameBoard1[x][y] != 'S' && gameBoard1[x][y] != 'R' && gameBoard1[x][y] != 'D')
				{
					buttons[y][x]->SetColor(SpaceToColor(gameBoard1[x][y]), buttons[y][x]->GetOutlined());
				}
				else
				{
					buttons[y][x]->SetColor(NewColor(0, 0, 255), buttons[y][x]->GetOutlined());
				}
			}
		}
	}
	else
	{
		for (int y = 0; y < buttons.size(); y++)
		{
			for (int x = 0; x < buttons[y].size(); x++)
			{
				if (showAllies)
				{
					buttons[y][x]->SetColor(SpaceToColor(gameBoard2[x][y]), buttons[y][x]->GetOutlined());
				}
				else if (gameBoard2[x][y] != 'C' && gameBoard2[x][y] != 'B' && gameBoard2[x][y] != 'S' && gameBoard2[x][y] != 'R' && gameBoard2[x][y] != 'D')
				{
					buttons[y][x]->SetColor(SpaceToColor(gameBoard2[x][y]), buttons[y][x]->GetOutlined());
				}
				else
				{
					buttons[y][x]->SetColor(NewColor(0, 0, 255), buttons[y][x]->GetOutlined());
				}
			}
		}
	}
}

Board::SpaceType Board::GetShipOnSpace(int x, int y, int boardOwner)
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

bool Board::PlaceShip(int x1, int y1, int x2, int y2, int shipLength, Board::SpaceType shipType, int boardOwner)
{
	/*
	* This function is a big boy (very important)
	*
	* Given the coordinates, ship length, ship type, and who owns the board, place the ship segments onto the grid.
	* Return true if ship is placed successfully
	* Return false if ship cannot be placed successfully
	*/

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

bool Board::ChangeBoardSpace(int x, int y, int boardOwner, Player& player)
{
	/*
	* This function is a big boy (very important)
	*
	* Given the coordinates and board owner, make a change (from an attack) to the grid
	* Use the player parameter to make changes to the player's stats
	*/

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

	return false;
}

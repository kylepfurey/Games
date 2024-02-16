#include <iostream>
#include <string>
#include "Board.h"
#include "Player.h"

using namespace std;

int SwitchPlayers(int previousPlayer) {
	/*
	This function should return the number for the next player to have their turn
	*/

	if (previousPlayer == 1)
	{
		return 2;
	}

	return 1;
}

void ShipPlacementInput(int shipLength, Board& board, Board::SpaceType shipType, int currentPlayer) {
	board.DisplayBoard(currentPlayer, true);
	cout << "Please place a ship of length " << shipLength << "." << endl;
	string inputX, inputY;
	//GET X1
	cout << "Input starting X coordinate... ";
	cin >> inputX;
	int x1 = stoi(inputX);
	while (x1 < 1 || x1 > 10) {
		cout << endl;
		cout << "Invalid input. Please input a number 1-10... ";
		cin >> inputX;
		int x1 = stoi(inputX);
	}

	cout << endl;
	//GET Y1
	cout << "Input starting Y coordinate... ";
	cin >> inputY;
	char y1 = static_cast<char>(inputY[0]);
	while (board.GetIntegerY(y1) < 0 || board.GetIntegerY(y1) > 9) {
		cout << endl;
		cout << "Invalid input. Please input a capital letter A-J... ";
		cin >> inputY;
		y1 = static_cast<char>(inputY[0]);
	}

	cout << endl;
	//GET X2
	cout << "Input ending X coordinate... ";
	cin >> inputX;
	int x2 = stoi(inputX);
	while (x2 < 1 || x2 > 10) {
		cout << endl;
		cout << "Invalid input. Please input a number 1-10... ";
		cin >> inputX;
		x2 = stoi(inputX);
	}

	cout << endl;

	//GET Y2
	cout << "Input ending Y coordinate... ";
	cin >> inputY;
	char y2 = static_cast<char>(inputY[0]);
	while (board.GetIntegerY(y2) < 0 || board.GetIntegerY(y2) > 9) {
		cout << endl;
		cout << "Invalid input. Please input a capital letter A-J... ";
		cin >> inputY;
		y2 = static_cast<char>(inputY[0]);
	}

	cout << endl;

	//PLACE ON BOARD
	if (!board.PlaceShip(x1, y1, x2, y2, shipLength, shipType, currentPlayer)) {
		//invalid placement, try again
		ShipPlacementInput(shipLength, board, shipType, currentPlayer);
		cout << "That ship is not in a valid location!";

	}
}

void PlaceShips(Board& board, int currentPlayer)
{
	for (int i = 0; i < 5; i++)
	{
		int length = 0;

		switch (i)
		{
		case 0:

			length = 5;

			break;

		case 1:

			length = 4;

			break;

		case 2:

			length = 3;

			break;

		case 3:

			length = 3;

			break;

		case 4:

			length = 2;

			break;
		}

		ShipPlacementInput(length, board, (Board::SpaceType)(i + 1), currentPlayer);
	}
}

void AttackInput(Board& board, int currentPlayer, Player& player) {
	board.DisplayBoard(currentPlayer, false);
	string inputX, inputY;
	//GET X1
	cout << "Input attack X coordinate... ";
	cin >> inputX;
	int x1 = stoi(inputX);
	while (x1 < 1 || x1 > 10) {
		cout << endl;
		cout << "Invalid input. Please input a number 1-10... ";
		cin >> inputX;
		x1 = stoi(inputX);
	}
	x1--;

	cout << endl;
	//GET Y1
	cout << "Input attack Y coordinate... ";
	cin >> inputY;
	char y1 = static_cast<char>(inputY[0]);
	while (board.GetIntegerY(y1) < 0 || board.GetIntegerY(y1) > 9) {
		cout << endl;
		cout << "Invalid input. Please input a capital letter A-J... ";
		cin >> inputY;
		cin >> inputY;
		y1 = static_cast<char>(inputY[0]);
	}

	cout << endl;

	if (board.ChangeBoardSpace(x1, y1, currentPlayer, player) && player.shipCount > 0)
	{
		AttackInput(board, currentPlayer, player);
	}
}

int main(int argc, char* argv[]) {
	//game introduction
	cout << "BATTLESHIP" << endl;
	cout << "This is a two player game. Each player should take their turn without any screen peeking." << endl;
	Board BattleshipBoard;
	Player player1, player2;
	int currentPlayer = 1;

	//player 1 board setup
	PlaceShips(BattleshipBoard, currentPlayer);

	currentPlayer = SwitchPlayers(currentPlayer);

	//player 2 board setup
	PlaceShips(BattleshipBoard, currentPlayer);

	currentPlayer = SwitchPlayers(currentPlayer);

	while (player1.shipCount > 0 && player2.shipCount > 0) {
		//GAMING

		/*
		This is where the main game loop will take place. Make sure to:
			* switch players
			* display the board
			* get input for attack
		*/

		if (currentPlayer == 1)
		{
			AttackInput(BattleshipBoard, currentPlayer, player1);
		}
		else
		{
			AttackInput(BattleshipBoard, currentPlayer, player2);
		}

		currentPlayer = SwitchPlayers(currentPlayer);
	}

	currentPlayer = SwitchPlayers(currentPlayer);

	BattleshipBoard.DisplayBoard(currentPlayer, false);

	//VICTORY CONDITION CHECK
	if (player1.shipCount > 0)
	{
		cout << "Player 2 is Victorious!" << endl;
	}
	else
	{
		cout << "Player 1 is Victorious!" << endl;
	}

	return 0;
}

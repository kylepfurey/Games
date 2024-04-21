
// Battleship in SDL
// by Kyle Furey and Allen Cantin

#include <vector>

#include "Board.h"
#include "Player.h"

#include "SDL Macros.h"
#include "SDL Button.h"

using namespace std;


// SETTINGS

// Whether a hit allows the player to repeat their turn
const bool repeat_turn_on_hit = false;


// TEXT

// Print to in-game log
#define PRINT(string) text.SetText(string, text.GetTextColor());

// Display the current turn
#define TURN(player_number) turn.SetText("Player " + std::to_string(player_number) + "'s Turn", turn.GetTextColor());


// INPUT

// X coordinate macro
#define X coords[0]

// Y coordinate macro
#define Y coords[1]

// Returns the x and y of a clicked button in a 2D series of buttons, or -1, -1 if no button was clicked
vector<int> GetInput(vector<vector<SDL_Button*>> buttons, int x, int y)
{
	vector<int> coordinates = vector<int>{ -1, -1 };

	for (int j = 0; j < buttons.size(); j++)
	{
		int i = SDL_Button::GetOverlapped(buttons[j], x, y);

		if (i != -1)
		{
			coordinates[0] = i;
			coordinates[1] = j;

			return coordinates;
		}
	}

	return coordinates;
}

// SDL arguments macro
#define SDL_ARGS bool& quit, vector<int>& coords, vector<vector<SDL_Button*>>& buttons, vector<SDL_Button*>& assets, vector<SDL_Texture*>& textures, SDL_Renderer* renderer, vector<SDL_Rect*>& transforms, Mix_Chunk* hit, Mix_Chunk* lose, Mix_Chunk* miss, Mix_Chunk* select

// Loops until the input from the player is complete (not to be called directly, use INPUT macro instead after creating a window.
void ObtainInput(SDL_ARGS)
{
	// Create event
	SDL_Event SDL_event;

	// Store break variable
	bool end = false;

	while (!end)
	{
		while (SDL_PollEvent(&SDL_event))
		{
			if (SDL_event.type == SDL_QUIT)
			{
				quit = true;
				end = true;
				break;
			}

			// Quit check
			INPUT_KEY_DOWN(SDLK_ESCAPE)
			{
				quit = true;
				end = true;
				LOOP_QUIT;
			}
			INPUT_END_KEY;

			// Mouse movement
			INPUT_MOUSE_MOVE;
			{
				for (int y = 0; y < buttons.size(); y++)
				{
					for (int x = 0; x < buttons[y].size(); x++)
					{
						if (buttons[y][x]->IsOverlapped(mouse.x, mouse.y))
						{
							buttons[y][x]->SetColor(buttons[y][x]->GetColor(), true);
						}
						else
						{
							buttons[y][x]->SetColor(buttons[y][x]->GetColor(), false);
						}
					}
				}
			}
			INPUT_END_MOUSE_MOVE;

			// Mouse button
			INPUT_MOUSE_BUTTON_DOWN;
			{
				coords = GetInput(buttons, mouse.x, mouse.y);

				if (X != -1 && Y != -1)
				{
					end = true;
					LOOP_QUIT;
				}
			}
			INPUT_END_MOUSE_BUTTON;

			// Render assets
			for (int i = 0; i < assets.size(); i++)
			{
				assets[i]->Render();
			}

			for (int i = 0; i < textures.size(); i++)
			{
				SDL_RenderCopy(renderer, textures[i], 0, transforms[i]);
			}

			SDL_RenderPresent(renderer);

			SDL_RenderClear(renderer);
		}
	}
}

// SDL parameters macro
#define SDL_PARAMS quit, coords, buttons, assets, textures, renderer, transforms, hit, lose, miss, select

// Input macro
#define INPUT ObtainInput(SDL_PARAMS);

// Return on quit macro
#define RETURN if (quit) { return; }

// Break on quit macro
#define BREAK if (quit) { break; }


// GAME FUNCTIONS

// Swaps players
int SwitchPlayers(int previousPlayer, SDL_Button& turn)
{
	/*
	This function should return the number for the next player to have their turn
	*/

	if (previousPlayer == 1)
	{
		TURN(2);

		return 2;
	}

	TURN(1);

	return 1;
}

// Places a ship
void ShipPlacementInput(int shipLength, Board& board, Board::SpaceType shipType, int currentPlayer, SDL_Button& text, SDL_ARGS) {
	board.DisplayBoard(currentPlayer, true, buttons);
	PRINT("Please place a ship of length " + std::to_string(shipLength) + ".");

	// GET LOCATION 1
	INPUT;
	RETURN;
	PLAY_SOUND(select, 0, -1);
	buttons[Y][X]->SetColor(NewColor(0,255,0), buttons[Y][X]->GetOutlined());
	int x1 = X;
	int y1 = Y;

	// GET LOCATION 2
	INPUT;
	RETURN;
	PLAY_SOUND(select, 0, -1);
	buttons[Y][X]->SetColor(NewColor(0, 255, 0), buttons[Y][X]->GetOutlined());
	int x2 = X;
	int y2 = Y;

	//PLACE ON BOARD
	if (!board.PlaceShip(x1, y1, x2, y2, shipLength, shipType, currentPlayer))
	{
		//invalid placement, try again
		ShipPlacementInput(shipLength, board, shipType, currentPlayer, text, SDL_PARAMS);
	}
}

// Places all ships on the boards
void PlaceShips(Board& board, int currentPlayer, SDL_Button& text, SDL_ARGS)
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

		ShipPlacementInput(length, board, (Board::SpaceType)(i + 1), currentPlayer, text, SDL_PARAMS);
		RETURN;
	}
}

// Receives input to damage a ship
void AttackInput(Board& board, int currentPlayer, Player& player, SDL_Button& text, SDL_ARGS) {
	board.DisplayBoard(currentPlayer, false, buttons);

	// GET LOCATION
	INPUT;
	RETURN;
	int x1 = X;
	int y1 = Y;

	if (board.ChangeBoardSpace(x1, y1, currentPlayer, player) && player.shipCount > 0)
	{
		PRINT("DIRECT HIT!");

		PLAY_SOUND(hit, 0, -1);

		if (repeat_turn_on_hit)
		{
			AttackInput(board, currentPlayer, player, text, SDL_PARAMS);
		}
	}
	else
	{
		PRINT("MISS!");

		PLAY_SOUND(miss, 0, -1);
	}
}


// MAIN

// Game start function
int main(int argc, char* argv[])
{
	// Initialize SDL
	CREATE_WINDOW("Battleship", 1920, 1080, 0, 0, 0);
	START_TEXT;
	START_AUDIO(1);
	SET_VOLUME(10);

	// Initialize input variables
	vector<int> coords;
	bool quit = false;

	// Store visualized assets
	vector<SDL_Button*> assets = vector<SDL_Button*>();

	// Create grid
	SDL_Button grid = SDL_Button(renderer, "Images/Grid.bmp", window_width / 2, window_height / 2, 1, 1);
	grid.transform->x -= grid.transform->w / 2;
	grid.transform->y -= grid.transform->h / 2;
	assets.push_back(&grid);

	// Create buttons
	vector<vector<SDL_Button*>> buttons;

	for (int y = 0; y < 10; y++)
	{
		vector<SDL_Button*> row;

		buttons.push_back(row);

		for (int x = 0; x < 10; x++)
		{
			// Allocate buttons (vector will deallocate this)
			buttons[y].push_back(new SDL_Button(renderer, NewColor(255, 255, 255), false,
				grid.transform->x + grid.transform->w / 21 + grid.transform->w / 21 * 2 * x,
				grid.transform->y + grid.transform->h / 21 + grid.transform->h / 21 * 2 * y,
				grid.transform->w / 21,
				grid.transform->h / 21));

			assets.push_back(buttons[y][x]);
		}
	}

	// Create display text
	SDL_Button title = SDL_Button(renderer, NewColor(0, 0, 0), false, window_width / 2 - 200, 0, 400, 100, "Images/Font.ttf", 26, "BATTLESHIP", NewColor(255, 0, 0));
	assets.push_back(&title);

	SDL_Button turn = SDL_Button(renderer, NewColor(0, 0, 0), false, 100, 50, 300, 50, "Images/Font.ttf", 26, "Player 1's Turn", NewColor(255, 255, 255));
	assets.push_back(&turn);

	SDL_Button text = SDL_Button(renderer, NewColor(0, 0, 0), false, window_width / 2 - 500, window_height - 100, 1000, 100, "Images/Font.ttf", 26, "Player 1, select your ship spots.", NewColor(255, 255, 255));
	assets.push_back(&text);

	// Create sounds
	LOAD_SOUND("Sounds/Hit.wav", hit);
	LOAD_SOUND("Sounds/Lose.wav", lose);
	LOAD_SOUND("Sounds/Miss.wav", miss);
	LOAD_SOUND("Sounds/Select.wav", select);

	// Game loop
	while (!quit)
	{
		// Reset colors of buttons
		for (int y = 0; y < buttons.size(); y++)
		{
			for (int x = 0; x < buttons[y].size(); x++)
			{
				buttons[y][x]->SetColor(NewColor(0, 0, 255), false);
			}
		}

		//game introduction
		Board BattleshipBoard;
		Player player1, player2;
		int currentPlayer = 1;

		TURN(1);

		//player 1 board setup
		PlaceShips(BattleshipBoard, currentPlayer, text, SDL_PARAMS);

		currentPlayer = SwitchPlayers(currentPlayer, turn);

		BREAK;

		//player 2 board setup
		PlaceShips(BattleshipBoard, currentPlayer, text, SDL_PARAMS);

		currentPlayer = SwitchPlayers(currentPlayer, turn);

		BREAK;

		PRINT("Time to fight!");

		while (player1.shipCount > 0 && player2.shipCount > 0)
		{
			if (currentPlayer == 1)
			{
				AttackInput(BattleshipBoard, currentPlayer, player1, text, SDL_PARAMS);
			}
			else
			{
				AttackInput(BattleshipBoard, currentPlayer, player2, text, SDL_PARAMS);
			}

			BREAK;

			currentPlayer = SwitchPlayers(currentPlayer, turn);
		}

		BREAK;

		currentPlayer = SwitchPlayers(currentPlayer, turn);

		BattleshipBoard.DisplayBoard(currentPlayer, false, buttons);

		PLAY_SOUND(lose, 0, -1);

		//VICTORY CONDITION CHECK
		if (player1.shipCount > 0)
		{
			PRINT("Player 2 is victorious!");
		}
		else
		{
			PRINT("Player 1 is victorious!");
		}

		INPUT;
		BREAK;
	}

	// Destroy buttons
	for (int i = 0; i < assets.size(); i++)
	{
		assets[i]->Destroy();
	}

	// Destroy sounds
	REMOVE_SOUND(hit);
	REMOVE_SOUND(lose);
	REMOVE_SOUND(miss);
	REMOVE_SOUND(select);

	// Deallocate memory
	CLOSE_AUDIO;
	CLOSE_TEXT;
	CLOSE_WINDOW;

	return 0;
}

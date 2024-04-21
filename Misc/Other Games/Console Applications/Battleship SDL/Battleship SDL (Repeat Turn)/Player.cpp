#pragma once
#include "Player.h"

Player::Player()
{
	/*
	*Initialize the variables for the player's ships
	*/
	shipCount = 5;
	CarrierHealth = 5;
	BattleshipHealth = 4;
	CruiserHealth = 3;
	SubmarineHealth = 3;
	DestroyerHealth = 2;
}

Player::~Player()
{

}

void Player::ShipDamaged(char ship)
{
	/*
	* Depending on the character code parameter, the appropriate ship should take damage
	*/

	switch (ship)
	{
	default:

		return;

	case 'C':

		CarrierHealth--;

		if (CarrierHealth == 0)
		{
			shipCount--;
		}

		return;

	case 'B':

		BattleshipHealth--;

		if (BattleshipHealth == 0)
		{
			shipCount--;
		}

		return;

	case 'S':

		SubmarineHealth--;

		if (SubmarineHealth == 0)
		{
			shipCount--;
		}

		return;

	case 'R':

		CruiserHealth--;

		if (CruiserHealth == 0)
		{
			shipCount--;
		}

		return;

	case 'D':

		DestroyerHealth--;

		if (DestroyerHealth == 0)
		{
			shipCount--;
		}

		return;
	}
}

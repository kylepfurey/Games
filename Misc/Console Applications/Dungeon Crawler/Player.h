
// Player Stats

#pragma once
#include <iostream>

// Starting health
#define DEFAULT_HEALTH 100

// Health
float player_health;

// Damage the player
float& damage(float damage)
{
	std::cout << "YOU TOOK " << damage << " DAMAGE!\n";

	player_health -= damage;

	if (player_health < 0)
	{
		player_health = 0;
	}

	return player_health;
}

// Heal the player
float& heal(float healing)
{
	std::cout << "YOU HEALED " << healing << " HEALTH!\n\n";

	player_health += healing;

	if (player_health > DEFAULT_HEALTH)
	{
		player_health = DEFAULT_HEALTH;
	}

	return player_health;
}

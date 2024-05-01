#include "Player.h"

// Player Functions
Player::Player()
{
	// Constructor
	name = "";
	currentAction = None;
	target = 0;

	alive = true;
	health = 10;
	maxHealth = 15;

	attackPower = 3;
	isDefending = false;
	defendMultiplier = 0.4f;
	healAmount = 2;
}

Player::~Player()
{
	// Deconstructor

}

std::string Player::GetName()
{
	// Get this player's name
	return name;
}

void Player::SetName(std::string newName)
{
	// Sets this player's name
	name = newName;
}

bool Player::IsAlive()
{
	// Returns whether this player is alive or not
	return alive;
}

bool Player::IsDefending()
{
	// Returns whether this player is defending this turn or not
	return isDefending;
}

Action Player::GetAction()
{
	// Gets this player's current action
	return currentAction;
}

void Player::Defend()
{
	// Reflects damage back at the opponent
	isDefending = true;
}

void Player::SetAction(Action newAction)
{
	// Set this player's new action
	currentAction = newAction;

	if (newAction == Action::Defend)
	{
		Defend();
	}
	else
	{
		isDefending = false;
	}
}

float Player::Health()
{
	// Gets this player's health
	return health;
}

float Player::MaxHealth()
{
	// Gets this player's maximum health
	return maxHealth;
}

void Player::DeathCheck()
{
	// Checks if this player is currently dead
	if (health <= 0)
	{
		health = 0;
		alive = false;
	}
}

void Player::Damage(float damage)
{
	// Damages this player's health by a specific amount (ignores defense)
	health -= damage;

	DeathCheck();
}

void Player::Damage(Player& attacker)
{
	// Damages this player's health (reflects damage if defending)
	if (isDefending)
	{
		health -= attacker.attackPower * defendMultiplier;

		DeathCheck();

		if (alive)
		{
			attacker.Damage(attacker.attackPower * (1 - defendMultiplier));
		}
	}
	else
	{
		health -= attacker.attackPower;

		DeathCheck();
	}
}

void Player::SetTarget(int targetIndex)
{
	// Sets this player's current target
	target = targetIndex;
}

float Player::Stats(Action statType)
{
	// Gets this player's stat of the given action
	switch (statType)
	{
	case Action::Attack:

		return attackPower;

		break;

	case Action::Defend:

		return defendMultiplier;

		break;

	case Action::Heal:

		return healAmount;

		break;

	default:

		return 0;

		break;
	}
}

int Player::GetTarget()
{
	// Get this player's target
	return target;
}

void Player::Attack(Player& enemy)
{
	// Attacks another player's health
	enemy.Damage(*this);
}

void Player::Overheal()
{
	// Checks if this player has more than their max health
	if (health > maxHealth)
	{
		health = maxHealth;
	}
}

void Player::Heal()
{
	// Heals this player's health
	health += healAmount;

	Overheal();
}

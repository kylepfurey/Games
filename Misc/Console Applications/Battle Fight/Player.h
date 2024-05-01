#pragma once
#include <string>

// Possible Player Actions
enum Action { None, Attack, Defend, Heal };

// Player Class
class Player
{
private:

	// Private variables and functions
	std::string name;
	Action currentAction;
	int target;

	bool alive;
	float health;
	float maxHealth;

	float attackPower;
	bool isDefending;
	float defendMultiplier;
	float healAmount;

	void Defend();						// Reflects damage back at the opponent

	void DeathCheck();
	void Overheal();

public:

	// Public variables and functions
	Player();							// Constructor
	~Player();							// Deconstructor

	std::string GetName();				// Gets this player's name
	void SetName(std::string newName);	// Sets this player's name
	Action GetAction();					// Gets this player's current action
	void SetAction(Action newAction);	// Set this player's new action
	int GetTarget();					// Get this player's target
	void SetTarget(int targetIndex);	// Sets this player's current target
	float Stats(Action statType);		// Gets this player's stat of the given action

	bool IsAlive();						// Returns whether this player is alive or not
	bool IsDefending();					// Returns whether this player is defending this turn or not
	float Health();						// Gets this player's health
	float MaxHealth();					// Gets this player's maximum health
	void Damage(Player& attacker);		// Damages this player's health
	void Damage(float damage);			// Damages this player's health

	// ATTACK -> HEAL -> BLOCK -> ATTACK

	void Attack(Player& enemy);			// Attacks another player's health
	void Heal();						// Heals this player's health
};

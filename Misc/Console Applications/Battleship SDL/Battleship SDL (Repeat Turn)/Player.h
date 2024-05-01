#pragma once

class Player {
public:
	Player();
	~Player();
	void ShipDamaged(char ship);
	int shipCount;
private:
	int CarrierHealth;
	int BattleshipHealth;
	int CruiserHealth;
	int SubmarineHealth;
	int DestroyerHealth;
};

#pragma once
#include <unordered_map>
extern std::unordered_map<char, unsigned long long> mask1;
extern std::unordered_map<char, std::vector<char>> avaible_moves;
class Position {
public:

	

	
	Position(unsigned long long _position, char _zero, char _last_move, char _count_moves);
	inline char operator[](char d);
	unsigned long long position;
	char zero;
	char last_move;
	char count_moves;
	unsigned long long make_move(char move);
	unsigned long long move_right();
	unsigned long long move_left();
	unsigned long long move_up();
	unsigned long long move_down();
	
};


#include "bit_tag.h"

std::unordered_map<char, unsigned long long> mask1{ {0,17293822569102704640},{1,1080863910568919040},{2,67553994410557440},
								   {3,4222124650659840},{4,263882790666240},{5,16492674416640},
								   {6,1030792151040}, {7,64424509440},{8,4026531840},{9,251658240},
								   {10,15728640},{11,983040},{12,61440},{13,3840},{14,240},{15,15} };

extern std::unordered_map<char, std::vector<char>> avaible_moves{ {0,{1,4}},
																{3,{-1,4}},
																{12,{1,-4}},
																{15,{-1,-4}},
																{1,{-1,1,4}}, {2,{-1,1,4}},
																{4,{1,-4,4}}, {8,{1,-4,4}},
																{5,{-1,1,-4,4}} ,{6,{-1,1,-4,4}},
																{9,{-1,1,-4,4}}, {10,{-1,1,-4,4}},
																{7,{-1,-4,4}}, {11,{-1,-4,4}},
																{13,{-1,1,-4}},{14,{-1,1,-4}}};

Position::Position(unsigned long long _position, char _zero, char _last_move, char _count_moves)
	: position(_position), zero(_zero), last_move(_last_move), count_moves(_count_moves) {}


inline char Position::operator[](char d) {
	return (position & mask1[d]) >> (64 - (d + 1) * 4);
}

unsigned long long Position::make_move(char move) {
	if (move == -1 && zero > 0) {
		return move_left();
	}
	if (move == 1 && zero < 16) {
		return move_right();
	}
	if (move == -4 && zero > 3) {
		return move_up();
	}
	if (move == 4 && zero < 12) {
		return move_down();
	}
	return 0;
}

unsigned long long Position::move_left() {
	return ((position & mask1[zero - 1]) >> 4) | (position & ~mask1[zero - 1]);
}
unsigned long long Position::move_right() {
	return (((position & mask1[zero + 1]) << 4) | (position & ~mask1[zero + 1]));
}
unsigned long long Position::move_down() {
	return ((position & mask1[zero + 4]) << 16) | (position & ~mask1[zero + 4]);
}
unsigned long long  Position::move_up() {
	return ((position & mask1[zero - 4]) >> 16) | (position & ~mask1[zero - 4]);
}



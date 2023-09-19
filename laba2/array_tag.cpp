#include "array_tag.h"
#include <iostream>
APosition::APosition(APosition* _parent, std::array<uint8_t, 16>&& _position, uint8_t _zero, int8_t _last_move)
	: parent(_parent), position(_position), zero(_zero), last_move(_last_move) {

}

inline uint8_t APosition::operator[](uint8_t d) {
	return position[d];
}

unsigned long long APosition::get_position()
{
	unsigned long long exp = 1;
	unsigned long long  result = 0;

	for (int i = 15; i >= 0; i--) {
		result += position[i] * exp;
		exp *= 16;
	}
	return result;
}

 std::array<uint8_t, 16> APosition::make_move(int8_t move) {
	if (move == -1) {
		return move_left();
	}
	if (move == 1) {
		return move_right();
	}
	if (move == -4) {
		return move_up();
	}
	if (move == 4) {
		return move_down();
	}
	return {};
}

 std::array<uint8_t, 16> APosition::move_left() {
	auto res = position;
	std::swap(res[zero], res[zero - 1]);
	return res;
}
 std::array<uint8_t, 16> APosition::move_right() {
	auto res = position;
	std::swap(res[zero], res[zero + 1]);
	return res;
}
 std::array<uint8_t, 16>  APosition::move_up() {
	auto res = position;
	std::swap(res[zero], res[zero - 4]);
	return res;
}
 std::array<uint8_t, 16> APosition::move_down() {
	auto res = position;
	std::swap(res[zero], res[zero + 4]);
	return res;
}

inline bool APosition::operator==(const APosition& p) const
{
	return this->position==p.position;
}

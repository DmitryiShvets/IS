#pragma once
#include <array>

class APosition {
public:
	//APosition(APosition* _parent, std::array<char, 16> _position, char _zero, char _last_move);
	APosition(APosition* _parent, std::array<uint8_t, 16>&& _position, uint8_t _zero, int8_t _last_move);
	inline uint8_t operator[](uint8_t d);
	APosition* parent;
	std::array<uint8_t,16> position;
	unsigned long long get_position();
	uint8_t zero;
	int8_t last_move;
	 std::array<uint8_t, 16> make_move(int8_t move);
	 std::array<uint8_t, 16>  move_right();
	 std::array<uint8_t, 16>  move_left();
	 std::array<uint8_t, 16>  move_up();
	 std::array<uint8_t, 16>  move_down();


	inline bool operator==(const APosition& p) const;
};

template<>
struct std::hash<std::array<uint8_t, 16>> {
	size_t operator() (const std::array<uint8_t, 16>& key) const {
		size_t exp = 1;
		size_t result = 0;

		for (int i = 15; i >= 0; i--) {
			result += key[i] * exp;
			exp *= 16;
		}
		return result;
	}
};
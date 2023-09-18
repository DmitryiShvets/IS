#include "bit_tag.h"
#include <array>
#include <bitset>
#include "staff.h"
#include <iostream>
std::array<unsigned long long, 16> mask_a{
	17293822569102704640,
	1080863910568919040,
	67553994410557440,
	4222124650659840,
	263882790666240,
	16492674416640,
	1030792151040,
	64424509440,
	4026531840,
	251658240,
	15728640,
	983040,
	61440,
	3840,
	240,
	15
};


Position::Position(Position* _parent, unsigned long long _position, uint8_t _zero, int8_t _last_move, int _depth)
	: parent(_parent), position(_position), zero(_zero), last_move(_last_move), depth(_depth) {
}

Position::Position(const std::string& _position)
	:parent(nullptr), last_move(0), depth(0)
{
	position = hex_to_int(_position);
	zero = find_zero();
}


inline uint8_t Position::operator[](uint8_t d) const {
	return uint8_t((position & mask_a[d]) >> (64 - (d + 1) * 4));
}

uint8_t Position::find_zero()
{
	for (uint8_t i = 0; i < 16; i++)
	{
		if (operator[](i) == 0)return i;
	}
	return -1;
}

bool Position::is_valid_position()
{
	int inversions_count = 0;
	for (uint8_t i = 0; i < 16; i++)
	{
		for (uint8_t j = 0; j < i; j++)
		{
			auto a = operator[](j);
			auto b = operator[](i);
			if (a && b && a > b)inversions_count++;
		}

	}

	inversions_count += 1 + zero / 4;

	return inversions_count % 2 == 0;
}

int Position::hdm() const {
	int sum = 0;
	for (int i = 0; i < 16; ++i) {
		int element = int((position & mask_a[i]) >> (64 - (i + 1) * 4));
		if (element == 0)continue;
		int x1 = i % 4;
		int y1 = i / 4;
		int x2 = (element - 1) % 4;
		int y2 = (element - 1) / 4;

		sum += abs(x1 - x2) + abs(y1 - y2);

	}
	return sum;
}

Position::~Position()
{
	
}

int Position::hcp() const
{
	int sum = 0;
	for (int i = 0; i < 16; ++i) {
		int element = int((position & mask_a[i]) >> (64 - (i + 1) * 4));
		if (element && element != i + 1) 	sum++;
	}
	return sum;
}


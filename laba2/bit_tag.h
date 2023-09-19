#pragma once
#include <vector>
#include <string>
extern std::array<unsigned long long, 16> mask_a;

class Position {

public:
	Position(Position* _parent, unsigned long long _position, uint8_t _zero, int8_t _last_move, int _depth);
	Position(const std::string& _position);

	inline uint8_t operator[](uint8_t d) const;
	bool is_valid_position();
	~Position();
	friend bool operator==(const Position& l, const Position& r)
	{
		return l.position == r.position; // keep the same order
	}
	int hdm() const;
	int hcp() const;
	static int heuristic(unsigned long long pos);
	Position* parent;
	unsigned long long position;
	int8_t zero;
	int depth;
	int8_t last_move;
	uint8_t find_zero();
};

struct CasualComparePositions {
	bool operator()(const Position* a, const Position* b) const {
		return a->depth + a->hcp() > b->depth + b->hcp();
	}
};

struct ManhattanComparePositions {
	bool operator()(const Position* a, const Position* b) const {
		return a->depth + a->hdm() > b->depth + b->hdm();
	}
};

template<>
struct std::hash<Position> {
	size_t operator() (const Position& key) const {

		return key.position;
	}
};

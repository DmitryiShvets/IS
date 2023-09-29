#pragma once
#include <bitset>
#include <iostream>
#include <vector>
#include <random>
#include <numeric>
constexpr auto N = 9;

class Position {
public:
	uint8_t count_queens;
	std::bitset<N* N> board;

	Position();
	Position(std::bitset<N* N> _board, uint8_t _count_queens);

	void print_pos()const;
	void print_board() const;

	std::vector<int> to_vector() const;
	std::bitset<N* N> get_new_pos(int x, int y) const;
	void place_queen(int x, int y);
	bool is_safe(int x, int y) const;
	int fitness() const;
};

class PositionN : public Position {
public:
	std::vector<int> pos;

	PositionN();
	PositionN(const std::vector<int>& _pos);

	PositionN mutate(double mutation_rate) const;
	PositionN crossover(const PositionN& other) const;

};

struct CasualComparePositions {
	bool operator()(const PositionN& a, const PositionN& b) const {
		return a.fitness() < b.fitness();
	}
};

struct ComparePositions {
	bool operator()(const Position& a, const Position& b) const {
		return a.fitness() < b.fitness();
	}
};

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

	void print_board() const {

		std::cout << "  ";
		for (int i = 0; i < N; ++i) {
			std::cout << i << " ";
		}
		std::cout << std::endl;

		for (int row = N - 1; row >= 0; --row) {
			std::cout << row << " ";
			for (int col = 0; col < N; ++col) {
				std::cout << (board[row * N + col] ? "Q " : ". ");
			}
			std::cout << row << std::endl;
		}
		std::cout << "  ";

		for (int i = 0; i < N; ++i) {
			std::cout << i << " ";
		}
		std::cout << std::endl;
	}

	std::vector<int> to_vector() const {
		std::vector<int> result;
		for (int i = 0; i < N * N; i++)
		{
			if (board[i])result.push_back(i % N);
		}
		return result;
	}

	void print_pos()const {
		std::cout << "{";
		for (int i = 0; i < N * N; i++)
		{
			if (board[i])std::cout << i % N << ", ";
		}
		std::cout << "}" << std::endl;
	}

	void place_queen(int x, int y) {
		board[y * N + x] = 1;
	}

	Position(std::bitset<N* N> _board, uint8_t _count_queens) : board(_board), count_queens(_count_queens) {}

	Position() : board(0), count_queens(0) {}

	bool isSafe(int x, int y) const {
		// Проверка по горизонтали и вертикали
		for (int i = 0; i < N; ++i) {
			//   по столбцам            по строкам
			if (board[y * N + i] || board[i * N + x]) {
				return false;
			}
		}

		// Проверка по диагонали от точки (x,y) направо вверх
		for (int i = 1; y + i < N && x + i < N; ++i) {
			if (board[(y + i) * N + x + i]) {
				return false;
			}
		}
		// Проверка по диагонали от точки (x,y) направо вниз
		for (int i = 1; y - i >= 0 && x + i < N; ++i) {
			//std::cout << ((y - i) * 8 + x + i) << " ";
			if (board[(y - i) * N + x + i]) {
				return false;
			}
		}
		// Проверка по диагонали от точки (x,y) налево вниз
		for (int i = 1; y - i >= 0 && x - i >= 0; ++i) {
			//std::cout << ((y - i) * 8 + x - i) << " ";
			if (board[(y - i) * N + x - i]) {
				return false;
			}
		}
		// Проверка по диагонали от точки (x,y) налево вверх
		for (int i = 1; y + i < N && x - i >= 0; ++i) {
			if (board[(y + i) * N + x - i]) {
				return false;
			}
		}
		return true; // Позиция безопасна
	}

	std::bitset<N* N> placeQueen1(int x, int y) {
		auto tmp = board;
		tmp[y * N + x] = 1;
		return tmp;
	}

};

class PositionN : public Position {
public:
	std::vector<int> pos;

	PositionN(const std::vector<int>& _pos) :pos(_pos) {
		for (int i = 0; i < N; i++)
		{
			this->place_queen(pos[i], i);

		}
		this->count_queens = N;
	}

	PositionN() {
		std::vector<int> gen(N);
		// Генерируем случайную начальную популяцию
		for (int i = 0; i < N; ++i) {
			gen[i] = i;
		}
		std::random_device rd;
		std::mt19937 g(rd());
		std::shuffle(gen.begin(), gen.end(), g);

		for (int i = 0; i < N; i++)
		{
			this->place_queen(gen[i], i);
		}
		this->count_queens = N;
		this->pos = std::move(gen);
	}

	//PositionN(PositionN&& other) = default;
	//PositionN& operator=(PositionN&&other) = default;
	//PositionN(PositionN&) = delete;
	//PositionN& operator=(PositionN&) = delete;

	int fitness() const {
		int conflicts = 0;

		for (int x = 0; x < N; ++x) {
			for (int y = 0; y < N; ++y) {
				if (board[y * N + x]) {
					for (int i = 0; i < N; ++i) {
						if (i != x && board[y * N + i]) {
							conflicts++;
						}
						if (i != y && board[i * N + x]) {
							conflicts++;
						}
					}
					for (int i = 1; i < N; ++i) {
						if (x + i < N && y + i < N && board[(y + i) * N + (x + i)]) {
							conflicts++;
						}
						if (x - i >= 0 && y - i >= 0 && board[(y - i) * N + (x - i)]) {
							conflicts++;
						}
						if (x + i < N && y - i >= 0 && board[(y - i) * N + (x + i)]) {
							conflicts++;
						}
						if (x - i >= 0 && y + i < N && board[(y + i) * N + (x - i)]) {
							conflicts++;
						}
					}
				}
			}
		}

		return conflicts;
	}

	PositionN mutate(double mutation_rate) {
		auto queens = pos;

		std::random_device rd;
		std::mt19937 gen(rd());
		std::uniform_int_distribution<int> rand_index(0, N-1);
		std::uniform_real_distribution<double> p(0.0, 1.0);

		for (int i = 0; i < N; ++i) {
			if (p(gen) < mutation_rate) {
				int new_pos = rand_index(gen);
				std::swap(queens[i], queens[new_pos]);
			}
		}
		return PositionN{ queens };
	}

	PositionN crossover(const PositionN& other) {
		std::vector<int> childBoard = pos;

		std::vector<int> queens(N);
		std::iota(std::begin(queens), std::end(queens), 0);

		for (int i = 0; i < N; ++i) {
			if (childBoard[i] == other.pos[i]) {
				queens.erase(std::find(std::begin(queens), std::end(queens), i));
			}
			else {
				childBoard[i] = -1;
			}
		}

		std::random_device rd;
		std::mt19937 gen(rd());

		for (int i = 0; i < N; ++i) {
			if (childBoard[i] == -1) {
				std::uniform_int_distribution<int> distribution(0, (int)queens.size() - 1);
				int rand_i = distribution(gen);
				childBoard[i] = queens[rand_i];
				queens.erase(std::find(std::begin(queens), std::end(queens), queens[rand_i]));

			}
		}

		return PositionN(childBoard);
	}

};

struct CasualComparePositions {
	bool operator()(const PositionN& a, const PositionN& b) const {
		return a.fitness() < b.fitness();
	}
};


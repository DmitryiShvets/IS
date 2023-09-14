#include <iostream>
#include <string>
#include <unordered_map>

#include <queue>
#include "bit_tag.h"
#include <unordered_set>
#include <chrono>
void print_bin(unsigned long long a)
{
	for (int i = 1; i < 65; i++)
	{
		//Проверяем старший бит)
		if (a & 0x8000000000000000)
			std::cout << "1";
		else
			std::cout << "0";

		//Сдвигаем влево на 1 бит
		a = a << 1;
		if (i % 4 == 0)std::cout << " ";

	}
	std::cout << "\n";
}
std::unordered_map<char, int>  hex{ {'1',1},{'2',2},{'3',3},{'4',4},
								  {'5',5},{'6',6},{'7',7},{'8',8},
								  {'9',9},{'A',10},{'B',11},{'C',12},
								  {'D',13},{'E',14},{'F',15},{'0',0} };



unsigned long long hex_to_int(const std::string& value) {
	unsigned long long exp = 1;
	unsigned long long result = 0;

	for (auto end = value.rbegin(); end != value.rend(); end++) {
		result += hex[*end] * exp;
		exp *= 16;
	}
	return result;
}
unsigned long long cucle_rshift(unsigned long long value, int k) {
	auto tmp1 = value >> k;
	auto tmp2 = value << (64 - k);

	return tmp1 | tmp2;
}
unsigned long long cucle_lshift(unsigned long long value, int k) {
	auto tmp1 = value << k;
	auto tmp2 = value >> (64 - k);

	return tmp1 | tmp2;
}
void generate_mask(int position) {
	for (size_t i = 0; i < 64; i++)
	{
		if (i >= position * 4 && i <= position * 4 + 3)std::cout << "1";
		else std::cout << "0";
	}
}
void generate_mask() {
	unsigned long long a = 15;
	for (size_t i = 0; i < 16; i++)
	{
		std::cout << a << std::endl;
		a *= 16;
	}
}

int bfs(unsigned long long start, unsigned long long end, char zero_pos) {
	std::queue<Position> q;
	std::unordered_set<unsigned long long> s;


	q.emplace(start, zero_pos, 0, 0);

	while (!q.empty())
	{
		auto cur = q.front();
		q.pop();
		s.insert(cur.position);
		for (auto move : avaible_moves[cur.zero]) {
			if (move == cur.last_move)continue;

			auto new_pos = cur.make_move(move);
			//std::cout <<"текущаа позиция: " <<cur.position<<" индекс нуля: "<< (int)cur.zero 
						//<<" операция: " <<(int)move<< " новая позиция: " << new_pos << std::endl;
			if (new_pos == end) {
				//std::cout << "конец ходов: " << (int)cur.count_moves +1 << std::endl;
				return (int)cur.count_moves + 1;
			}
			if (!s.contains(new_pos)) {
				q.emplace(new_pos, cur.zero + move, -move, cur.count_moves + 1);
				s.insert(new_pos);
			}
		}


	}
	return -1;
}
inline unsigned long long move_right(unsigned long long positon, char k) {
	return ((positon & mask1[k]) >> 4) | (positon & ~mask1[k]);
}

inline unsigned long long move_left(unsigned long long positon, char k) {
	return ((positon & mask1[k]) << 4) | (positon & ~mask1[k]);
}

inline unsigned long long move_up(unsigned long long positon, char k) {
	return ((positon & mask1[k]) << 16) | (positon & ~mask1[k]);
}

inline long long move_down(unsigned long long positon, char k) {
	return ((positon & mask1[k]) >> 16) | (positon & ~mask1[k]);
}


int main() {


	unsigned long long end = hex_to_int("123456789ABCDEF0");
	int count = 0;

	auto start_t = std::chrono::high_resolution_clock::now();

	/*unsigned long long start = hex_to_int("1234067859ACDEBF");
	bfs(start, end, 4);*/

	/*unsigned long long start = hex_to_int("5134207896ACDEBF");
	bfs(start, end, 5);*/

	/*unsigned long long start = hex_to_int("16245A3709C8DEBF");
	bfs(start, end, 8);*/

	/*unsigned long long start = hex_to_int("1723068459ACDEBF");
	bfs(start, end, 4);*/

	/*unsigned long long start = hex_to_int("12345678A0BE9FCD"); 19
	bfs(start, end, 9);*/

	/*unsigned long long start = hex_to_int("01245738A6BE9FCD"); 24
	bfs(start, end, 0);*/

	unsigned long long start = hex_to_int("51240738A6BE9FCD");
	count =bfs(start, end, 4);

	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << duration.count() << " ns\n" << "count: " << count << std::endl;

	return 0;
}
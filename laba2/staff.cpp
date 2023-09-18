#include <unordered_map>
#include <iostream>
#include <fstream>
#include "staff.h"
#include <array>
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

std::unordered_map<int, char>  hex_a{ {1,'1'},{2,'2'},{3,'3'},{4,'4'},
								  {5,'5'},{6,'6'},{7,'7'},{8,'8'},
								  {9,'9'},{10,'A'},{11,'B'},{12,'C'},
								  {13,'D'},{14,'E'},{15,'F'},{0,'0'} };


unsigned long long hex_to_int(const std::string& value) {
	unsigned long long exp = 1;
	unsigned long long result = 0;

	for (auto end = value.rbegin(); end != value.rend(); end++) {
		result += hex[*end] * exp;
		exp *= 16;
	}
	return result;
}

std::array<uint8_t, 16> hex_to_arr(const std::string& value)
{
	std::array<uint8_t, 16> res;

	for (int i = 15; i >= 0; i--)
	{
		res[i] = hex[value[i]];
	}
	return res;
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
	for (int i = 0; i < 64; i++)
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

std::string int_to_hex(unsigned long long pos)
{
	std::string result;
	for (int i = 0; i < 16; ++i) {
		int element = int((pos & mask_a[i]) >> (64 - (i + 1) * 4));
		result += hex_a[element];
	}
	return result;
}

void save_positions(Position* pos, const std::string& file_name)
{
	
	std::ofstream out;        
	out.open(file_name);     
	if (!out.is_open())
	{
		std::cout << "Ошибка открытия файла " << file_name << std::endl;
	}

	auto tmp = pos;
	while (tmp) {
		out << "Позиция: " << int_to_hex(tmp->position) << " Ход " << (int)tmp->last_move << std::endl;
		tmp = tmp->parent;
	}
	out.close();
}

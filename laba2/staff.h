#pragma once
#include <string>
#include "bit_tag.h"
unsigned long long hex_to_int(const std::string& value);
std::array<uint8_t, 16> hex_to_arr(const std::string& value);
void print_bin(unsigned long long a);
unsigned long long cucle_rshift(unsigned long long value, int k);
unsigned long long cucle_lshift(unsigned long long value, int k);
void generate_mask(int position);
void generate_mask();
std::string int_to_hex(unsigned long long pos);
void save_positions(Position* pos, const std::string& file_name);


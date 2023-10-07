#pragma once
#include <unordered_map>
#include <string>

class Database
{
public:
	std::unordered_map<std::string, int> opening_book = {};
	void Init();

private:
	void load_openings();

};


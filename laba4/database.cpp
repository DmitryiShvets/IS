#include "database.h"
#include <fstream>
#include <iostream>

void Database::Init()
{
	load_openings();
}

void Database::load_openings()
{
	std::ifstream csv("openings.txt");
	std::string pastMoves;
	std::string nextMove;
	int nextMoveInt;

	while (std::getline(csv, pastMoves)) {
		std::getline(csv, nextMove);
		nextMoveInt = std::stoi(nextMove);

		//std::cout <<"key: " << pastMoves ;
		//std::cout << " value: " << nextMoveInt << std::endl;
		opening_book.insert({ pastMoves, nextMoveInt });
	}

}

#pragma once
#include "board.h"
#include "game.h"

#define Hashsize 1024 //(2GB)67108864 (4GB)134217728 (8GB)268435456

class Location {
public:
	int i;
	int j;
	int g;
};


class AIPlayer
{
public:
	void Computer_Think(int* x, int* y);
	void Init(Game* game,Board * board);
private:
	int Dynamicdeep = 10;                               //динамическая глубина
	int DynamicdeepS = 0;                               //Увеличьте размер стартового лота
	int DynamicdeepE = 0;                               //Увеличьте размер закрывающего лота
	int deepStart = 4;                                  //начальная глубина
	int deepEnd = 8;                                    //конечная глубина
	int endTime = 46;                                   //Окончательный поиск Pefect End
	int fRandomMove = true;                             //начальный случайный ход
	int RandomMove = 2;                                 //Начальный случайный размер лота
	int Search_Counter;
	int resultX, resultY;
	double historyAttenua = 0.2;                        //History attenuation coefficient
	int search_deep = 4;
	int fiterative = true;                              //Итеративное углубление Iterative Deepening
	int fMTD = true;                                    //тест памяти MTD Memory-enhanced Test Driver
	int alpha_beta_option = true;                       //Alpha-Beta отсечение
	Location history[2][Board_Size * Board_Size][Board_Size * Board_Size]; //таблица истории Move Ordering
	long long int zobrist[2][Board_Size][Board_Size];   //Zobrist Hash
	int zhash = true;                                   //Таблица замены transposition table
	long long int Hashnode = 0;
	long long int Searchnode = 0;

	Board* m_board;
	Game* m_game;
	//MinMax
	int  Search(int myturn, int mylevel);
	//Рекурсивный поиск в глубину
	int  search_next(int x, int y, int myturn, int mylevel, int alpha, int beta);

	//Рассчитать zobristHash
	long long int getHash(void);
	//Генератор случайных чисел U64
	long long int rand64(void);
};


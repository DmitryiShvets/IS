#pragma once
#include "board.h"
#include "game.h"
#include "database.h"
#define Hashsize 9710886 //(2GB)67108864 (4GB)134217728 (8GB)268435456

class Location {
public:
	int i;
	int j;
	int g;
};


class AIPlayer
{
public:
	void computer_think(int* x, int* y);
	void Init(Game* game, Board* board, Database* db);

private:
	int dyn_deep = 10;                               //динамическая глубина
	int dyn_deep_start = 0;                               //Увеличьте размер стартового лота
	int dyn_deep_end = 0;                               //Увеличьте размер закрывающего лота
	int deep_start = 4;                                  //начальная глубина
	int deep_end = 8;                                    //конечная глубина
	int search_deep = 4;

	int end_time = 46;                                   //Окончательный поиск Pefect End
	bool opening = true;

	int resultX, resultY;

	double history_attenua = 0.2;                        //History attenuation coefficient
	int f_iterative = true;                              //Итеративное углубление Iterative Deepening
	int f_MTD = true;                                    //тест памяти MTD Memory-enhanced Test Driver
	int f_alpha_beta_option = true;                       //Alpha-Beta отсечение

	Location history[2][Board_Size * Board_Size][Board_Size * Board_Size]; //таблица истории Move Ordering
	long long int zobrist[2][Board_Size][Board_Size];   //Zobrist Hash
	int zhash = true;                                   //Таблица замены transposition table

	long long int hash_node = 0;
	long long int search_node = 0;

	Board* m_board;
	Game* m_game;
	Database* m_db;
	//MinMax
	int  min_max(int myturn, int mylevel);
	//Рекурсивный поиск в глубину
	int  search_next(int x, int y, int myturn, int mylevel, int alpha, int beta);

	//Рассчитать zobristHash
	long long int get_hash(void);
	//Генератор случайных чисел U64
	long long int rand64(void);
};


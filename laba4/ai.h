#pragma once
#include "board.h"
#include "game.h"
#include "database.h"
#define Hashsize 9710886 //(2GB)67108864 (4GB)134217728 (8GB)268435456
#include <chrono>

class Position {
public:
	int x;
	int y;
	int g;
};


class AIPlayer
{
public:
	void computer_think(int* x, int* y);
	void Init(Game* game, Board* board, Database* db);
	void set_min_base_deep(int value);
	double max_search_time = 0.0;

private:
	int min_base_deep = 10;									//динамическая глубина
	int deep_start = 8;										//начальная глубина
	int deep_end = 8;										//конечная глубина
	int search_deep;

	bool ending = false;

	int open_moves = 0;
	int open_deep = 0;
	int resultX, resultY;

	double history_attenua = 0.2;                        //History attenuation coefficient
	int f_MTD = true;                                    //Memory-enhanced Test Driver
	int f_alpha_beta_option = true;                      //Alpha-Beta отсечение

	Position history[2][Board_Size * Board_Size][Board_Size * Board_Size]; //таблица истории Move Ordering
	long long int zobrist[2][Board_Size][Board_Size];   //Zobrist Hash

	Board* m_board;
	Game* m_game;
	Database* m_db;

	int  min_max(int myturn, int mylevel);
	//Рекурсивный поиск в глубину
	int  search_next(int x, int y, int myturn, int mylevel, int alpha, int beta);

	//Рассчитать zobristHash
	long long int get_position_hash(void);
	//Генератор случайных чисел U64
	long long int rand64(void);
};


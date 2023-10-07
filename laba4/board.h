#pragma once

constexpr int Board_Size = 8;
#define TRUE 1
#define FALSE 0


class Board
{
public:

	int legal_moves[Board_Size][Board_Size]; //матрица возможных ходов
	int legal_move_index[50][3];
	int now_board[Board_Size][Board_Size];   //текущая позиция
	int tmp_board[Board_Size][Board_Size];   //предыдущая позиция для отмены хода
	int b_count, w_count;					//колличество черных и белых камней
	int stone_color[2] = { 1,2 };			// 1: black, 2: white


	void Init();
	//Обновляет возможные ходы и возвращает количество возможных ходов.
	int  find_legal_moves(int color);
	//Определитет, валидный ли ход.
	int  is_valid_move(int x, int y);
	//Определитет, можно ли захватить вражеские камни
	int  сheck_сross(int x, int y, bool update);
	//Поместить камень
	int  add_stone(int x, int y, int turn);

private:

	int dir_x[8] = { 0, 1, 1, 1, 0, -1, -1, -1 };
	int dir_y[8] = { -1, -1, 0, 1, 1, 1, 0, -1 };
	
	//Определяет может ли сосед d захватать вражескую фигуру, если update=true то захват
	int  flip_enemy(int x, int y, int d, bool update);
};


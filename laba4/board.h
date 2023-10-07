#pragma once

constexpr int Board_Size = 8;
#define TRUE 1
#define FALSE 0


class Board
{
public:

	int Legal_Moves[Board_Size][Board_Size]; //матрица возможных ходов
	int Legal_Move_Index[50][3];
	int Now_Board[Board_Size][Board_Size];   //текущая позиция
	int B, W; //колличество черных и белых камней
	int Stones_color[2] = { 1,2 }; // 1: black, 2: white

	void Init();
	//Обновить возможные ходы и вернуть количество возможных ходов.
	int  Find_Legal_Moves(int color);
	//Определите, валидный ли ход.
	int  In_Board(int x, int y);
	//Определите, можно ли захватить шахматную фигуру
	int  Check_Cross(int x, int y, int update);
	//Поместить камень
	int  Put_a_Stone(int x, int y, int turn);

private:

	int DirX[8] = { 0, 1, 1, 1, 0, -1, -1, -1 };
	int DirY[8] = { -1, -1, 0, 1, 1, 1, 0, -1 };
	
	//Определите, может ли направление d захватить шахматную фигуру，update Чтобы выполнить сигнал захвата
	int  Check_Straight_Army(int x, int y, int d, bool update);
};


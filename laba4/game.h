#pragma once
#include "board.h"


class Game {
public:

	void Init(Board* board);
	//Сделать ход
	int  Play_a_Move(int x, int y);
	//Определите, является ли оно окончательным
	int  Check_EndGame();
	//Показать доску и возможные ходы
	void Show_Board_and_Set_Legal_Moves();
	//Коэффициент обновления
	int  Compute_Grades(int flag, int isL);
	
	int HandNumber;
	int Turn = 0;

	void set_comp_take(int take);
	int get_exit_code();
	int Computer_Take;

private:

	Board* m_board;

	int Winner;
	int Black_Count;
	int	White_Count;
	int LastX;
	int LastY;
	int fS = 4;                                         //Стабильный вес StableDiscs Weight
	int endTime = 46;                                   //Окончательный поиск Pefect End
	long long int Grades = 0;

	//int sequence[Board_Size * Board_Size];

	//Рассчитать стабилизатор
	int StableDiscs(int me);
};
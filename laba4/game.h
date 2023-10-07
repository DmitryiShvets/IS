#pragma once
#include "board.h"
#include <string>

class Game {
public:

	void Init(Board* board);
	//Сделать ход
	int  play_move(int x, int y);
	//Отменить ход
	void  undo_move();
	//Проверка что у противника нет ходов
	bool  player_have_moves();
	//Определите, является ли оно окончательным
	int  is_game_end();
	//Показать доску и возможные ходы
	void print_board_and_set_legal_moves();
	//Вычислить оценку позиции
	int  compute_grades(int flag, int isL);
	//Задать очередь ходов для компьютера
	void set_comp_take(int take);
	//Определить результат игры
	int get_exit_code();
	
	std::string move_history = "";
	int cur_move_number;
	int now_turn = 0;
	int computer_take;

private:

	Board* m_board;

	int winner;
	int black_count;
	int	white_count;
	int last_x;
	int last_y;
	int fS = 4;                                         //Стабильный вес stable discs weight
	int end_time = 46;                                   //Окончательный поиск Pefect End
	long long int grades = 0;

	//Рассчитать стабилизатор
	int stable_discs(int me);
};
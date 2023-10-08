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
	//Проверка что у игрока нет ходов
	bool  player_have_moves();
	//Проверка, что игра окончена
	int  is_game_end();
	//Показать доску и возможные ходы
	void print_board_and_set_legal_moves();
	//Вычислить оценку позиции
	int  compute_grades(int flag, int isL);
	//Задать очередь ходов для компьютера
	void set_comp_take(int take);
	//Задать очередь ходов для компьютера
	//Определить результат игры
	int get_exit_code();
	
	std::string move_history = "";
	int cur_move_number;
	int now_turn = 0;
	int computer_take;
	int end_time = 46;                                   //Окончательный поиск Pefect End

private:

	Board* m_board;

	int winner;
	int black_count;									//коллисетво камней на доске
	int	white_count;
	int last_x;											//последний соверхшенный ход
	int last_y;
	int fS = 4;                                          //Стабильный вес stable discs weight
	long long int grades = 0;

	//Рассчитать колличество стабильных камней
	int stable_discs(int me);
};
#include "game.h"
#include <stdio.h>
#include <memory.h>

void Game::Init(Board* board)
{
	m_board = board;

	cur_move_number = 0;
	now_turn = 0;
	last_x = last_y = -1;
	black_count = white_count = 0;
	winner = 0;

}

int Game::play_move(int x, int y)
{

	if (x == -1 && y == -1) {
		//pass
		cur_move_number++;
		now_turn = 1 - now_turn;
		return 1;
	}

	if (!m_board->is_valid_move(x, y)) return 0;

	m_board->find_legal_moves(now_turn);
	if (m_board->legal_moves[x][y] == FALSE) return 0;

	if (m_board->add_stone(x, y, now_turn)) {

		cur_move_number++;
		last_x = x;
		last_y = y;

		now_turn = 1 - now_turn;
		m_board->сheck_сross(x, y, 1);

		int move_index = last_y * 8 + last_x;
		move_history.append(std::to_string(move_index) + ",");

		compute_grades(TRUE, 0);
		return 1;
	}
	else
		return 0;
	return 1;
}

void Game::undo_move()
{
	//now_turn = 1 - now_turn;
	std::memcpy(m_board->now_board, m_board->tmp_board, sizeof(int) * Board_Size * Board_Size);
}

bool Game::player_have_moves()
{
	int count_move_enemy = m_board->find_legal_moves(1-computer_take);
	if (count_move_enemy == 0) {
		return false;
	}
	else return true;
}

int Game::is_game_end()
{
	int lc1, lc2;

	lc2 = m_board->find_legal_moves(1 - now_turn);
	lc1 = m_board->find_legal_moves(now_turn);

	if (lc1 == 0 && lc2 == 0) {
		if (black_count > white_count) {
			printf("Black Win!\n");
			if (winner == 0)
				winner = 1;
		}
		else if (black_count < white_count) {
			printf("White Win!\n");
			if (winner == 0)
				winner = 2;
		}
		else {
			printf("Draw\n");
			winner = 0;
		}
		print_board_and_set_legal_moves();
		printf("Game is over\n");

		return TRUE;
	}

	return FALSE;
}

void Game::print_board_and_set_legal_moves()
{
	int i, j;

	m_board->find_legal_moves(now_turn);

	printf("a b c d e f g h\n");
	for (i = 0; i < Board_Size; i++) {
		for (j = 0; j < Board_Size; j++) {
			if (m_board->now_board[j][i] > 0) {
				if (m_board->now_board[j][i] == 2)
					printf("W ");//white
				else
					printf("B ");//black
			}

			if (m_board->now_board[j][i] == 0) {
				if (m_board->legal_moves[j][i] == 1)
					printf("? ");
				else printf(". ");
			}
		}
		printf(" %d\n", i + 1);
	}
	printf("\n");
}

int Game::compute_grades(int flag, int isL)
{
	if (flag) {
		m_board->find_legal_moves(0);
		black_count = m_board->b_count;
		white_count = m_board->w_count;
		printf("Grade: Black %d, White %d\n", black_count, white_count);
		return 0;
	}
	grades++;
	//миттельшпиль
	if (cur_move_number < end_time) {
		int SB, SW, BM;
		SB = stable_discs(1);
		SW = stable_discs(2);

		if (isL == 0) {
			BM = m_board->find_legal_moves(0);
		}
		else {
			BM = m_board->legal_move_index[0][0];
		}
		return (fS * (SB - SW) + BM);
	}
	//эндшпиль
	else {
		if (isL == 0) {
			m_board->b_count = 0;
			for (int i = 0; i < Board_Size; i++)
				for (int j = 0; j < Board_Size; j++) {
					if (m_board->now_board[i][j] == 1) {
						m_board->b_count++;
					}
				}
		}
		return (m_board->b_count);
	}
}

void Game::set_comp_take(int take)
{
	computer_take = take;
}


int Game::get_exit_code()
{
	if (computer_take == 0 && winner == 1)return 0;
	else if (computer_take == 1 && winner == 1)return 3;
	else if (computer_take == 0 && winner == 2)return 3;
	else if (computer_take == 1 && winner == 2)return 0;
	else return 4;
}

int Game::stable_discs(int me)
{
	int i, j, k, num = 0;
	int a = 0, b = 0, c = 0, d = 0;
	//в левом верхнем углу
	i = 0;
	j = 0;
	if (m_board->now_board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i + k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			a++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i][j + k] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			d++;
	}
	//верхний правый угол
	i = Board_Size - 1;
	j = 0;
	if (m_board->now_board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i - k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			a++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i][j + k] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			b++;
	}
	//нижний левый угол
	i = 0;
	j = Board_Size - 1;
	if (m_board->now_board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i + k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			c++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i][j - k] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			d++;
	}
	//нижний правый угол
	i = Board_Size - 1;
	j = Board_Size - 1;
	if (m_board->now_board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i - k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			c++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->now_board[i][j - k] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			b++;
	}
	if (a > 1)
		num -= Board_Size;
	if (b > 1)
		num -= Board_Size;
	if (c > 1)
		num -= Board_Size;
	if (d > 1)
		num -= Board_Size;
	return num;
}





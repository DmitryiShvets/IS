#include "Board.h"
#include <memory.h>
#include <stdlib.h>
#include <time.h>

void Board::Init() {
	srand((unsigned)time(NULL));
	memset(now_board, 0, sizeof(int) * Board_Size * Board_Size);

	now_board[3][3] = now_board[4][4] = 2; //белые
	now_board[3][4] = now_board[4][3] = 1; //черные
	memcpy(tmp_board, now_board, sizeof(int) * Board_Size * Board_Size);

}

int Board::find_legal_moves(int turn)
{
	int i, j;
	int me = stone_color[turn];
	int legal_count = 0;
	legal_move_index[0][0] = 0;
	b_count = w_count = 0;

	for (i = 0; i < Board_Size; i++)
		for (j = 0; j < Board_Size; j++)
			legal_moves[i][j] = 0;

	for (i = 0; i < Board_Size; i++)
		for (j = 0; j < Board_Size; j++) {
			if (now_board[i][j] == 0) {
				//Ускорение обрезки
				if (i > 0 && i < Board_Size - 1 && j>0 && j < Board_Size - 1) {
					if ((now_board[i - 1][j - 1] == 0 || now_board[i - 1][j - 1] == me) &&
						(now_board[i - 1][j] == 0 || now_board[i - 1][j] == me) &&
						(now_board[i - 1][j + 1] == 0 || now_board[i - 1][j + 1] == me) &&
						(now_board[i][j - 1] == 0 || now_board[i][j - 1] == me) &&
						(now_board[i][j + 1] == 0 || now_board[i][j + 1] == me) &&
						(now_board[i + 1][j - 1] == 0 || now_board[i + 1][j - 1] == me) &&
						(now_board[i + 1][j] == 0 || now_board[i + 1][j] == me) &&
						(now_board[i + 1][j + 1] == 0 || now_board[i + 1][j + 1] == me)) {
						continue;
					}
				}
				now_board[i][j] = me;
				if (сheck_сross(i, j, false) == TRUE) {
					legal_moves[i][j] = TRUE;
					legal_count++;
					legal_move_index[0][0] = legal_count;
					legal_move_index[legal_count][1] = i;
					legal_move_index[legal_count][2] = j;
				}
				now_board[i][j] = 0;
			}
			else if (now_board[i][j] == 1) {
				b_count++;
			}
			else if (now_board[i][j] == 2) {
				w_count++;
			}
		}

	return legal_count;
}

int Board::сheck_сross(int x, int y, bool update)
{
	int  dx, dy;

	if (!is_valid_move(x, y) || now_board[x][y] == 0)
		return FALSE;

	int enemy = 3 - now_board[x][y];
	int flip_count = 0;

	for (int k = 0; k < 8; k++) {
		dx = x + dir_x[k];
		dy = y + dir_y[k];
		if (is_valid_move(dx, dy) && now_board[dx][dy] == enemy) {
			flip_count += flip_enemy(x, y, k, update);
		}
	}

	if (flip_count > 0)	return TRUE;
	else return FALSE;
}

int Board::add_stone(int x, int y, int turn)
{
	if (now_board[x][y] == 0) {

		now_board[x][y] = stone_color[turn];
		return TRUE;
	}
	return FALSE;
}

int Board::is_valid_move(int x, int y)
{
	if (x >= 0 && x < Board_Size && y >= 0 && y < Board_Size)
		return TRUE;
	else
		return FALSE;
}

int Board::flip_enemy(int x, int y, int d, bool update)
{
	int me = now_board[x][y];
	int enemy = 3 - me;
	int flip_count = 0;
	bool found_flag = false;
	int flag[Board_Size][Board_Size] = { {0} };

	int tx = x;
	int ty = y;

	for (int i = 0; i < Board_Size; i++) {
		tx += dir_x[d];
		ty += dir_y[d];

		if (is_valid_move(tx, ty)) {
			if (now_board[tx][ty] == enemy) {
				flip_count++;
				flag[tx][ty] = TRUE;
			}
			else if (now_board[tx][ty] == me) {
				found_flag = true;
				break;
			}
			else break;
		}
		else break;
	}

	if (found_flag && (flip_count > 0) && update) {
		for (int i = 0; i < Board_Size; i++)
			for (int j = 0; j < Board_Size; j++)
				if (flag[i][j] == TRUE) {
					if (now_board[i][j] != 0)
						now_board[i][j] = 3 - now_board[i][j];
				}
	}
	if (found_flag && (flip_count > 0))
		return flip_count;
	else return 0;
}



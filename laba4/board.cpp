#include "Board.h"
#include <memory.h>
#include <stdlib.h>
#include <time.h>

void Board::Init() {
	srand((unsigned)time(NULL));
	memset(Now_Board, 0, sizeof(int) * Board_Size * Board_Size);

	Now_Board[3][3] = Now_Board[4][4] = 2; //белые
	Now_Board[3][4] = Now_Board[4][3] = 1; //черные

}

int Board::Find_Legal_Moves(int turn)
{
	int i, j;
	int me = Stones_color[turn];
	int legal_count = 0;
	Legal_Move_Index[0][0] = 0;
	B = W = 0;

	for (i = 0; i < Board_Size; i++)
		for (j = 0; j < Board_Size; j++)
			Legal_Moves[i][j] = 0;

	for (i = 0; i < Board_Size; i++)
		for (j = 0; j < Board_Size; j++) {
			if (Now_Board[i][j] == 0) {
				//Ускорение обрезки
				if (i > 0 && i < Board_Size - 1 && j>0 && j < Board_Size - 1) {
					if ((Now_Board[i - 1][j - 1] == 0 || Now_Board[i - 1][j - 1] == me) &&
						(Now_Board[i - 1][j] == 0 || Now_Board[i - 1][j] == me) &&
						(Now_Board[i - 1][j + 1] == 0 || Now_Board[i - 1][j + 1] == me) &&
						(Now_Board[i][j - 1] == 0 || Now_Board[i][j - 1] == me) &&
						(Now_Board[i][j + 1] == 0 || Now_Board[i][j + 1] == me) &&
						(Now_Board[i + 1][j - 1] == 0 || Now_Board[i + 1][j - 1] == me) &&
						(Now_Board[i + 1][j] == 0 || Now_Board[i + 1][j] == me) &&
						(Now_Board[i + 1][j + 1] == 0 || Now_Board[i + 1][j + 1] == me)) {
						continue;
					}
				}
				Now_Board[i][j] = me;
				if (Check_Cross(i, j, FALSE) == TRUE) {
					Legal_Moves[i][j] = TRUE;
					legal_count++;
					Legal_Move_Index[0][0] = legal_count;
					Legal_Move_Index[legal_count][1] = i;
					Legal_Move_Index[legal_count][2] = j;
				}
				Now_Board[i][j] = 0;
			}
			else if (Now_Board[i][j] == 1) {
				B++;
			}
			else if (Now_Board[i][j] == 2) {
				W++;
			}
		}

	return legal_count;
}

int Board::Check_Cross(int x, int y, int update)
{
	int k, dx, dy;

	if (!In_Board(x, y) || Now_Board[x][y] == 0)
		return FALSE;

	int army = 3 - Now_Board[x][y];
	int army_count = 0;

	for (k = 0; k < 8; k++) {
		dx = x + DirX[k];
		dy = y + DirY[k];
		if (In_Board(dx, dy) && Now_Board[dx][dy] == army) {
			army_count += Check_Straight_Army(x, y, k, update);
		}
	}

	if (army_count > 0)
		return TRUE;
	else
		return FALSE;
}

int Board::Put_a_Stone(int x, int y, int turn)
{
	if (Now_Board[x][y] == 0) {

		Now_Board[x][y] = Stones_color[turn];
		return TRUE;
	}
	return FALSE;
}

int Board::In_Board(int x, int y)
{
	if (x >= 0 && x < Board_Size && y >= 0 && y < Board_Size)
		return TRUE;
	else
		return FALSE;
}

int Board::Check_Straight_Army(int x, int y, int d, bool update)
{
	int me = Now_Board[x][y];
	int army = 3 - me;
	int army_count = 0;
	int found_flag = FALSE;
	int flag[Board_Size][Board_Size] = { {0} };

	int tx = x;
	int ty = y;

	for (int i = 0; i < Board_Size; i++) {
		tx += DirX[d];
		ty += DirY[d];

		if (In_Board(tx, ty)) {
			if (Now_Board[tx][ty] == army) {
				army_count++;
				flag[tx][ty] = TRUE;
			}
			else if (Now_Board[tx][ty] == me) {
				found_flag = TRUE;
				break;
			}
			else
				break;
		}
		else
			break;
	}

	if ((found_flag == TRUE) && (army_count > 0) && update) {
		for (int i = 0; i < Board_Size; i++)
			for (int j = 0; j < Board_Size; j++)
				if (flag[i][j] == TRUE) {
					if (Now_Board[i][j] != 0)
						Now_Board[i][j] = 3 - Now_Board[i][j];
				}
	}
	if ((found_flag == TRUE) && (army_count > 0))
		return army_count;
	else return 0;
}



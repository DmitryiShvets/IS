#include "game.h"
#include <stdio.h>
#include <memory.h>

void Game::Init(Board* board)
{
	m_board = board;

	HandNumber = 0;
	Turn = 0;
	LastX = LastY = -1;
	Black_Count = White_Count = 0;
	Winner = 0;

}

int Game::Play_a_Move(int x, int y)
{

	if (x == -1 && y == -1) {
		//pass
		HandNumber++;
		Turn = 1 - Turn;
		return 1;
	}

	if (!m_board->In_Board(x, y))
		return 0;

	m_board->Find_Legal_Moves(Turn);
	if (m_board->Legal_Moves[x][y] == FALSE) return 0;

	if (m_board->Put_a_Stone(x, y, Turn)) {

		HandNumber++;
		LastX = x;
		LastY = y;

		Turn = 1 - Turn;
		m_board->Check_Cross(x, y, 1);

		Compute_Grades(TRUE, 0);
		return 1;
	}
	else
		return 0;
	return 1;
}

int Game::Check_EndGame()
{
	int lc1, lc2;

	lc2 = m_board->Find_Legal_Moves(1 - Turn);
	lc1 = m_board->Find_Legal_Moves(Turn);

	if (lc1 == 0 && lc2 == 0) {
		if (Black_Count > White_Count) {
			printf("Black Win!\n");
			if (Winner == 0)
				Winner = 1;
		}
		else if (Black_Count < White_Count) {
			printf("White Win!\n");
			if (Winner == 0)
				Winner = 2;
		}
		else {
			printf("Draw\n");
			Winner = 0;
		}
		Show_Board_and_Set_Legal_Moves();
		printf("Game is over\n");

		return TRUE;
	}

	return FALSE;
}

void Game::Show_Board_and_Set_Legal_Moves()
{
	int i, j;

	m_board->Find_Legal_Moves(Turn);

	printf("a b c d e f g h\n");
	for (i = 0; i < Board_Size; i++) {
		for (j = 0; j < Board_Size; j++) {
			if (m_board->Now_Board[j][i] > 0) {
				if (m_board->Now_Board[j][i] == 2)
					printf("O ");//white
				else
					printf("X ");//black
			}

			if (m_board->Now_Board[j][i] == 0) {
				if (m_board->Legal_Moves[j][i] == 1)
					printf("? ");
				else printf(". ");
			}
		}
		printf(" %d\n", i + 1);
	}
	printf("\n");
}

int Game::Compute_Grades(int flag, int isL)
{
	if (flag) {
		m_board->Find_Legal_Moves(0);
		Black_Count = m_board->B;
		White_Count = m_board->W;
		printf("Grade: Black %d, White %d\n", m_board->B, m_board->W);
		return 0;
	}
	Grades++;
	//миттельшпиль
	if (HandNumber < endTime) {
		int SB, SW, BM;
		SB = StableDiscs(1);
		SW = StableDiscs(2);

		if (isL == 0) {
			BM = m_board->Find_Legal_Moves(0);
		}
		else {
			BM = m_board->Legal_Move_Index[0][0];
		}
		return (fS * (SB - SW) + BM);
	}
	//эндшпиль
	else {
		if (isL == 0) {
			m_board->B = 0;
			for (int i = 0; i < Board_Size; i++)
				for (int j = 0; j < Board_Size; j++) {
					if (m_board->Now_Board[i][j] == 1) {
						m_board->B++;
					}
				}
		}
		return (m_board->B);
	}
}


void Game::set_comp_take(int take)
{
	Computer_Take = take;
}

int Game::get_exit_code()
{
	if (Computer_Take == 0 && Winner == 1)return 0;
	else if (Computer_Take == 1 && Winner == 1)return 3;
	else if (Computer_Take == 0 && Winner == 2)return 3;
	else if (Computer_Take == 1 && Winner == 2)return 0;
	else return 4;
}

int Game::StableDiscs(int me)
{
	int i, j, k, num = 0;
	int a = 0, b = 0, c = 0, d = 0;
	//в левом верхнем углу
	i = 0;
	j = 0;
	if (m_board->Now_Board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i + k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			a++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i][j + k] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			d++;
	}
	//верхний правый угол
	i = Board_Size - 1;
	j = 0;
	if (m_board->Now_Board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i - k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			a++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i][j + k] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			b++;
	}
	//нижний левый угол
	i = 0;
	j = Board_Size - 1;
	if (m_board->Now_Board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i + k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			c++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i][j - k] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			d++;
	}
	//нижний правый угол
	i = Board_Size - 1;
	j = Board_Size - 1;
	if (m_board->Now_Board[i][j] == me) {
		num++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i - k][j] != me)
				break;
			num++;
		}
		if (k == Board_Size)
			c++;
		for (k = 1; k < Board_Size; k++) {
			if (m_board->Now_Board[i][j - k] != me)
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





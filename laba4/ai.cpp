#include "ai.h"
#include <stdlib.h>
#include <stdio.h>
#include <time.h>
#include <memory.h>


void AIPlayer::Computer_Think(int* x, int* y)
{
	if (fRandomMove && m_game->HandNumber < RandomMove) { //начальный случайный ход
		int lm = m_board->Find_Legal_Moves(m_game->Turn);
		int RMV = (rand() % lm) + 1;
		*x = m_board->Legal_Move_Index[RMV][1];
		*y = m_board->Legal_Move_Index[RMV][2];
		return;
	}
	else {
		time_t clockBegin, clockEnd;
		static int think_time = 0;
		int flag = 0;

		//разрушение таблицы истории
		if (historyAttenua != 0) {
			for (int i = 0; i < Board_Size * Board_Size; i++) {
				for (int j = 0; j < Board_Size * Board_Size; j++) {
					history[0][i][j].g *= historyAttenua;
					history[1][i][j].g *= historyAttenua;
				}
			}
		}

		//динамическая глубина
		if (DynamicdeepS <= m_game->HandNumber && m_game->HandNumber <= DynamicdeepE) {
			deepEnd = Dynamicdeep + 2;
		}
		//Окончательный поиск
		else if (m_game->HandNumber >= endTime) {
			deepEnd = (Board_Size * Board_Size) - m_game->HandNumber;
		}
		else {
			deepEnd = Dynamicdeep;
		}

		clockBegin = clock();

		resultX = resultY = -1;
		Search_Counter = 0;

		flag = Search(m_game->Turn, 0);

		clockEnd = clock();
		think_time += (clockEnd - clockBegin) / 1000;
		printf("used thinking time= %d min. %d.%d sec.\n", think_time / 60000, (think_time % 60000) / 1000, (think_time % 60000) % 1000);

		if (flag) {
			*x = resultX;
			*y = resultY;
		}
		else {
			*x = *y = -1;
			return;
		}
	}

}

void AIPlayer::Init(Game* game, Board* board)
{
	m_board = board;
	m_game = game;
	Search_Counter = 0;

	//Инициализировать таблицу истории
	Location tempL;
	for (int i = 0, m = 0; i < Board_Size; i++) {
		for (int j = 0; j < Board_Size; j++, m++) {
			tempL.i = i;
			tempL.j = j;
			tempL.g = -1;
			for (int k = 0; k < Board_Size * Board_Size; k++) {
				history[0][k][m] = tempL;
				history[1][k][m] = tempL;
			}
		}
	}
	//Инициализировать таблицу замены
	for (int i = 0; i < Board_Size; i++) {
		for (int j = 0; j < Board_Size; j++) {
			zobrist[0][i][j] = rand64();
			zobrist[1][i][j] = rand64();
		}
	}
}

int AIPlayer::Search(int myturn, int mylevel)
{
	if (m_board->Find_Legal_Moves(myturn) <= 0)
		return FALSE;

	int B[Board_Size][Board_Size];
	int L[Board_Size][Board_Size];
	memcpy(B, m_board->Now_Board, sizeof(int) * Board_Size * Board_Size);
	memcpy(L, m_board->Legal_Moves, sizeof(int) * Board_Size * Board_Size);

	Location min, max;
	min.i = min.j = -1; min.g = INT_MAX;
	max.i = max.j = -1; max.g = INT_MIN;
	int i, j, k, test, g;
	int alpha = INT_MIN, beta = INT_MAX;

	for (k = 0; k < Board_Size * Board_Size; k++) {
		i = history[myturn][mylevel][k].i;
		j = history[myturn][mylevel][k].j;
		if (L[i][j] == TRUE) {
			memcpy(m_board->Now_Board, B, sizeof(int) * Board_Size * Board_Size);
			m_board->Now_Board[i][j] = m_board->Stones_color[myturn];
			m_board->Check_Cross(i, j, TRUE);

			//Итеративное углубление
			for (search_deep = deepStart, g = 0; search_deep <= deepEnd; search_deep += 2) {
				if (fiterative == FALSE)
					search_deep = deepEnd;
				//MTD
				if (fMTD) {
					beta = INT_MAX;
					alpha = INT_MIN;
					test = g;
					do {
						g = search_next(i, j, 1 - myturn, mylevel + 1, test - 1, test);
						if (g < test)
							test = beta = g;
						else {
							alpha = g;
							test = g + 1;
						}
					} while (alpha < beta);
				}
				else
					g = search_next(i, j, 1 - myturn, mylevel + 1, alpha, beta);
			}

			if (myturn == 0) {      // max level
				if (g > max.g) {
					max.g = g;
					max.i = i;
					max.j = j;
				}
			}
			else {
				if (g < min.g) {    // min level
					min.g = g;
					min.i = i;
					min.j = j;
				}
			}
			// cutoff
			if (alpha_beta_option)
				if (alpha >= beta) {
					i = Board_Size;
					j = Board_Size;
				}
		}
	}

	memcpy(m_board->Now_Board, B, sizeof(int) * Board_Size * Board_Size);

	if (myturn == 0) {
		resultX = max.i;
		resultY = max.j;
		return TRUE;
	}
	else {
		resultX = min.i;
		resultY = min.j;
		return TRUE;
	}
	return FALSE;
}

int comparH(const void* a, const void* b) {
	Location* aa = (Location*)a;
	Location* bb = (Location*)b;
	return bb->g - aa->g;
}

struct {
	long long int checksum;
	char depth;
	char entry_type;                                //0=exact, 1=lower_bound, 2=upper_bound
	int g;
} hashtable[2][Hashsize];

int AIPlayer::search_next(int x, int y, int myturn, int mylevel, int alpha, int beta)
{
	long long int hash64;
	long long int hash32;
	if (mylevel >= search_deep) {
		hash64 = getHash();
		hash32 = hash64 & (Hashsize - 1);
		hashtable[myturn][hash32].checksum = hash64;
		hashtable[myturn][hash32].depth = 0;
		hashtable[myturn][hash32].entry_type = 0;
		hashtable[myturn][hash32].g = m_game->Compute_Grades(FALSE, 0);
		return hashtable[myturn][hash32].g;
	}

	if (m_board->Find_Legal_Moves(myturn) <= 0) {
		return m_game->Compute_Grades(FALSE, 1);
	}

	Search_Counter++;

	int i, j, k, g = 0;

	int B[Board_Size][Board_Size];
	int L[Board_Size][Board_Size];
	memcpy(B, m_board->Now_Board, sizeof(int) * Board_Size * Board_Size);
	memcpy(L, m_board->Legal_Moves, sizeof(int) * Board_Size * Board_Size);

	int fentry_type = 2;
	for (k = 0; k < Board_Size * Board_Size; k++) {
		i = history[myturn][mylevel][k].i;
		j = history[myturn][mylevel][k].j;
		if (L[i][j] == TRUE) {
			memcpy(m_board->Now_Board, B, sizeof(int) * Board_Size * Board_Size);
			m_board->Now_Board[i][j] = m_board->Stones_color[myturn];
			m_board->Check_Cross(i, j, TRUE);

			hash64 = getHash();
			hash32 = hash64 & (Hashsize - 1);

			int flag = false;
			if (zhash && hashtable[1 - myturn][hash32].checksum == hash64 &&
				hashtable[1 - myturn][hash32].depth >= search_deep - (mylevel + 1)) {
				if (hashtable[1 - myturn][hash32].entry_type == 0) {
					Hashnode++;
					g = hashtable[1 - myturn][hash32].g;
					flag = true;
				}
				else if (hashtable[1 - myturn][hash32].entry_type == 1) {
					if ((1 - myturn) == 0 && hashtable[1 - myturn][hash32].g >= beta) {
						Hashnode++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
					else if ((1 - myturn) == 1 && hashtable[1 - myturn][hash32].g <= alpha) {
						Hashnode++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
				}
				else if (hashtable[1 - myturn][hash32].entry_type == 2) {
					if ((1 - myturn) == 0 && hashtable[1 - myturn][hash32].g <= alpha) {
						Hashnode++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
					else if ((1 - myturn) == 1 && hashtable[1 - myturn][hash32].g >= beta) {
						Hashnode++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
				}

			}
			if (flag == false) {
				Searchnode++;
				g = search_next(i, j, 1 - myturn, mylevel + 1, alpha, beta);

			}

			if (myturn == 0) {      // max ply
				if (g > alpha) {
					alpha = g;
					history[myturn][mylevel][k].g += 2 * mylevel * mylevel;  //Обычный узел
					fentry_type = 0;
				}
				history[myturn][mylevel][k].g -= 2 * mylevel * mylevel;     //бедный узел
			}
			else {               // min ply
				if (g < beta) {
					beta = g;
					history[myturn][mylevel][k].g += 2 * mylevel * mylevel;  //Обычный узел
					fentry_type = 0;
				}
				history[myturn][mylevel][k].g -= 2 * mylevel * mylevel;     //бедный узел
			}
			// cutoff
			if (alpha_beta_option)
				if (alpha >= beta) {
					i = Board_Size;
					j = Board_Size;
					history[myturn][mylevel][k].g += 16 * mylevel * mylevel; //приоритетный узел
					fentry_type = 1;
					break;
				}
		}
	}

	memcpy(m_board->Now_Board, B, sizeof(int) * Board_Size * Board_Size);

	//Сортировка приоритета узла
	qsort(history[myturn][mylevel], Board_Size * Board_Size, sizeof(history[myturn][mylevel][0]), comparH);

	hash64 = getHash();
	hash32 = hash64 & (Hashsize - 1);

	if (myturn == 0) {   //max level
		if (hashtable[myturn][hash32].depth <= search_deep - mylevel) {
			hashtable[myturn][hash32].checksum = hash64;
			hashtable[myturn][hash32].depth = search_deep - mylevel;
			hashtable[myturn][hash32].entry_type = fentry_type;
			hashtable[myturn][hash32].g = alpha;
		}
		return alpha;
	}
	else {               //min level
		if (hashtable[myturn][hash32].depth <= search_deep - mylevel) {
			hashtable[myturn][hash32].checksum = hash64;
			hashtable[myturn][hash32].depth = search_deep - mylevel;
			hashtable[myturn][hash32].entry_type = fentry_type;
			hashtable[myturn][hash32].g = beta;
		}
		return beta;
	}
}

long long int AIPlayer::getHash(void)
{
	long long int hash = 0;
	for (int i = 0; i < Board_Size; i++) {
		for (int j = 0; j < Board_Size; j++) {
			if (m_board->Now_Board[i][j] == 1) {
				hash ^= zobrist[0][i][j];
			}
			else if (m_board->Now_Board[i][j] == 2) {
				hash ^= zobrist[1][i][j];
			}
		}
	}
	return hash;
}

long long int AIPlayer::rand64(void)
{
	return rand() ^ ((long long int)rand() << 15) ^ ((long long int)rand() << 30) ^ ((long long int)rand() << 45) ^ ((long long int)rand() << 60);
}

//int AIPlayer::comparH(const void* a, const void* b)
//{
//	Location* aa = (Location*)a;
//	Location* bb = (Location*)b;
//	return bb->g - aa->g;
//}


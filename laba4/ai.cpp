#include "ai.h"
#include <stdlib.h>
#include <stdio.h>
#include <memory.h>
#include <chrono>

void AIPlayer::computer_think(int* x, int* y)
{
	std::unordered_map<std::string, int>::iterator query;
	 query = m_db->opening_book.find(m_game->move_history);

	if ( query != this->m_db->opening_book.end()) {
		*y = (*query).second / 8;
		*x = (*query).second % 8;
		printf("Known opening! Computer takes next move from opening book.\n");
		return;
	}
	else {
		opening = false;

		//разрушение таблицы истории
		if (history_attenua != 0) {
			for (int i = 0; i < Board_Size * Board_Size; i++) {
				for (int j = 0; j < Board_Size * Board_Size; j++) {
					history[0][i][j].g *= history_attenua;
					history[1][i][j].g *= history_attenua;
				}
			}
		}

		//динамическая глубина
		if (dyn_deep_start <= m_game->cur_move_number && m_game->cur_move_number <= dyn_deep_end) {
			deep_end = dyn_deep + 2;
		}
		//Окончательный поиск
		else if (m_game->cur_move_number >= end_time) {
			deep_end = (Board_Size * Board_Size) - m_game->cur_move_number;
		}
		else {
			deep_end = dyn_deep;
		}

		resultX = resultY = -1;
		auto start_t = std::chrono::high_resolution_clock::now();

		int find_success = min_max(m_game->now_turn, 0);

		auto end_t = std::chrono::high_resolution_clock::now();
		auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);

		printf("computer thinking time= %f sec.\n", (duration.count() / 1e+9));

		if (find_success) {
			*x = resultX;
			*y = resultY;
		}
		else {
			*x = *y = -1;
			return;
		}
	}

}

void AIPlayer::Init(Game* game, Board* board, Database* db)
{
	m_board = board;
	m_game = game;
	m_db = db;

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

int AIPlayer::min_max(int myturn, int mylevel)
{
	if (m_board->find_legal_moves(myturn) <= 0)
		return FALSE;

	int B[Board_Size][Board_Size];
	int L[Board_Size][Board_Size];
	memcpy(B, m_board->now_board, sizeof(int) * Board_Size * Board_Size);
	memcpy(L, m_board->legal_moves, sizeof(int) * Board_Size * Board_Size);

	Location min, max;
	min.i = min.j = -1; min.g = INT_MAX;
	max.i = max.j = -1; max.g = INT_MIN;
	int i, j, k, test, g;
	int alpha = INT_MIN, beta = INT_MAX;

	for (k = 0; k < Board_Size * Board_Size; k++) {
		i = history[myturn][mylevel][k].i;
		j = history[myturn][mylevel][k].j;
		if (L[i][j] == TRUE) {
			memcpy(m_board->now_board, B, sizeof(int) * Board_Size * Board_Size);
			m_board->now_board[i][j] = m_board->stone_color[myturn];
			m_board->сheck_сross(i, j, true);

			//Итеративное углубление
			for (search_deep = deep_start, g = 0; search_deep <= deep_end; search_deep += 2) {
				if (f_iterative == FALSE) search_deep = deep_end;
				//MTD
				if (f_MTD) {
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
				else g = search_next(i, j, 1 - myturn, mylevel + 1, alpha, beta);
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
			if (f_alpha_beta_option)
				if (alpha >= beta) {
					i = Board_Size;
					j = Board_Size;
				}
		}
	}

	memcpy(m_board->now_board, B, sizeof(int) * Board_Size * Board_Size);

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
		hash64 = get_hash();
		hash32 = hash64 & (Hashsize - 1);
		hashtable[myturn][hash32].checksum = hash64;
		hashtable[myturn][hash32].depth = 0;
		hashtable[myturn][hash32].entry_type = 0;
		hashtable[myturn][hash32].g = m_game->compute_grades(FALSE, 0);
		return hashtable[myturn][hash32].g;
	}

	if (m_board->find_legal_moves(myturn) <= 0) {
		return m_game->compute_grades(FALSE, 1);
	}

	int i, j, k, g = 0;

	int B[Board_Size][Board_Size];
	int L[Board_Size][Board_Size];
	memcpy(B, m_board->now_board, sizeof(int) * Board_Size * Board_Size);
	memcpy(L, m_board->legal_moves, sizeof(int) * Board_Size * Board_Size);

	int fentry_type = 2;
	for (k = 0; k < Board_Size * Board_Size; k++) {
		i = history[myturn][mylevel][k].i;
		j = history[myturn][mylevel][k].j;
		if (L[i][j] == TRUE) {
			memcpy(m_board->now_board, B, sizeof(int) * Board_Size * Board_Size);
			m_board->now_board[i][j] = m_board->stone_color[myturn];
			m_board->сheck_сross(i, j, true);

			hash64 = get_hash();
			hash32 = hash64 & (Hashsize - 1);

			int flag = false;
			if (zhash && hashtable[1 - myturn][hash32].checksum == hash64 &&
				hashtable[1 - myturn][hash32].depth >= search_deep - (mylevel + 1)) {
				if (hashtable[1 - myturn][hash32].entry_type == 0) {
					hash_node++;
					g = hashtable[1 - myturn][hash32].g;
					flag = true;
				}
				else if (hashtable[1 - myturn][hash32].entry_type == 1) {
					if ((1 - myturn) == 0 && hashtable[1 - myturn][hash32].g >= beta) {
						hash_node++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
					else if ((1 - myturn) == 1 && hashtable[1 - myturn][hash32].g <= alpha) {
						hash_node++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
				}
				else if (hashtable[1 - myturn][hash32].entry_type == 2) {
					if ((1 - myturn) == 0 && hashtable[1 - myturn][hash32].g <= alpha) {
						hash_node++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
					else if ((1 - myturn) == 1 && hashtable[1 - myturn][hash32].g >= beta) {
						hash_node++;
						g = hashtable[1 - myturn][hash32].g;
						flag = true;
					}
				}

			}
			if (flag == false) {
				search_node++;
				g = search_next(i, j, 1 - myturn, mylevel + 1, alpha, beta);

			}

			if (myturn == 0) {      // max
				if (g > alpha) {
					alpha = g;
					history[myturn][mylevel][k].g += 2 * mylevel * mylevel;  //Обычный узел
					fentry_type = 0;
				}
				history[myturn][mylevel][k].g -= 2 * mylevel * mylevel;     //бедный узел
			}
			else {               // min 
				if (g < beta) {
					beta = g;
					history[myturn][mylevel][k].g += 2 * mylevel * mylevel;  //Обычный узел
					fentry_type = 0;
				}
				history[myturn][mylevel][k].g -= 2 * mylevel * mylevel;     //бедный узел
			}
			// cutoff
			if (f_alpha_beta_option)
				if (alpha >= beta) {
					i = Board_Size;
					j = Board_Size;
					history[myturn][mylevel][k].g += 16 * mylevel * mylevel; //приоритетный узел
					fentry_type = 1;
					break;
				}
		}
	}

	memcpy(m_board->now_board, B, sizeof(int) * Board_Size * Board_Size);

	//Сортировка приоритета узла
	qsort(history[myturn][mylevel], Board_Size * Board_Size, sizeof(history[myturn][mylevel][0]), comparH);

	hash64 = get_hash();
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

long long int AIPlayer::get_hash(void)
{
	long long int hash = 0;
	for (int i = 0; i < Board_Size; i++) {
		for (int j = 0; j < Board_Size; j++) {
			if (m_board->now_board[i][j] == 1) {
				hash ^= zobrist[0][i][j];
			}
			else if (m_board->now_board[i][j] == 2) {
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


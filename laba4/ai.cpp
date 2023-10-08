#include "ai.h"
#include <stdlib.h>
#include <stdio.h>
#include <memory.h>
#include <chrono>
void AIPlayer::Init(Game* game, Board* board, Database* db)
{
	m_board = board;
	m_game = game;
	m_db = db;

	//Инициализировать таблицу истории
	Position tempL;
	for (int i = 0, m = 0; i < Board_Size; i++) {
		for (int j = 0; j < Board_Size; j++, m++) {
			tempL.x = i;
			tempL.y = j;
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

void AIPlayer::set_min_base_deep(int value)
{
	min_base_deep = value;
}

void AIPlayer::computer_think(int* x, int* y)
{
	std::unordered_map<std::string, int>::iterator query;
	query = m_db->opening_book.find(m_game->move_history);

	if (query != this->m_db->opening_book.end()) {
		*y = (*query).second / 8;
		*x = (*query).second % 8;
		printf("Known opening! Computer takes next move from opening book.\n");
		return;
	}
	else {

		//разрушение таблицы истории
		if (history_attenua != 0) {
			for (int i = 0; i < Board_Size * Board_Size; i++) {
				for (int j = 0; j < Board_Size * Board_Size; j++) {
					history[0][i][j].g *= history_attenua;
					history[1][i][j].g *= history_attenua;
				}
			}
		}

		//Окончательный поиск
		if (m_game->cur_move_number >= m_game->end_time) {
			deep_end = (Board_Size * Board_Size) - m_game->cur_move_number;
			deep_start = 4;
			ending = true;
		}
		else {
			deep_end = min_base_deep;
		}

		resultX = resultY = -1;
		auto start_t = std::chrono::high_resolution_clock::now();

		int find_success = min_max(m_game->now_turn, 0);

		auto end_t = std::chrono::high_resolution_clock::now();
		auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t).count() / 1e+9;

		//if (!ending && min_deep > open_deep)min_deep = open_deep;
		if (max_search_time < duration)max_search_time = duration;

		printf("search time= %f sec moves: %d max deep: %d\n", duration, open_moves, open_deep);

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

int AIPlayer::min_max(int myturn, int mylevel)
{
	if (m_board->find_legal_moves(myturn) <= 0)
		return FALSE;

	int tmp_board[Board_Size][Board_Size];
	int tmp_legal_moves[Board_Size][Board_Size];
	std::memcpy(tmp_board, m_board->now_board, sizeof(int) * Board_Size * Board_Size);
	std::memcpy(tmp_legal_moves, m_board->legal_moves, sizeof(int) * Board_Size * Board_Size);

	Position min;
	min.x = min.y = -1;
	min.g = INT_MAX;

	Position max;
	max.x = max.y = -1;
	max.g = INT_MIN;

	int tmp_max_deep = deep_end;
	int test, g;
	int alpha = INT_MIN, beta = INT_MAX;
	int iteration = 0;
	open_deep = 0;

	for (int k = 0; k < Board_Size * Board_Size; k++) {
		int	i = history[myturn][mylevel][k].x;
		int	j = history[myturn][mylevel][k].y;
		if (tmp_legal_moves[i][j] == TRUE) {
			iteration++;
			std::memcpy(m_board->now_board, tmp_board, sizeof(int) * Board_Size * Board_Size);
			m_board->now_board[i][j] = m_board->stone_color[myturn];
			m_board->сheck_сross(i, j, true);

			deep_end = tmp_max_deep;
			//Итеративное углубление
			auto start = std::chrono::high_resolution_clock::now();

			for (search_deep = deep_start, g = 0; search_deep <= deep_end; search_deep += 2) {
				//MTD
				if (!ending) {
					auto end_t = std::chrono::high_resolution_clock::now();
					auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start).count() / 1e+9;
					if (duration > 0.1 && search_deep > min_base_deep) {
						break;
					}
					else {
						deep_end += 2;
					}
				}

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
				if (search_deep > open_deep)open_deep = search_deep;
			}

			if (myturn == 0) {      // max level
				if (g > max.g) {
					max.g = g;
					max.x = i;
					max.y = j;
				}
			}
			else {
				if (g < min.g) {    // min level
					min.g = g;
					min.x = i;
					min.y = j;
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

	std::memcpy(m_board->now_board, tmp_board, sizeof(int) * Board_Size * Board_Size);

	open_moves = iteration;

	if (myturn == 0) {
		resultX = max.x;
		resultY = max.y;
		return TRUE;
	}
	else {
		resultX = min.x;
		resultY = min.y;
		return TRUE;
	}
	return FALSE;
}

int comparH(const void* a, const void* b) {
	Position* aa = (Position*)a;
	Position* bb = (Position*)b;
	return bb->g - aa->g;
}

struct {
	long long int checksum;
	char depth;
	char entry_type;                                //0=exact, 1=lower_bound, 2=upper_bound
	int g;
} h_table[2][Hashsize];

int AIPlayer::search_next(int x, int y, int myturn, int mylevel, int alpha, int beta)
{
	long long int h64;
	long long int h32;
	if (mylevel >= search_deep) {
		h64 = get_position_hash();
		h32 = h64 & (Hashsize - 1);
		h_table[myturn][h32].checksum = h64;
		h_table[myturn][h32].depth = 0;
		h_table[myturn][h32].entry_type = 0;
		h_table[myturn][h32].g = m_game->compute_grades(FALSE, 0);
		return h_table[myturn][h32].g;
	}

	if (m_board->find_legal_moves(myturn) <= 0) {
		return m_game->compute_grades(FALSE, 1);
	}

	int i, j, k, g = 0;

	int tmp_board[Board_Size][Board_Size];
	int tmp_legal_moves[Board_Size][Board_Size];
	std::memcpy(tmp_board, m_board->now_board, sizeof(int) * Board_Size * Board_Size);
	std::memcpy(tmp_legal_moves, m_board->legal_moves, sizeof(int) * Board_Size * Board_Size);

	int fentry_type = 2;
	for (k = 0; k < Board_Size * Board_Size; k++) {
		i = history[myturn][mylevel][k].x;
		j = history[myturn][mylevel][k].y;
		if (tmp_legal_moves[i][j] == TRUE) {
			std::memcpy(m_board->now_board, tmp_board, sizeof(int) * Board_Size * Board_Size);
			m_board->now_board[i][j] = m_board->stone_color[myturn];
			m_board->сheck_сross(i, j, true);

			h64 = get_position_hash();
			h32 = h64 & (Hashsize - 1);

			int flag = false;
			if (h_table[1 - myturn][h32].checksum == h64 && h_table[1 - myturn][h32].depth >= search_deep - (mylevel + 1)) {
				if (h_table[1 - myturn][h32].entry_type == 0) {
					g = h_table[1 - myturn][h32].g;
					flag = true;
				}
				else if (h_table[1 - myturn][h32].entry_type == 1) {
					if ((1 - myturn) == 0 && h_table[1 - myturn][h32].g >= beta) {
						g = h_table[1 - myturn][h32].g;
						flag = true;
					}
					else if ((1 - myturn) == 1 && h_table[1 - myturn][h32].g <= alpha) {
						g = h_table[1 - myturn][h32].g;
						flag = true;
					}
				}
				else if (h_table[1 - myturn][h32].entry_type == 2) {
					if ((1 - myturn) == 0 && h_table[1 - myturn][h32].g <= alpha) {
						g = h_table[1 - myturn][h32].g;
						flag = true;
					}
					else if ((1 - myturn) == 1 && h_table[1 - myturn][h32].g >= beta) {
						g = h_table[1 - myturn][h32].g;
						flag = true;
					}
				}
			}

			if (flag == false) {
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

	std::memcpy(m_board->now_board, tmp_board, sizeof(int) * Board_Size * Board_Size);

	//Сортировка приоритета узла
	std::qsort(history[myturn][mylevel], Board_Size * Board_Size, sizeof(history[myturn][mylevel][0]), comparH);

	h64 = get_position_hash();
	h32 = h64 & (Hashsize - 1);

	if (myturn == 0) {   //max level
		if (h_table[myturn][h32].depth <= search_deep - mylevel) {
			h_table[myturn][h32].checksum = h64;
			h_table[myturn][h32].depth = search_deep - mylevel;
			h_table[myturn][h32].entry_type = fentry_type;
			h_table[myturn][h32].g = alpha;
		}
		return alpha;
	}
	else {               //min level
		if (h_table[myturn][h32].depth <= search_deep - mylevel) {
			h_table[myturn][h32].checksum = h64;
			h_table[myturn][h32].depth = search_deep - mylevel;
			h_table[myturn][h32].entry_type = fentry_type;
			h_table[myturn][h32].g = beta;
		}
		return beta;
	}
}

long long int AIPlayer::get_position_hash(void)
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


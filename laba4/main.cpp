#include "game.h"
#include "ai.h"
#include "database.h"

#include <iostream>
int main(int argc, char* argv[]) {
	char c[10];
	int prompt_y = -1, prompt_x = -1;
	int rx, ry, m = 0, n;

	Board g_board;
	Game g_game;
	AIPlayer g_ai;
	Database g_db;

	g_db.Init();
	g_board.Init();
	g_game.Init(&g_board);
	g_ai.Init(&g_game, &g_board, &g_db);

	if (argc == 3) {
		g_game.set_comp_take(static_cast<int>(*argv[2]) - '0');
		g_ai.set_dyn_deep_max(static_cast<int>(*argv[1]) - '0');
	}
	else if (argc == 2) {
		g_game.set_comp_take(static_cast<int>(*argv[1]) - '0');
	}
	else {
		std::cout << "Computer color is Black(0) or White(1)?" << std::endl;
		int compcolor;

		std::cin >> compcolor;
		g_game.set_comp_take(compcolor);
	}

	g_game.print_board_and_set_legal_moves();

	if (g_game.computer_take == 0) {
		g_ai.computer_think(&rx, &ry);
		std::cout << "computer played " << char(rx + 97) << " " << ry + 1 << std::endl;
		std::cerr << char(rx + 97) << ry + 1 << std::endl;
		g_game.play_move(rx, ry);
		g_game.print_board_and_set_legal_moves();
	}

	while (m++ < Board_Size * Board_Size) {
		for (;;) {
			if (!g_game.player_have_moves()) {
				m--;
				prompt_x = prompt_y = -1;
				g_game.play_move(prompt_x, prompt_y);
				break;
			}

			if (g_game.computer_take == 0) {
				std::cout << "input white move:(a-h 1-8), or undo (U/u)\n";
				std::cin >> c;
			}
			else if (g_game.computer_take == 1) {
				std::cout << "input black move:(a-h 1-8), or undo (U/u)\n";
				std::cin >> c;
			}

			if (c[0] == 'U' || c[0] == 'u') {
				prompt_x = prompt_y = -1;
				g_game.undo_move();
				g_game.print_board_and_set_legal_moves();
				m--;
				continue;
			}
			else {
				prompt_x = c[0] - 97;

				if (c[2] == '0') {
					prompt_y = 9;
				}
				else {
					prompt_y = c[1] - 49;
				}
			}
			if (g_game.now_turn != g_game.computer_take)std::memcpy(g_board.tmp_board, g_board.now_board,
				sizeof(int) * Board_Size * Board_Size);

			if (!g_game.play_move(prompt_x, prompt_y)) {
				std::cout << c[0] << prompt_y + 1 << " is a wong move!" << std::endl;
			}
			else break;
		}
		if (g_game.is_game_end())	return g_game.get_exit_code();
		g_game.print_board_and_set_legal_moves();

		g_ai.computer_think(&rx, &ry);
		std::cout << "computer played " << char(rx + 97) << ry + 1 << std::endl;
		std::cerr << char(rx + 97) << ry + 1 << std::endl;
		g_game.play_move(rx, ry);

		if (g_game.is_game_end())	return g_game.get_exit_code();
		g_game.print_board_and_set_legal_moves();

	}

	std::cout << "Game is over" << std::endl;
	std::cin >> n;

	return g_game.get_exit_code();
}
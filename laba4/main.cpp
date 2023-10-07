#include "game.h"
#include "ai.h"
#include <iostream>
int main(int argc, char* argv[]) {
	int compcolor;
	char c[10];
	int column_input = -1, row_input = -1;
	int rx, ry, m = 0, n;
	Board g_board;
	Game g_game;
	AIPlayer g_ai;

	g_board.Init();
	g_game.Init(&g_board);
	g_ai.Init(&g_game, &g_board);

	if (argc == 2) {
		g_game.set_comp_take(static_cast<int>(*argv[1]) - '0');
	}
	else {
		std::cout << "Computer color is Black(0) or White(1)?" << std::endl;
		std::cin >> compcolor;
		g_game.set_comp_take(compcolor);
	}

	g_game.Show_Board_and_Set_Legal_Moves();

	if (g_game.Computer_Take == 0) {
		g_ai.Computer_Think(&rx, &ry);
		std::cout << "Computer played " << char(rx + 97) << " " << ry + 1 << std::endl;
		std::cerr << char(rx + 97) << ry + 1 << std::endl;
		g_game.Play_a_Move(rx, ry);
		g_game.Show_Board_and_Set_Legal_Moves();
	}

	while (m++ < Board_Size * Board_Size) {
		for (;;) {

			if (g_game.Computer_Take == 0) {
				std::cout << "input White move:(a-h 1-8), or PASS\n";
				std::cin >> c;
			}
			else if (g_game.Computer_Take == 1) {
				std::cout << "input Black move:(a-h 1-8), or PASS\n";
				std::cin >> c;
			}
			if (c[0] == 'U' || c[0] == 'u')
				row_input = column_input = -1;

			else {
				row_input = c[0] - 97;

				if (c[2] == '0') {
					column_input = 9;
				}
				else {
					column_input = c[1] - 49;
				}
			}

			if (!g_game.Play_a_Move(row_input, column_input)) {
				std::cout << c[0] << " " << column_input + 1 << " is a Wrong move!" << std::endl;
			}
			else break;
		}
		if (g_game.Check_EndGame())	return g_game.get_exit_code();
		g_game.Show_Board_and_Set_Legal_Moves();

		g_ai.Computer_Think(&rx, &ry);
		std::cout << "Computer played " << char(rx + 97) << ry + 1 << std::endl;
		std::cerr << char(rx + 97) << ry + 1 << std::endl;
		g_game.Play_a_Move(rx, ry);

		if (g_game.Check_EndGame())	return g_game.get_exit_code();
		g_game.Show_Board_and_Set_Legal_Moves();

	}

	std::cout << "Game is over" << std::endl;
	std::cin >> n;

	return g_game.get_exit_code();
}
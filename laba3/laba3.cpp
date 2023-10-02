#include <iostream>
#include <bitset>
#include <chrono>
#include <numeric>

#include <queue>
#include <stack>
#include <unordered_map>
#include <unordered_set>
#include <random>
#include "bit_board.h"

void print_bin(unsigned long long a)
{
	for (int i = 1; i < 65; i++)
	{
		//Проверяем старший бит)
		if (a & 0x8000000000000000)
			std::cout << "1";
		else
			std::cout << "0";

		//Сдвигаем влево на 1 бит
		a = a << 1;
		if (i % 4 == 0)std::cout << " ";

	}
	std::cout << "\n";
}

std::vector<Position> BFS() {
	auto start_t = std::chrono::high_resolution_clock::now();
	std::vector<Position> result;
	std::queue<Position> q;
	std::unordered_set<std::bitset<N* N>> s;

	q.emplace(Position{ });


	while (!q.empty())
	{
		auto cur = std::move(q.front());
		q.pop();
		if (s.contains(cur.board))continue;
		s.insert(cur.board);

		if (cur.count_queens == N) {
			result.push_back(Position{ cur.board,cur.count_queens });
		}

		for (int x = 0; x < N; x++)
		{
			for (int y = 0; y < N; y++)
			{
				if (cur.is_safe(x, y)) {

					auto new_pos = cur.get_new_pos(x, y);
					q.emplace(Position{ new_pos,uint8_t(cur.count_queens + 1) });
				}

			}
		}
	}
	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "размерность: " << N << " время bfs " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
	return result;
}

std::vector<Position> DFS() {
	auto start_t = std::chrono::high_resolution_clock::now();
	std::vector<Position> result;
	std::stack<Position> q;
	std::unordered_set<std::bitset<N* N>> s;

	q.emplace(Position{ });

	while (!q.empty())
	{
		auto cur = std::move(q.top());
		q.pop();
		if (s.contains(cur.board))continue;

		s.insert(cur.board);

		if (cur.count_queens == N) {
			result.push_back(Position{ cur.board,cur.count_queens });
		}

		for (int x = 0; x < N; x++)
		{
			for (int y = 0; y < N; y++)
			{
				if (cur.is_safe(x, y)) {
					auto new_pos = cur.get_new_pos(x, y);
					q.emplace(Position{ new_pos,uint8_t(cur.count_queens + 1) });
				}

			}
		}
	}
	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "размерность: " << N << " время dfs " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
	return result;
}

std::vector<Position> IDS(int depth_limit) {
	auto start_t = std::chrono::high_resolution_clock::now();
	std::vector<Position> result;
	std::stack<Position> q;
	std::unordered_set<std::bitset<N* N>> s;

	q.emplace(Position{ });

	while (!q.empty())
	{
		auto cur = std::move(q.top());
		q.pop();
		if (s.contains(cur.board))continue;

		s.insert(cur.board);

		if (cur.count_queens == N) {
			result.push_back(Position{ cur.board,cur.count_queens });
		}
		if (cur.count_queens >= depth_limit) continue;

		for (int x = 0; x < N; x++)
		{
			for (int y = 0; y < N; y++)
			{
				if (cur.is_safe(x, y)) {
					auto new_pos = cur.get_new_pos(x, y);
					q.emplace(Position{ new_pos,uint8_t(cur.count_queens + 1) });
				}

			}
		}
	}
	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "размерность: " << N << " время ids " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
	return result;
}

std::vector<PositionN> GA(int population_size, double mutation_rate) {
	auto start_t = std::chrono::high_resolution_clock::now();
	// Выбор лучших решений
	std::vector<PositionN> population(population_size);
	std::sort(population.begin(), population.end(), CasualComparePositions());
	int  generations = 0;
	while (population[0].fitness() != 0)
	{
		std::vector<PositionN> parents(population.begin(), population.begin() + population_size / 2);

		// Скрещивание и мутация
		for (int i = 0; i < population_size / 2; ++i) {
			int parent1_idx = std::rand() % parents.size();
			int parent2_idx = std::rand() % parents.size();
			PositionN child = parents[parent1_idx].crossover(parents[parent2_idx]);
			PositionN mutable_child = child.mutate(mutation_rate);
			parents.push_back(mutable_child);
		}

		// Вычисление пригодности и сортировка
		std::sort(parents.begin(), parents.end(), CasualComparePositions());

		// Перенос лучших решений обратно в популяцию
		for (int i = population_size / 2, j = 0; i < population_size; ++i, j++) {
			population[i] = std::move(parents[j]);
		}

		generations++;
		std::sort(population.begin(), population.end(), CasualComparePositions());
		if (generations > 100000) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "размерность: " << N << " поколений: " << generations << " время GA " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			return	std::vector<PositionN>(population.begin(), population.begin() + population_size / 2);
		}
	}

	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "размерность: " << N << " поколений: " << generations << " время GA " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";

	// Возвращение лучших решений
	return std::vector<PositionN>(population.begin(), population.begin() + population_size / 2);
}

// Рассчет вероятности принятия худшего решения при текущей температуре
bool acceptance_rate(int cur_energy, int new_energy, double temperature) {
	if (new_energy < cur_energy) {
		return true;
	}
	double acceptance_probability = exp((cur_energy - new_energy) / temperature);
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_real_distribution<double> rand(0.0, 1.0);

	return rand(gen) < acceptance_probability;
}

PositionN SA(double initial_temperature, double mutation_rate, double cooling_rate, double f_marker, double l_marker) {
	auto start_t = std::chrono::high_resolution_clock::now();
	PositionN cur_pos{};
	PositionN best_solution = cur_pos;

	double cur_temp = initial_temperature;

	while (f_marker > 0.0) {

		PositionN new_solution = cur_pos.mutate(mutation_rate);

		int cur_energy = cur_pos.fitness();
		int new_energy = new_solution.fitness();

		if (new_energy == 0) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "размерность: " << N << " время SA " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			return new_solution;
		}

		if (acceptance_rate(cur_energy, new_energy, cur_temp)) {
			cur_pos = new_solution;
			if (new_energy < best_solution.fitness()) {
				best_solution = new_solution;
			}
		}

		f_marker -= cooling_rate;
	}

	while (cur_temp >= cooling_rate) {

		PositionN new_solution = best_solution.mutate(mutation_rate);

		int cur_energy = cur_pos.fitness();
		int new_energy = new_solution.fitness();

		if (new_energy == 0) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "размерность: " << N << " время SA " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			return new_solution;
		}

		if (acceptance_rate(cur_energy, new_energy, cur_temp)) {
			cur_pos = new_solution;
			if (new_energy < best_solution.fitness()) {
				best_solution = new_solution;
			}
		}

		cur_temp -= cooling_rate;
	}

	while (l_marker > 0.0) {

		PositionN new_solution = best_solution.mutate(mutation_rate);

		int cur_energy = cur_pos.fitness();
		int new_energy = new_solution.fitness();

		if (new_energy == 0) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "размерность: " << N << " время SA " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			return new_solution;
		}

		if (acceptance_rate(cur_energy, new_energy, cur_temp)) {
			cur_pos = new_solution;
			if (new_energy < best_solution.fitness()) {
				best_solution = new_solution;
			}
		}

		l_marker -= cooling_rate;
	}

	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "размерность: " << N << " время SA " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";

	return best_solution;
}

std::vector<Position> A_STAR() {
	auto start_t = std::chrono::high_resolution_clock::now();
	std::vector<Position> result;
	std::priority_queue<Position, std::vector<Position>, ComparePositions > q;
	std::unordered_set<std::bitset<N* N>> s;

	q.emplace(Position{ });

	while (!q.empty())
	{
		auto cur = q.top();
		q.pop();

		if (s.contains(cur.board))continue;
		if (cur.count_queens == N) {
			result.push_back(Position{ cur.board,cur.count_queens });
		}

		s.insert(cur.board);

		for (int x = 0; x < N; x++)
		{
			for (int y = 0; y < N; y++)
			{
				if (cur.is_safe(x, y)) {

					auto new_pos = cur.get_new_pos(x, y);
					if (!s.contains(new_pos)) {
						q.emplace(Position{ new_pos,uint8_t(cur.count_queens + 1) });
					}
				}
			}
		}
	}

	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "размерность: " << N << " время A* " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
	return result;
}

int main() {

	auto res1 = BFS();
	std::cout << "колличество решений: " << res1.size() << std::endl;

	//auto res2 = DFS();
	//std::cout << "колличество решений: " << res2.size() << std::endl;

	//auto res3 = IDS(N);
	//std::cout << "колличество решений: " << res3.size() << std::endl;

	auto res6 = A_STAR();
	std::cout << "колличество решений: " << res6.size() << std::endl;

	auto res4 = GA(10, 0.1);
	for (const auto& solution : res4) {
		solution.print_pos();
		std::cout << "Fitness: " << solution.fitness() << std::endl;
	}

	auto res5 = SA(100.0, 0.1, 0.1, 10.0, 10.0);
	std::cout << "решениe: ";
	res5.print_pos();
	std::cout << "колличество конфликтов: " << res5.fitness() << std::endl;



	//std::vector<PositionN> free;
	//free.emplace_back(PositionN{ {0, 5, 7, 2, 6, 3, 1, 4, } });
	//free.emplace_back(PositionN{ {0, 6, 4, 7, 1, 3, 5, 2, } });
	//free.emplace_back(PositionN{ {4,0,3,5,7,1,6,2} });
	//free.emplace_back(PositionN{ {1, 3, 5, 7, 2, 0, 6, 4, } });
	//free.emplace_back(PositionN{ {1, 4, 6, 0, 2, 7, 5, 3, } });
	//free.emplace_back(PositionN{ {2, 6, 1, 7, 4, 0, 3, 5, } });
	//free.emplace_back(PositionN{ {2, 4, 6, 0, 3, 1, 7, 5, } });
	//free.emplace_back(PositionN{ {2, 4, 1, 7, 5, 3, 6, 0, } });
	//free.emplace_back(PositionN{ {3, 6, 4, 1, 5, 0, 2, 7, } });
	//free.emplace_back(PositionN{ {3, 1, 6, 2, 5, 7, 0, 4, } });


	return 0;
}
//{4, 1, 7, 0, 3, 6, 2, 5, }
//Fitness: 0
//{5, 1, 3, 7, 4, 6, 2, 0, }
//Fitness: 8
//{4, 2, 7, 5, 3, 6, 1, 0, }
//Fitness: 8
//{6, 2, 4, 5, 0, 1, 3, 7, }
//Fitness: 10
//{2, 7, 0, 5, 6, 1, 4, 3, }
//Fitness: 20

//{5, 2, 4, 7, 0, 3, 1, 6, }
//Fitness: 0
//{0, 7, 3, 4, 1, 5, 2, 6, }
//Fitness: 8
//{4, 6, 7, 1, 3, 0, 2, 5, }
//Fitness: 8
//{3, 7, 5, 6, 0, 2, 4, 1, }
//Fitness: 10
//{0, 4, 6, 5, 1, 2, 3, 7, }
//Fitness: 12

//размерность: 80 время SA 112127901900 ns 112.128 s
//решениe : {44, 68, 13, 67, 12, 50, 41, 35, 54, 75, 43, 26, 9, 72, 53, 62, 76, 21, 40, 33, 15, 30, 42, 7, 16, 32, 56, 79, 59, 10, 24, 17, 69, 45, 77, 51, 36, 22, 25, 65, 78, 64, 61, 0, 71, 1, 49, 19, 31, 11, 48, 5, 58, 55, 29, 73, 38, 74, 23, 60, 28, 52, 2, 37, 27, 70, 46, 8, 34, 57, 6, 3, 18, 66, 63, 39, 4, 20, 47, 14, }
//колличество конфликтов : 2
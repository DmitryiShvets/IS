#include <iostream>
#include <chrono>

#include <queue>
#include <stack>
#include <unordered_map>

#include "bit_tag.h"
#include "staff.h"
#include "array_tag.h"

unsigned long long END = 1311768467463790320;



int bfs(const std::string& start) {
	auto start_t = std::chrono::high_resolution_clock::now();

	std::queue<Position*> q;
	std::unordered_map<unsigned long long, Position*> s;

	q.emplace(new Position{ start });

	if (!q.front()->is_valid_position()) {
		std::cout << "Неразрешимая позиция!\n";
		return -1;
	}

	while (!q.empty())
	{
		auto cur = q.front();
		q.pop();

		if (s.contains(cur->position))continue;

		s[cur->position] = cur;

		if (cur->last_move != -4) {
			if (cur->zero >= 4) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 4]) >> 16) | (cur->position & ~mask_a[cur->zero - 4]);
				if (new_pos == END) {
					auto end_t = std::chrono::high_resolution_clock::now();
					auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
					std::cout << "время bfs " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
					s[new_pos] = new Position{ cur,new_pos, (uint8_t)(cur->zero - 4),  4,cur->depth + 1 };
					save_positions(s[new_pos], "bfs.txt");
					for (auto& i : s)
					{
						delete i.second;
					}
					return s[new_pos]->depth;
				}
				q.emplace(new Position{ cur,new_pos, (uint8_t)(cur->zero - 4),  4,cur->depth + 1 });
			}
		}
		if (cur->last_move != 4) {
			if (cur->zero <= 11) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 4]) << 16) | (cur->position & ~mask_a[cur->zero + 4]);
				if (new_pos == END) {
					auto end_t = std::chrono::high_resolution_clock::now();
					auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
					std::cout << "время bfs " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
					s[new_pos] = new Position{ cur,new_pos, (uint8_t)(cur->zero + 4),  -4 ,cur->depth + 1 };
					save_positions(s[new_pos], "bfs.txt");
					for (auto& i : s)
					{
						delete i.second;
					}
					return s[new_pos]->depth;
				}
				q.emplace(new Position{ cur,new_pos, (uint8_t)(cur->zero + 4),  -4 ,cur->depth + 1 });
			}

		}
		if (cur->last_move != -1) {
			if (cur->zero % 4 != 0) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 1]) >> 4) | (cur->position & ~mask_a[cur->zero - 1]);
				if (new_pos == END) {
					auto end_t = std::chrono::high_resolution_clock::now();
					auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
					std::cout << "время bfs " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
					s[new_pos] = new Position{ cur,new_pos, (uint8_t)(cur->zero - 1),  1,cur->depth + 1 };
					save_positions(s[new_pos], "bfs.txt");
					for (auto& i : s)
					{
						delete i.second;
					}
					return s[new_pos]->depth;
				}
				q.emplace(new Position{ cur,new_pos, (uint8_t)(cur->zero - 1),  1,cur->depth + 1 });
			}

		}
		if (cur->last_move != 1) {
			if (cur->zero % 4 != 3) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 1]) << 4) | (cur->position & ~mask_a[cur->zero + 1]);
				if (new_pos == END) {
					auto end_t = std::chrono::high_resolution_clock::now();
					auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
					std::cout << "время bfs " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
					s[new_pos] = new Position{ cur,new_pos, (uint8_t)(cur->zero + 1),  -1 ,cur->depth + 1 };
					save_positions(s[new_pos], "bfs.txt");
					for (auto& i : s)
					{
						delete i.second;
					}
					return s[new_pos]->depth;
				}
				q.emplace(new Position{ cur,new_pos, (uint8_t)(cur->zero + 1),  -1 ,cur->depth + 1 });
			}

		}
	}
	return -1;

}

int dfs(const std::string& start) {
	auto start_t = std::chrono::high_resolution_clock::now();

	std::stack<Position*> q;
	std::unordered_map<unsigned long long, Position*> s;


	q.emplace(new Position{ start });

	if (!q.top()->is_valid_position()) {
		std::cout << "Неразрешимая позиция!\n";
		return -1;
	}

	while (!q.empty())
	{
		auto cur = q.top();
		q.pop();
		if (cur->position == END) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "время dfs " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			save_positions(cur, "dfs.txt");
			for (auto& i : s)
			{
				delete i.second;
			}
			return cur->depth;
		}

		if (s.contains(cur->position))continue;

		s[cur->position] = cur;

		if (cur->last_move != -4) {
			if (cur->zero >= 4) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 4]) >> 16) | (cur->position & ~mask_a[cur->zero - 4]);

				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 4), 4, cur->depth + 1 });
			}
		}
		if (cur->last_move != 4) {
			if (cur->zero <= 11) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 4]) << 16) | (cur->position & ~mask_a[cur->zero + 4]);

				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 4), -4, cur->depth + 1 });
			}

		}
		if (cur->last_move != -1) {
			if (cur->zero % 4 != 0) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 1]) >> 4) | (cur->position & ~mask_a[cur->zero - 1]);

				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 1), 1, cur->depth + 1 });
			}

		}
		if (cur->last_move != 1) {
			if (cur->zero % 4 != 3) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 1]) << 4) | (cur->position & ~mask_a[cur->zero + 1]);

				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 1), -1, cur->depth + 1 });
			}

		}
	}

	for (auto& i : s)
	{
		delete i.second;
	}
	return -1;

}

int ids(const std::string& start, int depth_limit) {
	auto start_t = std::chrono::high_resolution_clock::now();

	std::stack<Position*> q;
	std::unordered_map<unsigned long long, Position*> s;

	q.emplace(new Position{ start });

	if (!q.top()->is_valid_position()) {
		std::cout << "Неразрешимая позиция!\n";
		return -1;
	}

	while (!q.empty())
	{
		auto cur = q.top();
		q.pop();

		if (cur->position == END) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "found depth_limit: " << depth_limit << "время ids " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			save_positions(cur, "ids.txt");
			for (auto& i : s)
			{
				delete i.second;
			}
			return cur->depth;
		}

		auto previon_cur = s.find(cur->position);
		if (previon_cur != s.end()) {
			if ((*previon_cur).second->depth > cur->depth) {
				delete (*previon_cur).second;
				s.erase(previon_cur);
			}
			else continue;
		}

		s[cur->position] = cur;

		if (cur->depth < depth_limit) {
			if (cur->last_move != -4) {
				if (cur->zero >= 4) {
					auto new_pos = ((cur->position & mask_a[cur->zero - 4]) >> 16) | (cur->position & ~mask_a[cur->zero - 4]);

					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 4), 4, cur->depth + 1 });
				}
			}
			if (cur->last_move != 4) {
				if (cur->zero <= 11) {
					auto new_pos = ((cur->position & mask_a[cur->zero + 4]) << 16) | (cur->position & ~mask_a[cur->zero + 4]);

					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 4), -4, cur->depth + 1 });
				}

			}
			if (cur->last_move != -1) {
				if (cur->zero % 4 != 0) {
					auto new_pos = ((cur->position & mask_a[cur->zero - 1]) >> 4) | (cur->position & ~mask_a[cur->zero - 1]);

					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 1), 1, cur->depth + 1 });
				}

			}
			if (cur->last_move != 1) {
				if (cur->zero % 4 != 3) {
					auto new_pos = ((cur->position & mask_a[cur->zero + 1]) << 4) | (cur->position & ~mask_a[cur->zero + 1]);

					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 1), -1, cur->depth + 1 });
				}

			}
		}

	}
	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "not found depth_limit: " << depth_limit << " время IDA* " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
	for (auto& i : s)
	{
		delete i.second;
	}
	return -1;

}

int a_star(const std::string& start) {
	auto start_t = std::chrono::high_resolution_clock::now();

	std::priority_queue<Position*, std::vector<Position*>, ManhattanComparePositions> q;
	std::unordered_map<unsigned long long, Position*> s;

	q.emplace(new Position{ start });

	while (!q.empty())
	{
		auto cur = q.top();
		q.pop();
		if (cur->position == END) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "время A* " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			save_positions(cur, "astar.txt");
			for (auto& i : s)
			{
				delete i.second;
			}
			return cur->depth;
		}

		s[cur->position] = cur;

		if (cur->last_move != -4) {
			if (cur->zero >= 4) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 4]) >> 16) | (cur->position & ~mask_a[cur->zero - 4]);

				if (!s.contains(new_pos)) {
					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 4), 4, cur->depth + 1 });
				}
			}
		}
		if (cur->last_move != 4) {
			if (cur->zero <= 11) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 4]) << 16) | (cur->position & ~mask_a[cur->zero + 4]);

				if (!s.contains(new_pos)) {
					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 4), -4, cur->depth + 1 });
				}
			}

		}
		if (cur->last_move != -1) {
			if (cur->zero % 4 != 0) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 1]) >> 4) | (cur->position & ~mask_a[cur->zero - 1]);

				if (!s.contains(new_pos)) {
					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 1), 1, cur->depth + 1 });
				}
			}

		}
		if (cur->last_move != 1) {
			if (cur->zero % 4 != 3) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 1]) << 4) | (cur->position & ~mask_a[cur->zero + 1]);

				if (!s.contains(new_pos)) {
					q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 1), -1, cur->depth + 1 });
				}
			}

		}
	}

	for (auto& i : s)
	{
		delete i.second;
	}
	return -1;
}

int ida(const std::string& start, int bound) {
	int INF = 1e9;
	std::stack<Position*> q;
	std::unordered_map<unsigned long long, Position*> s;
	auto start_t = std::chrono::high_resolution_clock::now();

	q.emplace(new Position{ start });

	while (!q.empty())
	{
		auto cur = q.top();
		q.pop();

		auto f = cur->depth + cur->hdm();
		if (f > bound) {
			if (INF > f)INF = f;
			delete cur;
			continue;
		}


		if (cur->position == END) {
			auto end_t = std::chrono::high_resolution_clock::now();
			auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
			std::cout << "found 1 bound: " << bound << " время IDA* " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
			save_positions(cur, "ida.txt");
			for (auto& i : s)
			{
				delete i.second;
			}
			return 0;
		}

		s[cur->position] = cur;

		if (cur->last_move != -4) {
			if (cur->zero >= 4) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 4]) >> 16) | (cur->position & ~mask_a[cur->zero - 4]);
				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 4), 4, cur->depth + 1 });
			}
		}
		if (cur->last_move != 4) {
			if (cur->zero <= 11) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 4]) << 16) | (cur->position & ~mask_a[cur->zero + 4]);
				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 4), -4, cur->depth + 1 });
			}

		}
		if (cur->last_move != -1) {
			if (cur->zero % 4 != 0) {
				auto new_pos = ((cur->position & mask_a[cur->zero - 1]) >> 4) | (cur->position & ~mask_a[cur->zero - 1]);
				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero - 1), 1, cur->depth + 1 });
			}

		}
		if (cur->last_move != 1) {
			if (cur->zero % 4 != 3) {
				auto new_pos = ((cur->position & mask_a[cur->zero + 1]) << 4) | (cur->position & ~mask_a[cur->zero + 1]);
				q.emplace(new Position{ cur, new_pos, (uint8_t)(cur->zero + 1), -1, cur->depth + 1 });
			}

		}
	}
	auto end_t = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end_t - start_t);
	std::cout << "not found bound: " << bound << " время IDA* " << duration.count() << " ns " << duration.count() / 1e+9 << " s\n";
	for (auto& i : s)
	{
		delete i.second;
	}
	return INF;
}




int main() {


	std::string pos5{ "1234067859ACDEBF" };
	std::string pos19{ "12345678A0BE9FCD" };
	std::string pos22{ "12045738A6BE9FCD" };
	std::string pos27{ "51247308A6BE9FCD" };
	std::string pos48{ "04582E1DF79BCA36" };
	std::string pos52{ "FE169B4C0A73D852" };
	std::string pos55{ "D79F2E8A45106C3B" };
	std::string pos58{ "AF2C71E0B8634D59" };
	std::string pos61{ "BAC0F478E19623D5" };


	std::string start_pos = pos22;

	auto res1 = bfs(start_pos);
	std::cout << "count: " << res1 << std::endl;

	//auto res2 = dfs(start_pos);
	//std::cout << "count: " << res2 << std::endl;

	int res3;
	for (int i = 0; i < 100; i++)
	{
		res3 = ids(start_pos, i);
		if (res3 > 0)break;
	}
	std::cout << "count: " << res3 << std::endl;

	//auto res4 = a_star(start_pos);
	//std::cout << "count: " << res4 << std::endl;

	//int bound = Position::heuristic(hex_to_int(start_pos));
	//while (true) {
	//	bound = ida(start_pos, bound);
	//	if (bound == 0)break;
	//}
	//
	//std::cout << "count: " << bound << std::endl;

	return 0;
}

//not found bound : 41 время IDA * 1600 ns 1.6e-06 s
//not found bound : 43 время IDA * 12300 ns 1.23e-05 s
//not found bound : 45 время IDA * 77400 ns 7.74e-05 s
//not found bound : 47 время IDA * 725300 ns 0.0007253 s
//not found bound : 49 время IDA * 4774300 ns 0.0047743 s
//not found bound : 51 время IDA * 40825300 ns 0.0408253 s
//not found bound : 53 время IDA * 247856200 ns 0.247856 s
//not found bound : 55 время IDA * 1613966600 ns 1.61397 s
//not found bound : 57 время IDA * 10721144100 ns 10.7211 s
//not found bound : 59 время IDA * 79700620600 ns 79.7006 s
//found 1 bound : 61 время IDA * 137269282300 ns 137.269 s
//итого 230 сек
// laba1.cpp: определяет точку входа для приложения.
//

#include "laba1.h"

#include <vector>
#include <functional>
#include <chrono>

#include <queue>
#include <unordered_set>
#include <map>
class Node {
public:
	Node* parent;
	int value;

	Node(Node* _parent, int _value) :parent(_parent), value(_value) {}
};

inline int op1(int x) {
	return x + 3;
}
inline int op2(int x) {
	return x * 2;
}
inline int op3(int x) {
	return x - 2;
}

inline int rop1(int x) {
	return x - 3;
}
inline int rop2(int x) {
	return x / 2;
}
inline int rop3(int x) {
	return x + 2;
}


Node* task1(int a, int b, std::vector<std::function<int(int)>>& lambdas) {
	std::queue<Node*> q;
	std::unordered_set<int> s;
	auto start = new Node{ nullptr,a };
	q.push(start);

	while (!q.empty())
	{
		Node* cur = q.front();
		q.pop();
		s.insert(cur->value);

		for (auto& fn : lambdas) {
			int value = fn(cur->value);
			if (value > b)continue;
			if (!s.contains(value)) {
				auto tmp = new Node{ cur,value };
				q.push(tmp);
				s.insert(value);
			}
			if (value == b)return q.front();
		}
	}

	return nullptr;
}

Node* task2(int a, int b, std::vector<std::function<int(int)>>& lambdas) {
	std::queue<Node*> q;
	std::unordered_set<int> s;
	auto start = new Node{ nullptr,a };
	q.push(start);

	while (!q.empty())
	{
		Node* cur = q.front();
		q.pop();
		s.insert(cur->value);

		for (auto& fn : lambdas) {
			int value = fn(cur->value);
			if (value > b || value < a)continue;
			if (!s.contains(value)) {
				auto tmp = new Node{ cur,value };
				q.push(tmp);
				s.insert(value);
			}
			if (value == b)return q.front();
		}
	}

	return nullptr;
}

Node* task3(int b, int a, std::map<char, std::function<int(int)>>& lambdas) {
	std::queue<Node*> q;
	std::unordered_set<int> s;
	auto start = new Node{ nullptr,b };
	q.push(start);

	while (!q.empty())
	{
		Node* cur = q.front();
		q.pop();
		s.insert(cur->value);

		for (auto& x : lambdas) {
			if (x.first == '/' && cur->value % 2 != 0)continue;
			int value = x.second(cur->value);
			if (value < a || value > b)continue;
			if (!s.contains(value)) {
				auto tmp = new Node{ cur,value };
				q.push(tmp);
				s.insert(value);
			}
			if (value == a)return q.front();
		}
	}

	return nullptr;
}

std::pair<Node*, Node*> task4(int a, int b, std::vector<std::function<int(int)>>& op, std::map<char, std::function<int(int)>>& rop) {
	std::queue<Node*> q_left;
	std::queue<Node*> q_right;
	std::unordered_set<int> s_left;
	std::unordered_set<int> s_right;

	auto start = new Node{ nullptr,a };
	auto end = new Node{ nullptr,b };
	q_left.push(start);
	q_right.push(end);

	while (!q_left.empty() && !q_right.empty())
	{
		Node* cur_left = q_left.front();
		Node* cur_right = q_right.front();
		q_left.pop();
		q_right.pop();

		s_left.insert(cur_left->value);
		s_right.insert(cur_right->value);

		if (cur_left->value == cur_right->value) {

			std::cout << "встреча :" << cur_left->value << std::endl;
			return { cur_left,cur_right };
		}

		for (auto& fn : op) {
			int value = fn(cur_left->value);
			if (value > b || value < a)continue;
			if (!s_left.contains(value)) {
				auto tmp = new Node{ cur_left,value };
				q_left.push(tmp);
				s_left.insert(value);
			}
			//if (value == b)return q_left.front();
		}
		for (auto& x : rop) {
			if (x.first == '/' && cur_right->value % 2 != 0)continue;
			int value = x.second(cur_right->value);
			if (value > b || value < a)continue;
			if (!s_right.contains(value)) {
				auto tmp = new Node{ cur_right,value };
				q_right.push(tmp);
				s_right.insert(value);
			}
			//if (value == a)return q_right.front();
		}
	}

	return { nullptr,nullptr };
}
int task4a(int a, int b) {
	std::queue < std::pair<int, int>>q_left;
	std::queue < std::pair<int, int>> q_right;
	std::map<int, int> s_left;
	std::map<int, int> s_right;

	q_left.push(std::make_pair(a, 0));
	s_left[a] = 0;
	q_right.push(std::make_pair(b, 0));
	s_right[b] = 0;

	while (!q_left.empty() && !q_right.empty()) {
		int op = q_left.front().second;
		int rop = q_right.front().second;

		while (op == q_left.front().second) {
			int	numF = q_left.front().first;
			q_left.pop();

			if (s_right.find(numF) != s_right.end()) {
				return op + s_right[numF];
			}

			int add3 = numF + 3;
			int mult2 = numF * 2;

			if (s_left.count(add3) == 0) {
				q_left.push(std::make_pair(add3, op + 1));
				s_left[add3] = op + 1;
			}

			if (s_left.count(mult2) == 0) {
				q_left.push(std::make_pair(mult2, op + 1));
				s_left[mult2] = op + 1;
			}
		}

		while (rop == q_right.front().second) {
			int numB = q_right.front().first;
			q_right.pop();

			if (s_left.find(numB) != s_left.end()) {
				return rop + s_left[numB];
			}
			int sub3 = numB - 3;
			int div2 = numB / 2;
			if (s_right.count(sub3) == 0) {
				q_right.push(std::make_pair(sub3, rop + 1));
				s_right[sub3] = rop + 1;
			}
			if (numB % 2 == 0)
				if (s_right.count(div2) == 0) {
					q_right.push(std::make_pair(div2, rop + 1));
					s_right[div2] = rop + 1;
				}
		}
	}
	return -1;
}

int main()
{
	int count = 0;
	std::vector<std::function<int(int)>> lambdas;
	lambdas.push_back(op1);
	lambdas.push_back(op2);

	//lambdas.push_back(op3);

	std::map<char, std::function<int(int)>> map_lambdas;
	map_lambdas['/'] = rop2;
	map_lambdas['-'] = rop1;
	auto start_time = std::chrono::steady_clock::now();

	//Node* last = task2(2, 10000001, lambdas);
	//Node* last = task3(10000001, 2, map_lambdas);
	//while (last)
	//{
	//	count++;
	//	last = last->parent;
	//}
	//auto [left, right] = task4(2, 100, lambdas, map_lambdas);

	count = task4a(2, 10000001);

	auto end_time = std::chrono::steady_clock::now();
	auto elapsed_ns = std::chrono::duration_cast<std::chrono::nanoseconds>(end_time - start_time);

	std::cout << elapsed_ns.count() << " ns\n" << "count: " << count << std::endl;

	return 0;
}
//на деке при 100
// x64-Debug 153700 ns
// x64-Release 20500 ns
// x86-Debug 229300 ns
// x86-Release 17700 ns


//на очереди при 100 1 задание
// x64-Debug    57800 ns
// x64-Release   7900 ns
// x86-Debug     9200 ns
// x86-Release  17700 ns


//на очереди при 10000001 1 задание
// x64-Debug   9671635800 ns 
// x64-Release 2590164900 ns
// x86-Debug  14581367500 ns
// x86-Release 1940751900 ns


//на очереди при 10000001 3 задание
// x64-Debug       558700 ns  
// x64-Release      61000 ns
// x86-Debug       995100 ns
// x86-Release      56600 ns


//на очереди при 10000001 2 задание
// x64-Release 5363464500 ns с отсеканимем 7189256900 ns без отсекания
// x86-Release 4783751400 ns с отсеканимем 6500369500 ns без отсекания

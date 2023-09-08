// laba1.cpp: определяет точку входа для приложения.
//

#include "laba1.h"

#include <vector>
#include <functional>
#include <chrono>

#include <queue>
#include <unordered_set>
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

int main()
{
	int count = 0;
	std::vector<std::function<int(int)>> lambdas;
	lambdas.push_back(op2);
	lambdas.push_back(op1);
	lambdas.push_back(op3);

	auto start_time = std::chrono::steady_clock::now();

	Node* last = task1(2, 10000001, lambdas);

	while (last)
	{
		count++;
		last = last->parent;
	}

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

//на очереди при 10000001 2 задание
// x64-Release 5363464500 ns с отсеканимем 7189256900 ns без отсекания
// x86-Release 4783751400 ns с отсеканимем 6500369500 ns без отсекания
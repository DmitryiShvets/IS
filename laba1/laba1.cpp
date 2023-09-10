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
	int op_count = 0;
	Node(Node* _parent, int _value) :parent(_parent), value(_value) {}
	Node(Node* _parent, int _value, int op) :parent(_parent), value(_value), op_count(op) {}
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
	std::unordered_map<int, Node*>  s_left;
	std::unordered_map<int, Node*> s_right;

	q_left.push(new Node{ nullptr,a,0 });
	q_right.push(new Node{ nullptr,b,0 });
	s_left.emplace(a, q_left.front());
	s_right.emplace(b, q_right.front());
	while (!q_left.empty() && !q_right.empty())
	{
		Node* cur_left = q_left.front();
		int level_left = cur_left->op_count;

		while (cur_left->op_count == level_left)
		{
			cur_left = q_left.front();
			q_left.pop();

			for (auto& fn : op) {
				int value = fn(cur_left->value);
				if (value > b || value < a)continue;
				if (s_right.contains(value)) {
					//std::cout << "встреча :" << cur_left->value << std::endl;
					return { new Node{ cur_left,value,cur_left->op_count + 1 },s_right[value] };
				}
				if (!s_left.contains(value)) {
					auto tmp = new Node{ cur_left,value,cur_left->op_count + 1 };
					q_left.push(tmp);
					s_left.insert({ value,tmp });
				}
			}
		}
		Node* cur_right = q_right.front();
		int level_right = cur_right->op_count;

		while (cur_right->op_count == level_right)
		{
			cur_right = q_right.front();
			q_right.pop();

			for (auto& x : rop) {
				if (x.first == '/' && cur_right->value % 2 != 0)continue;
				int value = x.second(cur_right->value);
				if (value > b || value < a)continue;
				if (s_left.contains(value)) {
					//std::cout << "встреча :" << cur_right->value << std::endl;
					return { s_left[value],new Node{ cur_right,value,cur_right->op_count + 1 } };
				}
				if (!s_right.contains(value)) {
					auto tmp = new Node{ cur_right,value,cur_right->op_count + 1 };
					q_right.push(tmp);
					s_right.insert({ value,tmp });
				}
			}
		}



	}

	return { nullptr,nullptr };
}
//альтернативная реализациия без указателей и нод (к сожалению выигрыша не дала как я думал)
int task4b(int a, int b, std::vector<std::function<int(int)>>& op, std::map<char, std::function<int(int)>>& rop) {
	std::queue<std::pair<int, int>> q_left;
	std::queue<std::pair<int, int>> q_right;
	std::unordered_map<int, int>  s_left;
	std::unordered_map<int, int> s_right;

	q_left.emplace(a, 0);
	s_left.emplace(a, 0);
	q_right.emplace(b, 0);
	s_right.emplace(b, 0);
	while (!q_left.empty() && !q_right.empty())
	{
		int level_left = q_left.front().second;

		while (q_left.front().second == level_left)
		{
			int cur_left = q_left.front().first;
			q_left.pop();

			for (auto& fn : op) {
				int value = fn(cur_left);
				if (value > b || value < a)continue;
				if (s_right.contains(value)) {
					//std::cout << "встреча :" << cur_left->value << std::endl;
					return 1 + level_left + s_right[value];
				}
				if (!s_left.contains(value)) {
					q_left.emplace(value, level_left + 1);
					s_left.emplace(value, level_left + 1);
				}
			}
		}
		int level_right = q_right.front().second;

		while (q_right.front().second == level_right)
		{
			int cur_right = q_right.front().first;
			q_right.pop();

			for (auto& x : rop) {
				if (x.first == '/' && cur_right % 2 != 0)continue;
				int value = x.second(cur_right);
				if (value > b || value < a)continue;
				if (s_left.contains(value)) {
					//std::cout << "встреча :" << cur_right->value << std::endl;
					return 1 + s_left[value] + level_right;
				}
				if (!s_right.contains(value)) {
					q_right.emplace(value, level_right + 1);
					s_right.emplace(value, level_right + 1);
				}
			}
		}



	}

	return -1;
}

//ускоренная реализация c указателями и нодами (быстрее в 5 раз)
std::pair<Node*, Node*> task4a(int a, int b) {
	std::queue<Node*> q_left;
	std::queue<Node*> q_right;
	std::unordered_map<int, Node*>  s_left;
	std::unordered_map<int, Node*> s_right;

	q_left.push(new Node{ nullptr,a,0 });
	q_right.push(new Node{ nullptr,b,0 });
	s_left.emplace(a, q_left.front());
	s_right.emplace(b, q_right.front());

	Node* cur_left;
	Node* cur_right;

	while (!q_left.empty() && !q_right.empty())
	{
		cur_left = q_left.front();
		q_left.pop();

		if (s_right.contains(cur_left->value)) {
			//std::cout << "встреча :" << cur_left->value << std::endl;
			return { cur_left,s_right[cur_left->value] };
		}

		int l_value1 = cur_left->value * 2;
		if (l_value1 <= b && !s_left.contains(l_value1)) {
			s_left[l_value1] = new Node{ cur_left,l_value1,cur_left->op_count + 1 };
			q_left.push(s_left[l_value1]);
		}

		int l_value2 = cur_left->value + 3;
		if (l_value2 <= b && !s_left.contains(l_value2)) {
			s_left[l_value2] = new Node{ cur_left,l_value2,cur_left->op_count + 1 };
			q_left.push(s_left[l_value2]);
		}


		cur_right = q_right.front();
		q_right.pop();

		if (s_left.contains(cur_right->value)) {
			//std::cout << "встреча :" << cur_right->value << std::endl;
			return { s_left[cur_right->value], cur_right };
		}

		if (cur_right->value % 2 == 0) {
			int r_value1 = cur_right->value / 2;
			if (r_value1 >= a && !s_right.contains(r_value1)) {
				s_right[r_value1] = new Node{ cur_right,r_value1,cur_right->op_count + 1 };
				q_right.push(s_right[r_value1]);
			}
		}

		int r_value2 = cur_right->value - 3;
		if (r_value2 >= a && !s_right.contains(r_value2)) {
			s_right[r_value2] = new Node{ cur_right,r_value2,cur_right->op_count + 1 };
			q_right.push(s_right[r_value2]);
		}

	}

	return { nullptr,nullptr };
}



int main()
{

	int count = 0;

	auto start = std::chrono::high_resolution_clock::now();

	//Node* last = task2(2, 10000001, lambdas);
	//Node* last = task3(10000001, 2, map_lambdas);
	//while (last)
	//{
	//	count++;
	//	last = last->parent;
	//}

	//auto [left, right] = task4(2, 10000001, lambdas, map_lambdas); // 554100 ns
	//count = task4b(2, 10000001, lambdas, map_lambdas);             // 529100 ns

	auto [left, right] = task4a(2, 10000001);                        // 109800 ns

	auto end = std::chrono::high_resolution_clock::now();
	auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end - start);

	if (left && right) {                                           
		count = left->op_count + right->op_count;				   
	}
	std::cout << duration.count() << " ns\n" << "count: " << count << std::endl;

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

//на очереди при 10000001 3 задание
// x64-Debug       558700 ns  
// x64-Release      61000 ns
// x86-Debug       995100 ns
// x86-Release      56600 ns

//на очереди при 10000001 4 задание
// x64-Release      91200 ns
// x86-Release     109800 ns
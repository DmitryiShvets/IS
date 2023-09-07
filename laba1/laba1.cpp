// laba1.cpp: определяет точку входа для приложения.
//

#include "laba1.h"

#include <vector>
#include <deque>
#include <chrono>
class Node {
public:
	Node* parent;
	int value;
	char op;

	Node(Node* _parent, int _value, int _op) :parent(_parent), value(_value), op(_op) {

	}
};

Node* add(int index, std::deque<Node*>& dec) {
	if (dec[index]->value == 100)return dec[index];
	if (dec[index]->value > 100)return dec[index - 1];
	if (dec[index]->value * 2 <= 100) {
		auto tmp2 = new Node{ dec[index],dec[index]->value * 2 ,'*' };
		dec.emplace_back(tmp2);
	}
	if (dec[index]->value + 3 <= 100) {
		auto tmp1 = new Node{ dec[index],dec[index]->value + 3 ,'+' };
		dec.emplace_back(tmp1);
	}


	add(index++, dec);
}
int main()
{
	auto start_time = std::chrono::steady_clock::now();
	Node* a = new Node{ nullptr,2,' ' };
	std::deque<Node*>dec;
	dec.push_back(a);
	int index = 0;
	Node* cur = dec[index];

	while (cur->value != 100)
	{
		if (dec[index]->value * 2 <= 100) {
			auto tmp2 = new Node{ dec[index],dec[index]->value * 2 ,'*' };
			dec.emplace_back(tmp2);
		}
		if (dec[index]->value + 3 <= 100) {
			auto tmp1 = new Node{ dec[index],dec[index]->value + 3 ,'+' };
			dec.emplace_back(tmp1);
		}

		cur = dec[++index];

	}




	std::vector<char> operations;
	int count = 0;
	if (cur) {

		auto tmp = cur;
		while (tmp)
		{
			//std::cout << tmp->value << " ";
			//operations.push_back(tmp->op);
			tmp = tmp->parent;
			count++;
		}
		/*std::cout << std::endl;
		std::reverse(operations.begin(), operations.end());
		for (auto x : operations) {
			std::cout << x << " ";
		}*/
	}
	else {
		std::cout << "error" << std::endl;
	}
	auto end_time = std::chrono::steady_clock::now();
	auto elapsed_ns = std::chrono::duration_cast<std::chrono::nanoseconds>(end_time - start_time);
	std::cout << elapsed_ns.count() << " ns\n" << "count: " << count << std::endl;

	//std::vector<>
	
	return 0;
}

// x64-Debug 153700 ns
// x64-Release 20500 ns
// x86-Debug 229300 ns
// x86-Release 17700 ns
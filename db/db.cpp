// db.cpp: исходный файл для целевого объекта.
//

#include "db.h"
#include <iostream>
#include <fstream>
#include <string>

int main()
{
	std::string line;
	std::cout << "start" << std::endl;
	std::ifstream in("op1.txt"); // окрываем файл для чтения
	std::ofstream out("openings_v2.txt");          // поток для записи
	
	if (in.is_open())
	{
		if (out.is_open())
		{
			
			while (std::getline(in, line))
			{
				while (line.length() > 2)
				{
					auto last = line.find_last_of(',');
					auto pref = line.substr(0, last);
					auto sub = line.substr(last + 1);
					out << pref << "," << std::endl;
					out << sub << std::endl;
					//std::cout << pref << "," << std::endl;
					//std::cout << sub << std::endl;
					line = pref;
				}
			}
		}	
	}
	in.close();     // закрываем файл
	out.close();
	int p;
	std::cin >> p;
}

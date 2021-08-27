#include <iostream>
#pragma comment(lib,"ws2_32.lib")
#include <WinSock2.h>
#include <string>
#pragma warning(disable: 4996)
#include <fstream>




SOCKET connection;

int main() {
	WSAData wsaData;
	WORD Dllversion MAKEWORD(2, 1);
	if (WSAStartup(Dllversion, &wsaData) != 0) {
		std::cout << "Error" << std::endl;
		exit(1);
	}

	SOCKADDR_IN addr;
	int sizeofaddr = sizeof(addr);
	addr.sin_addr.s_addr = inet_addr("127.0.0.1");
	addr.sin_port = htons(1111);
	addr.sin_family = AF_INET;

	
	
		
	connection = socket(AF_INET, SOCK_STREAM, NULL);
	if(connect(connection, (SOCKADDR*)&addr, sizeof(addr))!=0)
	{
		std::cout << "Error: faild connect to server!" << std::endl;
		return 1;
	}
	

	std::cout << "Connected" << std::endl;
	char msg[256];
	recv(connection, msg, sizeof(msg), NULL);
	std::cout << msg << std::endl;

	


	std::string msg1;
	
	while (true)
	{
		
		std::getline(std::cin, msg1);

		// Открываем файл на чтение
		std::ifstream fin;
		fin.open(msg1);
		std::string file_name = msg1;

		//Определяем размер файла в байтах
		long file_size;
		fin.seekg(0, std::ios::end);
		file_size = fin.tellg();

		//Определяем расширение
		int st = file_name.find('.');
		int last_b = sizeof(file_name) - st;
		std::string ext = file_name.substr(st, last_b);

		std::cout << file_size << std::endl;


		//Отправляем имя файла и его размер
		send(connection, file_name.c_str(), 256, NULL);
		Sleep(5);
		send(connection,ext.c_str(),10,NULL);
		Sleep(5);
		send(connection, (char*)&file_size, sizeof(long), NULL);
		Sleep(5);
		//std::cout << file_name.substr(st, last_b) << std::endl;
		
		//пересчет количества строк
		char str[1024];
		fin.seekg(0, std::ios::beg);
		int line = 0;
		while (!fin.eof())
		{
			fin.getline(str,1024);
			line++;
		}

		// запись файла
		char* Buffer;
		fin.seekg(0, std::ios::beg);
		int i = 0;
		int size = 0;
		while (!fin.eof())
		{
			if ( (i+1) * 1024 > file_size)
			{
				size = file_size - i * 1024 - line;
			}
			else
			{
				size = 1024;
			}
			Buffer = new char[size+1];
			fin.read(Buffer, 1024);
			Buffer[size] = '\0';
			std::cout << Buffer << std::endl;
			send(connection, Buffer, 1024, NULL);
			Sleep(3);
			i++;
		} 
		fin.close();



		
	}
	

	


	system("pause");
	closesocket(connection);
	WSACleanup();
	return 0;
}
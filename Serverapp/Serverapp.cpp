#include <iostream>
#pragma comment(lib,"ws2_32.lib")
#include <WinSock2.h>
#pragma warning(disable:4996)
#include <fstream>





SOCKET connections[100];
size_t counter = 0;

void ClientHendler(size_t index)
{
	char msg[1024];
	int msg_size = 0;
	while (true)
	{
		// Получаем имя файла
		char msg2[256];
		recv(connections[index], msg2, sizeof(msg2), NULL);
		Sleep(5);

		//Получаем расширение файла
		char ext[10];
		recv(connections[index], ext, sizeof(ext), NULL);
		Sleep(5);

		// Получаем размерфайла в байтах
		int bytes_recv = recv(connections[index], (char*)&msg_size, sizeof(int), 0);
		if (bytes_recv != SOCKET_ERROR)
		{
			//Записываем в файл содержимое присылаемого файла
			std::ofstream fout;
			fout.open(msg2);
			int cnt = (msg_size / 1024);
			if (msg_size % 1024) 
			{
				cnt++;
			}
			
			for (size_t i = 0; i < cnt; i++)
			{
				recv(connections[index], msg, 1024, NULL);
				fout << msg;
				Sleep(3);
			}
			fout.close();
			
			std::cout << "User " << index << ": " << msg2 << " bytes: " << msg_size << " extension: " << ext << std::endl;
		}
		else
		{
			closesocket(connections[index]);
		}
		
		
	}
	delete[] msg;
};


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

	SOCKET sListen = socket(AF_INET, SOCK_STREAM, NULL);
	bind(sListen, (SOCKADDR*)&addr, sizeof(addr));
	listen(sListen, SOMAXCONN);

	SOCKET newConnection;

	for (size_t i = 0; i < 100; i++)
	{
		newConnection = accept(sListen, (SOCKADDR*)&addr, &sizeofaddr);

		if (newConnection == 0)
		{
			std::cout << "Error #2\n" << std::endl;
		}
		else
		{
			std::cout << "Client connected!" << std::endl;
			char msg[256] = "hello";
			
			send(newConnection, msg, sizeof(msg), NULL);

			connections[i] = newConnection;
			counter++;
			CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ClientHendler, (LPVOID)(i), NULL, NULL);

		}


		


	}
	system("pause");
	closesocket(sListen);
	WSACleanup();
	return 0;
}

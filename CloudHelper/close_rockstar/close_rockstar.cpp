#include <windows.h>
#include <tlhelp32.h>
#include <string>
#include <iostream>

void KillProcessByName(const std::wstring& processName) {
    HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hSnapshot == INVALID_HANDLE_VALUE) {
        std::cerr << "Failed to create snapshot of processes." << std::endl;
        return;
    }

    PROCESSENTRY32 pe;
    pe.dwSize = sizeof(PROCESSENTRY32);

    if (Process32First(hSnapshot, &pe)) {
        do {
            if (std::wstring(pe.szExeFile) == processName) {
                HANDLE hProcess = OpenProcess(PROCESS_TERMINATE, FALSE, pe.th32ProcessID);
                if (hProcess != NULL) {
                    TerminateProcess(hProcess, 0);
                    CloseHandle(hProcess);
                    std::wcout << L"Process " << processName << L" terminated." << std::endl;
                }
                break;
            }
        } while (Process32Next(hSnapshot, &pe));
    }

    CloseHandle(hSnapshot);
}

void StartProcess(const std::wstring& path) {
    STARTUPINFO si = { sizeof(si) };
    PROCESS_INFORMATION pi;

    if (CreateProcess(
        NULL,
        const_cast<LPWSTR>(path.c_str()),
        NULL,
        NULL,
        FALSE,
        0,
        NULL,
        NULL,
        &si,
        &pi)) {
        CloseHandle(pi.hProcess);
        CloseHandle(pi.hThread);
        std::wcout << L"Process " << path << L" started." << std::endl;
    }
    else {
        std::cerr << "Failed to start process." << std::endl;
    }
}

int main() {
    std::wstring processName = L"Launcher.exe";
    std::wstring processPath = L"C:\\Program Files\\Rockstar Games\\Launcher\\Launcher.exe";

    // Закрываем процесс
    KillProcessByName(processName);

    // Ждем 2 секунды
    Sleep(2000);

    // Запускаем процесс снова
    StartProcess(processPath);

    // Отсоединяем консоль
    FreeConsole();

    return 0;
}
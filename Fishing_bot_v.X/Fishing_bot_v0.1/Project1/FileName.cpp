#include <iostream>
#include <Windows.h>
#include <thread>
#include "imgui.h"
#include "imgui_impl_win32.h"
#include "imgui_impl_dx11.h"

using namespace std;

bool isRunning = false; // Флаг выполнения программы

// Функция для получения цвета пикселя
COLORREF GetPixelColor(HDC hdc, int x, int y) {
    return GetPixel(hdc, x, y);
}

// Функция для проверки цвета пикселя
bool IsPixelColor(COLORREF color, int r, int g, int b) {
    return (GetRValue(color) == r && GetGValue(color) == g && GetBValue(color) == b);
}

// Функция для нажатия пробела
void PressSpace() {
    keybd_event(VK_SPACE, 0, 0, 0);
    keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, 0);
    cout << "Пробел нажат!" << endl;
}

// Алгоритм работы программы
void RunProgram() {
    const int xStart = 686, yStart = 895;
    const int xEnd = 1221;
    const int searchColorR1 = 74, searchColorG1 = 223, searchColorB1 = 57;
    const int searchColorR2 = 248, searchColorG2 = 248, searchColorB2 = 248;
    const int searchColorR3 = 166, searchColorG3 = 166, searchColorB3 = 166;
    const int x2 = 1432, y2 = 959;

    HDC hdc = GetDC(NULL);

    while (isRunning) {
        // Поиск зеленого пикселя
        int foundX = -1;
        for (int x = xStart; x <= xEnd; ++x) {
            COLORREF color = GetPixelColor(hdc, x, yStart);
            if (IsPixelColor(color, searchColorR1, searchColorG1, searchColorB1)) {
                foundX = x;
                cout << "Пиксель найден на координате x: " << foundX << endl;
                break;
            }
        }

        if (foundX != -1) {
            while (true) {
                COLORREF color1 = GetPixelColor(hdc, foundX + 40, yStart);
                COLORREF color2 = GetPixelColor(hdc, foundX - 40, yStart);

                if (IsPixelColor(color1, searchColorR2, searchColorG2, searchColorB2) ||
                    IsPixelColor(color2, searchColorR2, searchColorG2, searchColorB2)) {
                    PressSpace();
                    break;
                }

                if (!isRunning) break;
                this_thread::sleep_for(chrono::milliseconds(100));
            }
        }

        this_thread::sleep_for(chrono::seconds(3));

        // Поиск второго пикселя
        while (isRunning) {
            COLORREF color = GetPixelColor(hdc, x2, y2);
            if (IsPixelColor(color, searchColorR3, searchColorG3, searchColorB3)) {
                PressSpace();
                break;
            }
            this_thread::sleep_for(chrono::milliseconds(100));
        }

        this_thread::sleep_for(chrono::seconds(3));
    }

    ReleaseDC(NULL, hdc);
}

// Основная программа с ImGui
int main() {
    // Инициализация окна с ImGui
    if (!ImGui_ImplWin32_Init(NULL) || !ImGui_ImplDX11_Init(NULL, NULL)) {
        cerr << "Ошибка инициализации ImGui" << endl;
        return -1;
    }

    thread workerThread;

    // Основной цикл
    while (true) {
        ImGui_ImplDX11_NewFrame();
        ImGui_ImplWin32_NewFrame();
        ImGui::NewFrame();

        ImGui::Begin("Простое меню");

        if (ImGui::Button(isRunning ? "Остановить" : "Запустить")) {
            if (isRunning) {
                isRunning = false;
                if (workerThread.joinable()) workerThread.join();
            }
            else {
                isRunning = true;
                workerThread = thread(RunProgram);
            }
        }

        ImGui::End();

        ImGui::Render();
        ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());

        if (GetAsyncKeyState(VK_ESCAPE)) break; // Выход по ESC
    }

    isRunning = false;
    if (workerThread.joinable()) workerThread.join();

    ImGui_ImplDX11_Shutdown();
    ImGui_ImplWin32_Shutdown();
    ImGui::DestroyContext();

    return 0;
}
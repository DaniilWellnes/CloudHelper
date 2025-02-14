#include <iostream>
#include <Windows.h>
#include <thread>
#include <atomic>

using namespace std;

atomic<bool> isRunning(false); // Флаг выполнения программы

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

// Основной алгоритм
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
            while (isRunning) {
                COLORREF color1 = GetPixelColor(hdc, foundX + 40, yStart);
                COLORREF color2 = GetPixelColor(hdc, foundX - 40, yStart);

                if (IsPixelColor(color1, searchColorR2, searchColorG2, searchColorB2) ||
                    IsPixelColor(color2, searchColorR2, searchColorG2, searchColorB2)) {
                    PressSpace();
                    break;
                }
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
    cout << "Работа программы остановлена." << endl;
}

int main() {
    setlocale(LC_ALL, "ru");

    thread workerThread;

    while (true) {
        cout << "Меню:\n";
        cout << "1. Запустить\n";
        cout << "2. Остановить\n";
        cout << "3. Выйти\n";
        cout << "Выберите действие: ";
        int choice;
        cin >> choice;

        if (choice == 1) {
            if (!isRunning) {
                isRunning = true;
                workerThread = thread(RunProgram);
                cout << "Программа запущена.\n";
            }
            else {
                cout << "Программа уже запущена.\n";
            }
        }
        else if (choice == 2) {
            if (isRunning) {
                isRunning = false;
                if (workerThread.joinable()) {
                    workerThread.join();
                }
                cout << "Программа остановлена.\n";
            }
            else {
                cout << "Программа не запущена.\n";
            }
        }
        else if (choice == 3) {
            if (isRunning) {
                isRunning = false;
                if (workerThread.joinable()) {
                    workerThread.join();
                }
            }
            cout << "Выход из программы.\n";
            break;
        }
        else {
            cout << "Неверный выбор. Попробуйте снова.\n";
        }
    }

    return 0;
}
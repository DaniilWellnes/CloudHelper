#include <iostream>
#include <Windows.h>
#include <thread>
#include <conio.h>

using namespace std;

// Функция для получения цвета пикселя по заданным координатам
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

int main() {
    setlocale(LC_ALL, "ru");

    

    const int xStart = 686, yStart = 895;
    const int xEnd = 1221;
    const int searchColorR1 = 74, searchColorG1 = 223, searchColorB1 = 57;
    const int searchColorR2 = 248, searchColorG2 = 248, searchColorB2 = 248;
    const int searchColorR3 = 166, searchColorG3 = 166, searchColorB3 = 166;
    const int x2 = 1432, y2 = 959;

    HDC hdc = GetDC(NULL); // Получение контекста устройства экрана

    while (true) {
        
        start_point:  // Метка

        //ПОИСК ЗЕЛЁНОГО ПИКСЕЛЯ
        
        // Поиск первого пикселя с цветом RGB = 74, 223, 57
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
            // Проверка двух точек (x+40, y) и (x-40, y)
            while (true) {
                COLORREF color1 = GetPixelColor(hdc, foundX + 40, yStart);
                COLORREF color2 = GetPixelColor(hdc, foundX - 40, yStart);

                if (IsPixelColor(color1, searchColorR2, searchColorG2, searchColorB2) ||
                    IsPixelColor(color2, searchColorR2, searchColorG2, searchColorB2)) {
                    PressSpace();
                    break;
                }

                this_thread::sleep_for(chrono::milliseconds(100)); // Задержка перед следующей проверкой
            }
        }

        // Задержка перед началом следующего поиска
        this_thread::sleep_for(chrono::seconds(3));

        //ПОИСК ЛУЧШЕЙ ТОЧКИ БЕЛОГО ПИКСЕЛЯ

        // Поиск второго пикселя с цветом RGB = 166, 166, 166
        while (true) {
            COLORREF color = GetPixelColor(hdc, x2, y2);
            if (IsPixelColor(color, searchColorR3, searchColorG3, searchColorB3)) {
                PressSpace();
                break;
            }

            this_thread::sleep_for(chrono::milliseconds(100)); // Задержка перед следующей проверкой
        }

        //ReleaseDC(NULL, hdc); // Освобождение контекста устройства
        cout << "ожидание Е" << endl;
        while (true) {

            if (_kbhit()) {  // Проверяем, была ли нажата клавиша
                char ch = _getch();  // Считываем нажатую клавишу
                if (ch == 0 || ch == 224) {  // Это специальные клавиши, например, стрелки
                    ch = _getch();  // Считываем код клавиши, если это специальная клавиша
                }

                if (ch == 'е' || ch == 'Е') {  // Проверяем, нажата ли русская "е"
                    cout << "е нажата" << endl;
                    goto start_point;
                    break;
                }
                else if (ch == 'e' || ch == 'E') {  // Проверяем, нажата ли английская "e"
                    cout << "е нажата" << endl;
                    goto start_point;
                    break;
                }
            }
        }

        // Задержка перед новой итерацией
        this_thread::sleep_for(chrono::seconds(3));
    }

    

    return 0;
}

#include <iostream>
#include <Windows.h>
#include <thread>
#include <conio.h>

using namespace std;

// ������� ��� ��������� ����� ������� �� �������� �����������
COLORREF GetPixelColor(HDC hdc, int x, int y) {
    return GetPixel(hdc, x, y);
}

// ������� ��� �������� ����� �������
bool IsPixelColor(COLORREF color, int r, int g, int b) {
    return (GetRValue(color) == r && GetGValue(color) == g && GetBValue(color) == b);
}

// ������� ��� ������� �������
void PressSpace() {
    keybd_event(VK_SPACE, 0, 0, 0);
    keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, 0);
    cout << "������ �����!" << endl;
}

int main() {
    setlocale(LC_ALL, "ru");

    

    const int xStart = 686, yStart = 895;
    const int xEnd = 1221;
    const int searchColorR1 = 74, searchColorG1 = 223, searchColorB1 = 57;
    const int searchColorR2 = 248, searchColorG2 = 248, searchColorB2 = 248;
    const int searchColorR3 = 166, searchColorG3 = 166, searchColorB3 = 166;
    const int x2 = 1432, y2 = 959;

    HDC hdc = GetDC(NULL); // ��������� ��������� ���������� ������

    while (true) {
        
        start_point:  // �����

        //����� ��˨���� �������
        
        // ����� ������� ������� � ������ RGB = 74, 223, 57
        int foundX = -1;
        for (int x = xStart; x <= xEnd; ++x) {
            COLORREF color = GetPixelColor(hdc, x, yStart);
            if (IsPixelColor(color, searchColorR1, searchColorG1, searchColorB1)) {
                foundX = x;
                cout << "������� ������ �� ���������� x: " << foundX << endl;
                break;
            }
        }

        if (foundX != -1) {
            // �������� ���� ����� (x+40, y) � (x-40, y)
            while (true) {
                COLORREF color1 = GetPixelColor(hdc, foundX + 40, yStart);
                COLORREF color2 = GetPixelColor(hdc, foundX - 40, yStart);

                if (IsPixelColor(color1, searchColorR2, searchColorG2, searchColorB2) ||
                    IsPixelColor(color2, searchColorR2, searchColorG2, searchColorB2)) {
                    PressSpace();
                    break;
                }

                this_thread::sleep_for(chrono::milliseconds(100)); // �������� ����� ��������� ���������
            }
        }

        // �������� ����� ������� ���������� ������
        this_thread::sleep_for(chrono::seconds(3));

        //����� ������ ����� ������ �������

        // ����� ������� ������� � ������ RGB = 166, 166, 166
        while (true) {
            COLORREF color = GetPixelColor(hdc, x2, y2);
            if (IsPixelColor(color, searchColorR3, searchColorG3, searchColorB3)) {
                PressSpace();
                break;
            }

            this_thread::sleep_for(chrono::milliseconds(100)); // �������� ����� ��������� ���������
        }

        //ReleaseDC(NULL, hdc); // ������������ ��������� ����������
        cout << "�������� �" << endl;
        while (true) {

            if (_kbhit()) {  // ���������, ���� �� ������ �������
                char ch = _getch();  // ��������� ������� �������
                if (ch == 0 || ch == 224) {  // ��� ����������� �������, ��������, �������
                    ch = _getch();  // ��������� ��� �������, ���� ��� ����������� �������
                }

                if (ch == '�' || ch == '�') {  // ���������, ������ �� ������� "�"
                    cout << "� ������" << endl;
                    goto start_point;
                    break;
                }
                else if (ch == 'e' || ch == 'E') {  // ���������, ������ �� ���������� "e"
                    cout << "� ������" << endl;
                    goto start_point;
                    break;
                }
            }
        }

        // �������� ����� ����� ���������
        this_thread::sleep_for(chrono::seconds(3));
    }

    

    return 0;
}

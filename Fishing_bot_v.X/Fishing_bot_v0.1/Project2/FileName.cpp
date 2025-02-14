#include <iostream>
#include <Windows.h>
#include <thread>
#include <atomic>

using namespace std;

atomic<bool> isRunning(false); // ���� ���������� ���������

// ������� ��� ��������� ����� �������
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

// �������� ��������
void RunProgram() {
    const int xStart = 686, yStart = 895;
    const int xEnd = 1221;
    const int searchColorR1 = 74, searchColorG1 = 223, searchColorB1 = 57;
    const int searchColorR2 = 248, searchColorG2 = 248, searchColorB2 = 248;
    const int searchColorR3 = 166, searchColorG3 = 166, searchColorB3 = 166;
    const int x2 = 1432, y2 = 959;

    HDC hdc = GetDC(NULL);

    while (isRunning) {
        // ����� �������� �������
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

        // ����� ������� �������
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
    cout << "������ ��������� �����������." << endl;
}

int main() {
    setlocale(LC_ALL, "ru");

    thread workerThread;

    while (true) {
        cout << "����:\n";
        cout << "1. ���������\n";
        cout << "2. ����������\n";
        cout << "3. �����\n";
        cout << "�������� ��������: ";
        int choice;
        cin >> choice;

        if (choice == 1) {
            if (!isRunning) {
                isRunning = true;
                workerThread = thread(RunProgram);
                cout << "��������� ��������.\n";
            }
            else {
                cout << "��������� ��� ��������.\n";
            }
        }
        else if (choice == 2) {
            if (isRunning) {
                isRunning = false;
                if (workerThread.joinable()) {
                    workerThread.join();
                }
                cout << "��������� �����������.\n";
            }
            else {
                cout << "��������� �� ��������.\n";
            }
        }
        else if (choice == 3) {
            if (isRunning) {
                isRunning = false;
                if (workerThread.joinable()) {
                    workerThread.join();
                }
            }
            cout << "����� �� ���������.\n";
            break;
        }
        else {
            cout << "�������� �����. ���������� �����.\n";
        }
    }

    return 0;
}
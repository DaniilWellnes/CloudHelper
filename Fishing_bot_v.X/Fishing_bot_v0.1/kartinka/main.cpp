#include <Windows.h>
#include <iostream>
#include <chrono>
#include <thread>

// ������� ��� ������� ������
HBITMAP captureScreen() {
    // �������� ���������� ������
    HWND hwnd = GetDesktopWindow();
    HDC hdcScreen = GetDC(hwnd);
    HDC hdcMem = CreateCompatibleDC(hdcScreen);

    // �������� ������ ������
    RECT screenRect;
    GetClientRect(hwnd, &screenRect);
    int width = screenRect.right;
    int height = screenRect.bottom;

    // ������� ����������� ��� �������
    HBITMAP hBitmap = CreateCompatibleBitmap(hdcScreen, width, height);
    SelectObject(hdcMem, hBitmap);

    // �������� ����� � ������
    BitBlt(hdcMem, 0, 0, width, height, hdcScreen, 0, 0, SRCCOPY);

    // ����������� �������
    DeleteDC(hdcMem);
    ReleaseDC(hwnd, hdcScreen);

    return hBitmap;
}

void trackPixelMovement() {
    // ������ ������
    HBITMAP hBitmap = captureScreen();

    // �������� ������ �����������
    BITMAP bmp;
    GetObject(hBitmap, sizeof(bmp), &bmp);

    // �������, ������� �� ����������� (��������, ������� � ������ ������)
    int centerX = bmp.bmWidth / 2; // ����� �� ��� X
    int centerY = bmp.bmHeight / 2; // ����� �� ��� Y

    // ��������� ���� ������� � ������
    HDC hdcMem = CreateCompatibleDC(NULL);
    SelectObject(hdcMem, hBitmap);
    COLORREF pixelColor = GetPixel(hdcMem, centerX, centerY);

    // ��� ������� ����� ���������� ��������� �������
    static int prevPositionX = centerX;  // ��������� ������� �� ��� X
    static bool isFirstFrame = true; // ���� ��� ������� �����

    if (isFirstFrame) {
        prevPositionX = centerX;
        isFirstFrame = false;
    }

    // ���������� ������� ������� � ����������
    if (centerX > prevPositionX) {
        std::cout << "������" << std::endl;
    }
    else if (centerX < prevPositionX) {
        std::cout << "�����" << std::endl;
    }

    // ��������� �������
    prevPositionX = centerX;

    // ����������� �������
    DeleteObject(hBitmap);
    DeleteDC(hdcMem);
}

int main() {
    setlocale(LC_ALL, "ru");
    while (true) {
        std::this_thread::sleep_for(std::chrono::seconds(1));  // ������� ���������� 1 �������

        // ������������ �������� �������
        trackPixelMovement();
    }

    return 0;
}

#include <Windows.h>
#include <iostream>
#include <chrono>
#include <thread>

// Функция для захвата экрана
HBITMAP captureScreen() {
    // Получаем дескриптор экрана
    HWND hwnd = GetDesktopWindow();
    HDC hdcScreen = GetDC(hwnd);
    HDC hdcMem = CreateCompatibleDC(hdcScreen);

    // Получаем размер экрана
    RECT screenRect;
    GetClientRect(hwnd, &screenRect);
    int width = screenRect.right;
    int height = screenRect.bottom;

    // Создаем изображение для захвата
    HBITMAP hBitmap = CreateCompatibleBitmap(hdcScreen, width, height);
    SelectObject(hdcMem, hBitmap);

    // Копируем экран в память
    BitBlt(hdcMem, 0, 0, width, height, hdcScreen, 0, 0, SRCCOPY);

    // Освобождаем ресурсы
    DeleteDC(hdcMem);
    ReleaseDC(hwnd, hdcScreen);

    return hBitmap;
}

void trackPixelMovement() {
    // Захват экрана
    HBITMAP hBitmap = captureScreen();

    // Получаем данные изображения
    BITMAP bmp;
    GetObject(hBitmap, sizeof(bmp), &bmp);

    // Пиксель, который мы отслеживаем (например, пиксель в центре экрана)
    int centerX = bmp.bmWidth / 2; // Центр по оси X
    int centerY = bmp.bmHeight / 2; // Центр по оси Y

    // Извлекаем цвет пикселя в центре
    HDC hdcMem = CreateCompatibleDC(NULL);
    SelectObject(hdcMem, hBitmap);
    COLORREF pixelColor = GetPixel(hdcMem, centerX, centerY);

    // Для первого кадра записываем начальную позицию
    static int prevPositionX = centerX;  // Начальная позиция по оси X
    static bool isFirstFrame = true; // Флаг для первого кадра

    if (isFirstFrame) {
        prevPositionX = centerX;
        isFirstFrame = false;
    }

    // Сравниваем текущую позицию с предыдущей
    if (centerX > prevPositionX) {
        std::cout << "Вправо" << std::endl;
    }
    else if (centerX < prevPositionX) {
        std::cout << "Влево" << std::endl;
    }

    // Обновляем позицию
    prevPositionX = centerX;

    // Освобождаем ресурсы
    DeleteObject(hBitmap);
    DeleteDC(hdcMem);
}

int main() {
    setlocale(LC_ALL, "ru");
    while (true) {
        std::this_thread::sleep_for(std::chrono::seconds(1));  // Частота обновления 1 секунда

        // Отслеживание движения пикселя
        trackPixelMovement();
    }

    return 0;
}

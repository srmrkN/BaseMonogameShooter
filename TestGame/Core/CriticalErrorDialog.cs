// TestGame/Core/CriticalErrorDialog.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestGame.Core;

public static class CriticalErrorDialog
{
    public static void Show()
    {
        string[] messages =
        {
            "Ошибка null. Неизвестно.",
            "Ошибка 0x5A10C: Буфер обмена уничтожен.",
            "ERROR 0xVOID0000000000000000000000000000000000000000000000null000000000000000000000000000000000000000000000000000000000: Неизвестная сущность в памяти.",
            "lethal EXCEPTION 0xDEATH2D: Текстурный массив превысил лимит в 18446744073709551616 пикселей.",
            "SYSTEM CRASH 0x: Видеопоток прерван в блоке: void_creature.",
            "MELTDOWN 0xsystem.HELLO: Шейдерная программа не может быть скомпилирована из-за отсутствия памяти.",
            "FATAL 0xM3M0RYL34K: Память системы переписана рекурсивной функцией: void_creature.DELETE.",
            "WARNING 0xH34RTB34T: Частота кадров синхронизирована с частотой обновления null.",
            "EXCEPTION 0xS1L3NTV01D: Звуковая подсистема более не функционирует. Звуки ОВтиТФнгнв.",
            "ERROR 0xF1L3S34RCH: Найден нераспознанный файл '[].vva' в рабочей директории.",
        };
        var rnd = new Random();
        string msg = messages[rnd.Next(messages.Length)];
        ShowSystemDialogAt(msg);
    }

    private static void ShowSystemDialogAt(string message)
    {
        var dummyForm = new Form
        {
            StartPosition = FormStartPosition.Manual, // Явно указываем позиционирование
            Location = new Point(0, 0),               // Положение для невидимой формы
            Size = new Size(1, 1),                    // Минимальный размер
            ShowInTaskbar = false,                    // Не показывать в панели задач
            TopMost = true,                           // Поверх всех окон
            FormBorderStyle = FormBorderStyle.None,   // Без рамки
            Opacity = 0,                              // Полностью прозрачная
            ShowIcon = false,
            MinimizeBox = false,
            MaximizeBox = false,
            ControlBox = false
        };

        bool messageBoxShown = false;

        dummyForm.Shown += (s, e) =>
        {
            if (!messageBoxShown)
            {
                messageBoxShown = true;
                MessageBox.Show(dummyForm, DistortText(message, new Random()), DistortText("оно тут.", new Random()), MessageBoxButtons.OK, MessageBoxIcon.Error);
                dummyForm.Close(); 
            }
        };
        
        Application.Run(dummyForm);

        static string DistortText(string text, Random rnd)
        {
            char[] chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (rnd.NextDouble() < 0.05) // 5% шанс искажения
                    chars[i] = (char)rnd.Next(33, 127); // Случайный символ ASCII
            }
            return new string(chars);
        }
    }
}
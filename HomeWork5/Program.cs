using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork5
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = Read();
            if (num == -1)
            {
                Console.WriteLine("Ошибка! Число в файле отсутствует!");
                return;
            }
            else if (num < 0 || num > 1_000_000_000)
            {
                Console.WriteLine($"Ошибка! Число {num} не укладывается в диапозон от 1 до 1_000_000_000!");
                return;
            }
            Console.WriteLine($"Из файла считано число N = {num}.");
            Console.Write("\nВведите цифру соответствующую варианту:\n ");
            Console.WriteLine($"1. Рассчитать группы чисел от 1 до {num} и вывести их на экран, " +
                $"при этом числа в каждой отдельно взятой группе не делятся друг на друга." +
                $" \n 2. Посмотреть колличество групп этих чисел.");
            int variant = int.Parse(Console.ReadLine());
            while (variant < 1 || variant > 2)
            {
                Console.WriteLine("Ошибка! Выбран не существующий вариант!Повторите ввод:");
                variant =int.Parse(Console.ReadLine());
            }
            switch (variant)
            {
                case 1:
                    char key;
                    CalcOfNum(num);
                    PrintResult();
                    Console.WriteLine("\nПоместить файл с рассчитанными группами в архив? д -\"Да\"/ н - \"Нет\" ");
                    key = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(key) =='д') Archiving();
                    break;
                case 2:
                    Console.WriteLine($"Колличество групп для чисел от 1 до {num}: {GetNumOfGroup(num)}");
                    break;
            }
        }
        static void Write(int n, StringBuilder text)
        {
            string path = "Numbers.txt";
            using (StreamWriter sw = new StreamWriter(path,true))
            {
                sw.WriteLine($"Группа {n}: {text};");
            }
        }
        static int GetNumOfGroup(int num) => (int)Math.Log(num, 2.0)+1;
        static void CalcOfNum(int num)
        {
            string path = "Numbers.txt";
            FileInfo file = new FileInfo(path);
            file.Delete();
            StringBuilder groupNum = new StringBuilder();
            long k = 1, m = 1;
            int i = 1;
            while (true)
            {
                if (k == m || k % m != 0)
                {
                    groupNum.Append(k.ToString() + ", ");
                    k++;
                }
                else
                {
                    groupNum.Remove(groupNum.Length - 2, 2);
                    Write(i, groupNum);
                    groupNum.Clear();
                    m = k;
                    i++;
                }
                if (k > num)
                {
                    groupNum.Remove(groupNum.Length - 2, 2);
                    Write(i, groupNum);
                    break;
                }
            }
        }
        static int Read()
        {
            string path = "Number.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                int num;
                if(!sr.EndOfStream)
                    return  num = int.Parse(sr.ReadLine().ToString());
                return -1;
            }
        }
        static void PrintResult()
        {
            string path = "Numbers.txt";
            Console.WriteLine();
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                    Console.WriteLine(sr.ReadLine());
            }
        }
        static void Archiving()
        {
            string path = "Numbers.txt";
            string zip = "Numbers.zip";
            using (FileStream origFile = new FileStream(path, FileMode.Open))
            {
                using(FileStream compFile = File.Create(zip))
                {
                    using (GZipStream archFile = new GZipStream(compFile, CompressionMode.Compress))
                    {
                        origFile.CopyTo(archFile);
                        Console.WriteLine($"\nАрхивация файла {path} завершена. " +
                            $" Размер файла до архивации {origFile.Length} байт" +
                            $" Размер файла после архивации {compFile.Length} байт");
                    }
                }
            }
        }
    }
}

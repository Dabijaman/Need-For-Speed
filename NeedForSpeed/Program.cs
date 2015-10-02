using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NeedForSpeed
{
    struct Car
    {
        public int x;
        public int y;
        public char c;
        public ConsoleColor color;
    }
    class Program
    {
        static void PrintOnPossition(int x, int y,char c, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(x, y);//poziciqta na koqto pishem
            Console.ForegroundColor = color;//cveta na kolichkata e cveta, zadaden v parametrite
            Console.Write(c);
            //PrintCars(x,y);//slagame kolichka na tazi poziciq
        }

        static void PrintStringOnPossition(int x, int y, string str, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(str);
        }
        //static string PrintCars(int x, int y)
        //{
            //char[,] cars = new char[3, 2];
            //cars[0, 0] = '*';
            //cars[0, 1] = '*';
            //cars[1, 0] = '.';
            //cars[1, 1] = '.';
            //cars[2, 0] = '*';
            //cars[2, 1] = '*';

            //for (int i = 0; i < cars.GetLength(0); i++)
            //{
            //    for (int n = 0; n < cars.GetLength(1); n++)
            //    {
            //        Console.Write(cars[i, n]);
            //    }
            //    Console.WriteLine();
            //}
        //    string cars = new string('*', 2);
        //    string cars2 = new string('.', 2); 
        //    new string('*', 2);
        //    Console.WriteLine(cars);
        //    return cars;
        //}

        static void Main(string[] args)
        {
            double speed = 100.0;
            int livesCount = 3;
            int playFiledWidth = 40;//redrow playfield
            Console.BufferHeight = Console.WindowHeight = 50;
            Console.BufferWidth = Console.WindowWidth = 70;
            //razmera na conzolata da e kolkoto e prozoreca.Da nqma skroler
            

           
            Car userCar = new Car();
            userCar.x = 20;
            userCar.y = Console.WindowHeight - 1;
            userCar.c = '@';
            userCar.color = ConsoleColor.Red;
            Random randomGenerator = new Random();

            List<Car> cars = new List<Car>();
            while (true)
            {
                bool hitted = false;
                speed++;
                if (speed > 450)
                {
                    speed = 450; 
                }
                
                {
                    Car newCar = new Car();//vseki pyt da syzdava nova kolichka
                    newCar.color = ConsoleColor.White;
                    newCar.x = randomGenerator.Next(0, playFiledWidth);
                    newCar.y = 0;
                    newCar.c = 'V';
                    cars.Add(newCar);
                }
                //move cars
                List<Car> newCarsList = new List<Car>();

                for (int i = 0; i < cars.Count; i++)//for zashtoto foreach ne ni dava da promenqme stoinosti, a samo da gi chetem
                {
                    Car oldCar = cars[i];
                    Car newCar = new Car();
                    newCar.x = oldCar.x;
                    newCar.y = oldCar.y + 1;
                    newCar.c = oldCar.c;
                    newCar.color = oldCar.color;
                    if (newCar.y == userCar.y && newCar.x == userCar.x)//check if other cars are hitting us
                    {
                        livesCount--;
                        hitted = true;
                        if (livesCount <= 0)
                        {
                            PrintStringOnPossition(50, 25, "GAME OVER", ConsoleColor.Red);
                            PrintStringOnPossition(50, 29, "Pres enter to exit", ConsoleColor.Red);
                            Console.WriteLine();
                            Environment.Exit(0);//spira izpylnenieto na programata
                        }
                    }
                    if (newCar.y < Console.WindowHeight)
                    {
                        newCarsList.Add(newCar);
                    }
          
                }
                cars = newCarsList;
                //move our car(key pressed)
                if (Console.KeyAvailable)// Poneje ReadKey e chakashta operaciq, i za da ne se dvijat vs koli, samo kogato dvijim nashata
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true); //buferira vs klavishi za da ne zabavq kogato zadyrjim nqkoi
                    }
                    if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        if (userCar.x - 1 >= 0)
                        {
                            userCar.x = userCar.x - 1;
                        }
                    }
                    else if (pressedKey.Key == ConsoleKey.RightArrow)
                    {
                        if (userCar.x + 1 < playFiledWidth)
                        {
                            userCar.x = userCar.x + 1;
                        }
                    }
                 
                }

           
                //clear the console
                Console.Clear();
              
                //draw info
              
                foreach (Car car in cars)
                {
                    PrintOnPossition(car.x, car.y, car.c, car.color);
                }
                if (hitted)
                {
                    PrintOnPossition(userCar.x, userCar.y, 'X',ConsoleColor.Red);
                }
                else
                {
                    PrintOnPossition(userCar.x, userCar.y, userCar.c, userCar.color);
                }

                PrintStringOnPossition(50,20,"Lives: "+livesCount,ConsoleColor.Yellow);
                //slow down program
                Thread.Sleep((int)(500 - speed));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        //print text
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
            int points = 0;
            int playFieledWidth = 40;// width of the play field 
            Console.BufferHeight = Console.WindowHeight = 50;
            int playFieldHeight = Console.WindowHeight; // height of the play field
            Console.BufferWidth = Console.WindowWidth = 70;
            //razmera na conzolata da e kolkoto e prozoreca.Da nqma skroler

            using (SoundPlayer music = new SoundPlayer("../../POL-stealth-mode-short.wav"))
            {
                music.PlayLooping();
            }

            //our car
            Car userCar = new Car();
            userCar.x = (playFieldHeight / 2);
            userCar.y = Console.WindowHeight - 1;
            userCar.c = '@';
            userCar.color = ConsoleColor.Red;

            Random randomGenerator = new Random();

            List<Car> cars = new List<Car>();
            while (true)
            {
                bool hitted = false;
                speed++;
                if (speed > 560)
                {
                    speed = 560;
                }

                {
                    Car newCar = new Car();//vseki pyt da syzdava nova kolichka
                    newCar.color = ConsoleColor.White;
                    newCar.x = randomGenerator.Next(0, playFieledWidth);
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

                    //check if we are hit
                    if (newCar.y == userCar.y && newCar.x == userCar.x)
                    {
                        livesCount--;
                        hitted = true;
                        //game over
                        if (livesCount <= 0)
                        {
                            PrintStringOnPossition(50, 25, "GAME OVER", ConsoleColor.Red);
                            PrintStringOnPossition(50, 29, "Pres enter to exit", ConsoleColor.Red);
                            Console.WriteLine();
                            Environment.Exit(0);//application exit
                        }
                    }

                    //adding points
                    if (newCar.y == playFieldHeight)
                    {
                        points++;
                    }
                    if (newCar.y < Console.WindowHeight)
                    {
                        newCarsList.Add(newCar);
                    }

                }
                cars = newCarsList;
                
                #region Movement //car movement(key pressed)
                if (Console.KeyAvailable)// Poneje ReadKey e chakashta operaciq, i za da ne se dvijat vs koli, samo kogato dvijim nashata
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true); //buferira vs klavishi za da ne zabavq kogato zadyrjim nqkoi
                    }
                    // moving to the left
                    if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        if (userCar.x - 1 >= 0)
                        {
                            userCar.x = userCar.x - 1;
                        }
                    }
                    //moving to the right
                    else if (pressedKey.Key == ConsoleKey.RightArrow)
                    {
                        if (userCar.x + 1 < playFieledWidth)
                        {
                            userCar.x = userCar.x + 1;
                        }
                    }
                    //moving up
                    else if (pressedKey.Key == ConsoleKey.UpArrow)
                    {
                        if (userCar.y > 1) // where other cars spawns
                        {
                            userCar.y = userCar.y - 1;
                        }
                    }
                    //moving down
                    else if (pressedKey.Key == ConsoleKey.DownArrow)
                    {
                        if (userCar.y < playFieldHeight - 1)
                        {
                            userCar.y = userCar.y + 1;
                        }
                    }
                    //speed up
                    else if (pressedKey.Key == ConsoleKey.Spacebar)
                    {
                        speed += 10;
                    }
                }

                #endregion

                //clear the console
                Console.Clear();

               
                
                foreach (Car car in cars)
                {
                    PrintOnPossition(car.x, car.y, car.c, car.color);
                }
                //when hit
                if (hitted)
                {
                    PrintOnPossition(userCar.x, userCar.y, 'X', ConsoleColor.Red);
                }
                else
                {
                    PrintOnPossition(userCar.x, userCar.y, userCar.c, userCar.color);
                }

                //print information
                PrintStringOnPossition(50, 20, "Lives: " + livesCount, ConsoleColor.Red);
                PrintStringOnPossition(50, 21, "Speed: " + speed, ConsoleColor.Blue);
                PrintStringOnPossition(50, 22, "Points: " + points, ConsoleColor.Yellow);
                //slow down program
                Thread.Sleep((int)(600 - speed)); // speed up the level according to speed

            }
        }
    }
}


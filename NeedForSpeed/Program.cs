using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;


namespace NeedForSpeed
{
   
    struct Car
    {
        public int x;
        public int y;
        public char c;
        public string s;
        public ConsoleColor color;
    }
    class Program
    {
        /// <summary>
        /// Print char on a selected position
        /// </summary>
        /// <param name="x">horizontal coordinate</param>
        /// <param name="y">vertical coordinate</param>
        /// <param name="c">symbol</param>
        /// <param name="color">color</param>
        static void PrintOnPossition(int x, int y,char c, ConsoleColor color = ConsoleColor.White)
         {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(c);
        }

        /// <summary>
        /// Print text on selected position
        /// </summary>
        /// <param name="x">horizontal coordinate</param>
        /// <param name="y">vertical coordinate</param>
        /// <param name="str">string we want to print</param>
        /// <param name="color">color of the string</param>
        static void PrintStringOnPossition(int x, int y, string str, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(str);
        }
        
        static void Main()
        {
            
           
            List<string> logoList = File.ReadAllLines("../../logo.txt").ToList();

            foreach (var line in logoList)
            {
                Console.WriteLine(line);
            }

            Console.ReadKey();
            Console.Clear();

            double speed = 100.0;
            int livesCount = 3;
            int points = 0;
            int counter = 0;
            // width of the play field 
            int playFieledWidth = 40;
            Console.BufferHeight = Console.WindowHeight = 50;
            // height of the play field
            int playFieldHeight = Console.WindowHeight; 
            //remove scroll of the console
            Console.BufferWidth = Console.WindowWidth = 70;
        
            //entering username
            StreamWriter fileName =new StreamWriter( @"../../log.txt",true);
            
            Console.WriteLine("Please Enter User Name:");
            string userName = Console.ReadLine();
            fileName.Write("{0} - ",userName);
#region music
            using (SoundPlayer music = new SoundPlayer("../../POL-stealth-mode-short.wav"))
            {
                music.PlayLooping();
            }
#endregion
            //user car
            Car userCar = new Car();
            userCar.x = (playFieldHeight / 2);
            userCar.y = Console.WindowHeight - 1;
            userCar.c = '▲';
            userCar.color = ConsoleColor.Red;

            Random randomGenerator = new Random(); //for the cars

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
                    //on every loop creates a new car
                    Car newCar = new Car();
                    newCar.color = ConsoleColor.White;
                    newCar.x = randomGenerator.Next(0, playFieledWidth);
                    newCar.y = 0;
                    newCar.c = '▼';
                    cars.Add(newCar);
                    counter++;
                }
                {
                    //for every 20 cars 1 live
                    if (counter >= 20) 
                    {
                        counter = 0;
                        Car liveCar = new Car();
                        liveCar.color = ConsoleColor.Red;
                        liveCar.x = randomGenerator.Next(0, playFieledWidth);
                        liveCar.y = 0;
                        liveCar.c = '♥';
                        cars.Add(liveCar);
                    }
                }
                //move cars
                List<Car> newCarsList = new List<Car>();

                for (int i = 0; i < cars.Count; i++)
                {
                    Car oldCar = cars[i];
                    Car newCar = new Car();
                    newCar.x = oldCar.x;
                    newCar.y = oldCar.y + 1;
                    newCar.c = oldCar.c;
                    newCar.color = oldCar.color;

                    //check for Lives
                    if (newCar.y == userCar.y && newCar.x == userCar.x && newCar.c == '♥')
                    {
                        livesCount++;
                        hitted = true;
                    }
                    //check if we are hit by car
                    else
                    {
                        if (newCar.y == userCar.y && newCar.x == userCar.x)
                        {
                            livesCount--;
                            hitted = true;
                  //game over
                            if (livesCount <= 0)
                            {
                                fileName.Write(points);
                                fileName.WriteLine();
                                fileName.Close();
                                PrintStringOnPossition(50, 25, "GAME OVER", ConsoleColor.Red);
                                PrintStringOnPossition(50, 29, "Press enter to exit", ConsoleColor.Red);
                                
//show info after game
#region InfoAfterGame
                                Console.Clear();
                                

                                List<string> info = File.ReadAllLines("../../log.txt").ToList();


                                Console.WriteLine("HIGH SCORE: ");

                                int line = 0;
                                foreach (var name in info)
                                {
                                    Console.WriteLine("{0}.{1}", line, name);
                                    line++;
                                }
                                
                                Console.ReadKey();
#endregion
                                Environment.Exit(0);// Exit
                            }
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
#region Movement
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true); 
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
                        speed += 15;
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

                //print additional information
                PrintStringOnPossition(50, 20, "Lives: " + livesCount, ConsoleColor.Red);
                PrintStringOnPossition(50, 21, "Speed: " + speed, ConsoleColor.Blue);
                PrintStringOnPossition(50, 22, "Points: " + points, ConsoleColor.Yellow);

                //slow down program
                Thread.Sleep((int)(600 - speed)); // speed up the level according to speed
            }
            }
        }
    }



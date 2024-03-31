using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dining_guys
{
    class Philosopher
    {
        int r;
        public Stopwatch timer { get; private set; }

        public string Name { get; set; }
        public Semaphore LeftFork { get; set; }
        public Semaphore RightFork { get; set; }

        Random rnd;
        int min;
        int max;
        int numberOfDishes = 20;
        public Philosopher(int min, int max, string name, Semaphore leftFork, Semaphore rightFork)
        {
            rnd = new Random();
            this.min = min;
            this.max = max;
            r = rnd.Next(min, max);
            Name = name;
            timer = Stopwatch.StartNew();
            timer.Start();
            LeftFork = leftFork;
            RightFork = rightFork;
        }
        void Eat()
        {
            Console.WriteLine($"{Name} is eating...");
            Thread.Sleep(rnd.Next(1000));
            timer.Restart();
        }
        void Think()
        {
            Console.WriteLine($"{Name} is thinking about those {numberOfDishes} plates of pasta left to eat...");
            Thread.Sleep(rnd.Next(max));
        }
        void ReleaseForks()
        {
            LeftFork.Release();
            RightFork.Release();
        }
        void GetLeftFork() { LeftFork.WaitOne(); }
        bool TryGetRightFork() { return RightFork.WaitOne((int)timer.ElapsedMilliseconds + r); }

        public void Dine()
        {
            while (numberOfDishes > 0)
            {
                Thread.Sleep(r);
                GetLeftFork();
                if (TryGetRightFork())
                {
                    Eat();
                    numberOfDishes--;
                    ReleaseForks();
                    Think();
                    timer.Restart();
                }
                else
                {
                    LeftFork.Release();
                    r = rnd.Next(min, max);
                }
            }
        } 
    }
}

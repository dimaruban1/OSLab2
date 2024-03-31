using System.Diagnostics;
using System.Runtime.InteropServices;
using dining_guys;


const int numPhilosophers = 999;
Stopwatch t = new Stopwatch();
t.Start();
// Initialize semaphores representing forks
Semaphore[] forks = new Semaphore[numPhilosophers];
for (int i = 0; i < numPhilosophers; i++)
{
    forks[i] = new Semaphore(1, 1); // Initialize each fork with count 1
}

// Create philosophers and start their dining process
Philosopher[] philosophers = new Philosopher[numPhilosophers];
Thread[] philosopherThreads = new Thread[numPhilosophers];
for (int i = 0; i < numPhilosophers; i++)
{
    Semaphore leftFork = forks[i];
    Semaphore rightFork = forks[(i + 1) % numPhilosophers]; // Circular arrangement of forks
    philosophers[i] = new Philosopher(0, 10, "philosopher " + i + " ", leftFork, rightFork);
    Thread philosopherThread = new Thread(philosophers[i].Dine);
    philosopherThreads[i] = philosopherThread;
    philosopherThread.Start();
}

// Wait for all philosopher threads to finish (this won't happen in this infinite loop)
foreach (Thread philosopherThread in philosopherThreads)
{
    philosopherThread.Join();
}
Console.WriteLine(t.Elapsed.ToString());

using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("🧪 Ejecutando ejemplos de Monitor en C#...");

        Console.WriteLine("----------Ejemplo 1----------");
        new Thread(MonitorExamples.SeccionCritica).Start();
        Thread.Sleep(300);

        Console.WriteLine("----------Ejemplo 2----------");
        new Thread(MonitorExamples.IntentarConTimeout).Start();
        Thread.Sleep(300);

        Console.WriteLine("----------Ejemplo 3----------");
        new Thread(() => MonitorExamples.Productor(42)).Start();
        Thread.Sleep(100);
        new Thread(MonitorExamples.Consumidor).Start();
        Thread.Sleep(500);

        Console.WriteLine("----------Ejemplo 4----------");
        new Thread(MonitorExamples.Transferencia).Start();
        Thread.Sleep(500);

        Console.WriteLine("----------Ejemplo 5----------");
        new Thread(MonitorExamples.Esperar).Start();
        Thread.Sleep(300);
        new Thread(MonitorExamples.Señalar).Start();
        Thread.Sleep(500);

        Console.WriteLine("----------Ejemplo 6----------");
        for (int i = 0; i < 5; i++)
        {
            string user = $"User{i}";
            new Thread(() => MonitorExamples.AccesoLimitado(user)).Start();
        }
        Thread.Sleep(2000);

        Console.WriteLine("----------Ejemplo 7----------");
        new Thread(MonitorExamples.HiloA).Start();
        Thread.Sleep(100);
        new Thread(MonitorExamples.HiloB).Start();
        Thread.Sleep(500);

        Console.WriteLine("----------Ejemplo 8----------");
        for (int i = 0; i < 3; i++)
        {
            string h = $"Hilo{i}";
            new Thread(() => MonitorExamples.Buscar(h)).Start();
        }
        Thread.Sleep(1000);

        Console.WriteLine("----------Ejemplo 9----------");
        for (int i = 0; i < 5; i++)
            new Thread(MonitorExamples.Contar).Start();
        Thread.Sleep(500);

        Console.WriteLine("----------Ejemplo 10----------");
        new Thread(() => MonitorExamples.AccederConTimeout("HiloX")).Start();

        Thread.Sleep(1000);
        Console.WriteLine("✅ Fin de los ejemplos.");
    }
}

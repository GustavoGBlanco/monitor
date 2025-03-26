
using System;
using System.Collections.Generic;
using System.Threading;

public static class MonitorExamples
{
    private static readonly object _lock = new();
    private static readonly object _lock1 = new();
    private static readonly object _lock2 = new();
    private static Queue<int> _cola = new();
    private static bool _listo = false;
    private static int _enUso = 0;
    private static readonly int _max = 3;
    private static bool _turnoA = true;
    private static bool _encontrado = false;
    private static int _contador = 0;

    public static void SeccionCritica()
    {
        Monitor.Enter(_lock);
        try { Console.WriteLine("Ejecutando sección crítica con Monitor."); }
        finally { Monitor.Exit(_lock); }
    }

    public static void IntentarConTimeout()
    {
        if (Monitor.TryEnter(_lock, TimeSpan.FromMilliseconds(300)))
        {
            try { Console.WriteLine("Acceso concedido."); }
            finally { Monitor.Exit(_lock); }
        }
        else Console.WriteLine("Timeout: no se pudo obtener el lock.");
    }

    public static void Productor(int valor)
    {
        Monitor.Enter(_lock);
        try
        {
            _cola.Enqueue(valor);
            Console.WriteLine($"Producido: {valor}");
            Monitor.Pulse(_lock);
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void Consumidor()
    {
        Monitor.Enter(_lock);
        try
        {
            while (_cola.Count == 0)
            {
                Console.WriteLine("Consumidor esperando...");
                Monitor.Wait(_lock);
            }
            int valor = _cola.Dequeue();
            Console.WriteLine($"Consumido: {valor}");
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void Transferencia()
    {
        Monitor.Enter(_lock1);
        try
        {
            Thread.Sleep(100);
            Monitor.Enter(_lock2);
            try { Console.WriteLine("Transferencia realizada con ambos recursos."); }
            finally { Monitor.Exit(_lock2); }
        }
        finally { Monitor.Exit(_lock1); }
    }

    public static void Esperar()
    {
        Monitor.Enter(_lock);
        try
        {
            while (!_listo)
            {
                Console.WriteLine("Esperando señal...");
                Monitor.Wait(_lock);
            }
            Console.WriteLine("¡Listo para continuar!");
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void Señalar()
    {
        Monitor.Enter(_lock);
        try
        {
            _listo = true;
            Monitor.PulseAll(_lock);
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void AccesoLimitado(string usuario)
    {
        Monitor.Enter(_lock);
        try
        {
            while (_enUso >= _max)
            {
                Console.WriteLine($"{usuario} esperando turno...");
                Monitor.Wait(_lock);
            }
            _enUso++;
        }
        finally { Monitor.Exit(_lock); }

        Console.WriteLine($"{usuario} accediendo al recurso.");
        Thread.Sleep(500);

        Monitor.Enter(_lock);
        try
        {
            _enUso--;
            Monitor.Pulse(_lock);
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void HiloA()
    {
        Monitor.Enter(_lock);
        try
        {
            while (!_turnoA) Monitor.Wait(_lock);
            Console.WriteLine("Hilo A ejecutando...");
            _turnoA = false;
            Monitor.Pulse(_lock);
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void HiloB()
    {
        Monitor.Enter(_lock);
        try
        {
            while (_turnoA) Monitor.Wait(_lock);
            Console.WriteLine("Hilo B ejecutando...");
            _turnoA = true;
            Monitor.Pulse(_lock);
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void Buscar(string hilo)
    {
        Monitor.Enter(_lock);
        try
        {
            if (_encontrado)
            {
                Console.WriteLine($"{hilo} aborta búsqueda.");
                return;
            }

            Console.WriteLine($"{hilo} buscando...");
            Thread.Sleep(200);

            if (!_encontrado)
            {
                _encontrado = true;
                Console.WriteLine($"{hilo} encontró el resultado.");
            }
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void Contar()
    {
        Monitor.Enter(_lock);
        try
        {
            _contador++;
            if (_contador % 3 == 0)
                Console.WriteLine($"Contador alcanzó múltiplo de 3: {_contador}");
        }
        finally { Monitor.Exit(_lock); }
    }

    public static void AccederConTimeout(string nombre)
    {
        if (Monitor.TryEnter(_lock, 300))
        {
            try
            {
                Console.WriteLine($"{nombre} accedió al recurso.");
                Thread.Sleep(500);
            }
            finally { Monitor.Exit(_lock); }
        }
        else Console.WriteLine($"{nombre} no pudo acceder (timeout).");
    }
}

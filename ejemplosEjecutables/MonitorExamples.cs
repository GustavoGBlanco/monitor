
using System;
using System.Collections.Generic;
using System.Threading;

public static class SeccionCritica
{
    private static object _lock = new();
    public static string Mensaje()
    {
        Monitor.Enter(_lock);
        try { return "Sección crítica ejecutada con Monitor.Enter/Exit"; }
        finally { Monitor.Exit(_lock); }
    }
}

public static class ContadorSeguro
{
    private static object _lock = new();
    public static int Incrementar(int contador)
    {
        Monitor.Enter(_lock);
        try { return ++contador; }
        finally { Monitor.Exit(_lock); }
    }
}

public static class IntentoConTimeout
{
    private static object _lock = new();
    public static string Intentar()
    {
        if (Monitor.TryEnter(_lock, TimeSpan.FromMilliseconds(500)))
        {
            try { return "Entró a la sección crítica"; }
            finally { Monitor.Exit(_lock); }
        }
        else return "Timeout: no se pudo entrar";
    }
}

public static class ContadorCompartido
{
    private static object _lock = new();
    private static int _contador = 0;

    public static void Incrementar()
    {
        Monitor.Enter(_lock);
        try { _contador++; }
        finally { Monitor.Exit(_lock); }
    }

    public static int Obtener()
    {
        Monitor.Enter(_lock);
        try { return _contador; }
        finally { Monitor.Exit(_lock); }
    }
}

public static class ListaCompartida
{
    private static object _lock = new();
    private static List<string> _mensajes = new();

    public static void Agregar(string mensaje)
    {
        Monitor.Enter(_lock);
        try { _mensajes.Add(mensaje); }
        finally { Monitor.Exit(_lock); }
    }

    public static string ObtenerMensajes()
    {
        Monitor.Enter(_lock);
        try { return string.Join(", ", _mensajes); }
        finally { Monitor.Exit(_lock); }
    }
}

public static class CuentaBancaria
{
    private static object _lock = new();
    private static int _saldo = 1000;

    public static string Retirar(int monto)
    {
        Monitor.Enter(_lock);
        try
        {
            if (_saldo >= monto)
            {
                _saldo -= monto;
                return $"Retirado {monto}. Saldo restante: {_saldo}";
            }
            else return "Fondos insuficientes";
        }
        finally { Monitor.Exit(_lock); }
    }
}

public static class RecursosAnidados
{
    private static object _lockA = new();
    private static object _lockB = new();

    public static string Transferir()
    {
        Monitor.Enter(_lockA);
        try
        {
            Monitor.Enter(_lockB);
            try { return "Transferencia completada con éxito"; }
            finally { Monitor.Exit(_lockB); }
        }
        finally { Monitor.Exit(_lockA); }
    }
}

public static class SincronizacionCondicional
{
    private static object _lock = new();
    private static bool _listo = false;

    public static string Esperar()
    {
        Monitor.Enter(_lock);
        try
        {
            while (!_listo) Monitor.Wait(_lock);
            return "Continuando luego de señal";
        }
        finally { Monitor.Exit(_lock); }
    }

    public static string Señalar()
    {
        Monitor.Enter(_lock);
        try
        {
            _listo = true;
            Monitor.Pulse(_lock);
            return "Señal enviado correctamente";
        }
        finally { Monitor.Exit(_lock); }
    }
}

public static class ProductorConsumidor
{
    private static object _lock = new();
    private static Queue<int> _cola = new();

    public static string Producir(int valor)
    {
        Monitor.Enter(_lock);
        try
        {
            _cola.Enqueue(valor);
            Monitor.Pulse(_lock);
            return $"Producido: {valor}";
        }
        finally { Monitor.Exit(_lock); }
    }

    public static string Consumir()
    {
        Monitor.Enter(_lock);
        try
        {
            while (_cola.Count == 0) Monitor.Wait(_lock);
            int val = _cola.Dequeue();
            return $"Consumido: {val}";
        }
        finally { Monitor.Exit(_lock); }
    }
}

public static class ControlDeStock
{
    private static object _lock = new();
    private static int _stock = 5;

    public static string Comprar(string usuario)
    {
        Monitor.Enter(_lock);
        try
        {
            if (_stock > 0)
            {
                _stock--;
                return $"{usuario} compró. Stock restante: {_stock}";
            }
            else return $"{usuario} no pudo comprar. Sin stock.";
        }
        finally { Monitor.Exit(_lock); }
    }
}

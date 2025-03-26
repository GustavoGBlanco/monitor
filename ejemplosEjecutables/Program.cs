
using System;
using System.Threading;

class MonitorExamplesApp
{
    static void Main()
    {
        Console.WriteLine("----------Ejemplo 1: Sección crítica----------");
        Console.WriteLine(SeccionCritica.Mensaje());
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 2: Contador seguro----------");
        int contador = 0;
        contador = ContadorSeguro.Incrementar(contador);
        Console.WriteLine("Contador = " + contador);
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 3: TryEnter con timeout----------");
        Console.WriteLine(IntentoConTimeout.Intentar());
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 4: Contador compartido----------");
        for (int i = 0; i < 1000; i++) new Thread(ContadorCompartido.Incrementar).Start();
        Thread.Sleep(500);
        Console.WriteLine("Contador total: " + ContadorCompartido.Obtener());
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 5: Lista compartida----------");
        ListaCompartida.Agregar("Hola");
        ListaCompartida.Agregar("Monitor");
        Console.WriteLine("Mensajes: " + ListaCompartida.ObtenerMensajes());
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 6: Cuenta bancaria----------");
        Console.WriteLine(CuentaBancaria.Retirar(200));
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 7: Recursos anidados----------");
        Console.WriteLine(RecursosAnidados.Transferir());
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 8: Espera condicional----------");
        Thread t1 = new Thread(() =>
        {
            Console.WriteLine("Esperar(): " + SincronizacionCondicional.Esperar());
        });
        t1.Start();

        Thread.Sleep(500);
        Console.WriteLine("Señalar(): " + SincronizacionCondicional.Señalar());
        t1.Join();
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 9: Productor/Consumidor----------");
        Thread t2 = new Thread(() =>
        {
            Console.WriteLine("Consumidor: " + ProductorConsumidor.Consumir());
        });
        t2.Start();

        Thread.Sleep(500);
        Console.WriteLine("Productor: " + ProductorConsumidor.Producir(123));
        t2.Join();
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 10: Control de stock----------");
        for (int i = 1; i <= 7; i++)
        {
            string usuario = $"Usuario{i}";
            Console.WriteLine(ControlDeStock.Comprar(usuario));
        }
        Console.WriteLine();
    }
}

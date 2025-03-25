# Módulo 2: `Monitor`, `Wait()` y `Pulse()` en C#

## 🔍 ¿Qué es `Monitor`?
`Monitor` es una clase que permite:
- Bloquear una sección crítica manualmente (`Enter` / `Exit`)
- Coordinar la ejecución entre hilos con `Wait()` y `Pulse()`

Es el mecanismo subyacente de `lock`, pero con **mayor control**.

---

## 🏠 Escenario práctico: **Productor - Consumidor**

Simulamos un sistema donde un hilo "productor" agrega tareas a una cola compartida, y otro hilo "consumidor" las procesa. Coordinamos el acceso usando `Monitor.Wait()` y `Monitor.Pulse()`.

### Archivos

#### `TaskQueue.cs`
```csharp
using System;
using System.Collections.Generic;

public class TaskQueue
{
    private readonly Queue<string> _queue = new();
    private readonly object _lock = new();

    public void Enqueue(string task)
    {
        lock (_lock)
        {
            _queue.Enqueue(task);
            Console.WriteLine($"[+] Producido: {task}");
            Monitor.Pulse(_lock); // Notifica a un consumidor
        }
    }

    public string Dequeue()
    {
        lock (_lock)
        {
            while (_queue.Count == 0)
            {
                Console.WriteLine("[ ] Esperando tareas...");
                Monitor.Wait(_lock);
            }

            string task = _queue.Dequeue();
            Console.WriteLine($"[-] Consumido: {task}");
            return task;
        }
    }
}
```

#### `Program.cs`
```csharp
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        var queue = new TaskQueue();

        Thread productor = new(() => {
            for (int i = 1; i <= 5; i++)
            {
                Thread.Sleep(500);
                queue.Enqueue($"Tarea {i}");
            }
        });

        Thread consumidor = new(() => {
            for (int i = 1; i <= 5; i++)
            {
                queue.Dequeue();
                Thread.Sleep(800);
            }
        });

        consumidor.Start();
        productor.Start();

        productor.Join();
        consumidor.Join();
    }
}
```

---

## 🤔 ¿Por qué usar `Monitor` y no `lock`?

`lock` **solo permite bloquear** una sección. En cambio, `Monitor` permite:
- Esperar eventos: `Monitor.Wait()`
- Notificar hilos: `Monitor.Pulse()` o `Monitor.PulseAll()`

Este mecanismo es vital para **coordinar hilos que dependen entre sí**.

---

## 🔢 Detalles importantes

- `Monitor.Wait(obj)` libera el lock y pone en espera al hilo.
- Cuando otro hilo hace `Monitor.Pulse(obj)`, uno de los hilos en espera es notificado.
- Usá `while` al esperar para evitar "despertares espurios".
- `Pulse()` notifica un solo hilo; `PulseAll()` a todos.

---

## 🧼 Buenas prácticas con `Monitor`

| Regla | Motivo |
|-------|--------|
| Siempre usar `Monitor.Wait()` dentro de un `while` | Asegura condiciones correctas al despertar |
| Usá `try/finally` si usás `Monitor.Enter()` manualmente | Evita olvidar `Exit()` en errores |
| Mantené el diseño simple | Evitá deadlocks y código confuso |

---

✅ Este fue el Módulo 2. En el siguiente veremos `Mutex` con un ejemplo entre procesos o hilos.

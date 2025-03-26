# Ejemplos prácticos y profesionales de `Monitor` en C#

Este documento presenta 10 ejemplos realistas y técnicamente justificados del uso de `Monitor` en C#, todos diseñados con hilos (`Thread`) para ilustrar cómo `Monitor` permite un control fino sobre la sincronización, incluyendo `Enter`, `Exit`, `TryEnter`, `Wait` y `Pulse`.

---

## 🧪 Ejemplo 1: Sección crítica con `Monitor.Enter` y `Exit`

```csharp
private static readonly object _lock = new();

public static void SeccionCritica()
{
    Monitor.Enter(_lock);
    try
    {
        Console.WriteLine("Ejecutando sección crítica con Monitor.");
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Uso básico de `Monitor` para proteger una sección crítica.

✅ **¿Por qué `Monitor`?**  
Proporciona control explícito sobre la entrada y salida del lock. Útil cuando se necesita flexibilidad adicional.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: más conciso, pero menos flexible.
- 🧵 `Mutex`: innecesario para sincronización dentro del mismo proceso.
- 🔄 `Barrier`: no aplica, no hay fases de sincronización.
- 📉 `Semaphore(Slim)`: más útil para acceso concurrente parcial.

---

## 🧪 Ejemplo 2: Uso de `Monitor.TryEnter` con timeout

```csharp
private static readonly object _lock = new();

public static void IntentarConTimeout()
{
    if (Monitor.TryEnter(_lock, TimeSpan.FromMilliseconds(300)))
    {
        try
        {
            Console.WriteLine("Acceso concedido.");
        }
        finally
        {
            Monitor.Exit(_lock);
        }
    }
    else
    {
        Console.WriteLine("Timeout: no se pudo obtener el lock.");
    }
}
```

🔍 Permite evitar bloqueos indefinidos.

✅ **¿Por qué `Monitor`?**  
`TryEnter` permite controlar cuánto tiempo se espera. `lock` no lo permite.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: no permite timeout.
- 🧵 `Mutex`: también permite timeout, pero más costoso.
- 🔄 `Barrier`: no aplica.
- 📉 `Semaphore(Slim)`: buena alternativa si hay concurrencia permitida.

---

## 🧪 Ejemplo 3: Productor-consumidor con `Monitor.Wait` y `Pulse`

```csharp
private static readonly object _lock = new();
private static Queue<int> _cola = new();

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
    finally
    {
        Monitor.Exit(_lock);
    }
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
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Coordina hilos que esperan y notifican con una cola compartida.

✅ **¿Por qué `Monitor`?**  
Es el único mecanismo en C# con `Wait`/`Pulse`.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: no soporta `Wait` ni `Pulse`.
- 🧵 `Mutex`: no tiene notificación.
- 🔄 `Barrier`: no adecuado para este patrón.
- 📉 `Semaphore(Slim)`: puede modelar esto, pero no con notificación por condiciones.

---

## 🧪 Ejemplo 4: Transferencia entre recursos sincronizados

```csharp
private static readonly object _lock1 = new();
private static readonly object _lock2 = new();

public static void Transferencia()
{
    Monitor.Enter(_lock1);
    try
    {
        Thread.Sleep(100); // Simula trabajo previo
        Monitor.Enter(_lock2);
        try
        {
            Console.WriteLine("Transferencia realizada con ambos recursos.");
        }
        finally
        {
            Monitor.Exit(_lock2);
        }
    }
    finally
    {
        Monitor.Exit(_lock1);
    }
}
```

🔍 Requiere adquirir dos recursos protegidos al mismo tiempo.

✅ **¿Por qué `Monitor`?**  
Permite realizar control manual de adquisición de múltiples locks.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: más limpio, pero menos flexible.
- 🧵 `Mutex`: igual, pero interprocesos.
- 🔄 `Barrier`: no aplica.
- 📉 `Semaphore(Slim)`: no gestiona orden ni composición de locks.

---

## 🧪 Ejemplo 5: Espera condicional (habilitar cuando listo)

```csharp
private static readonly object _lock = new();
private static bool _listo = false;

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
    finally
    {
        Monitor.Exit(_lock);
    }
}

public static void Señalar()
{
    Monitor.Enter(_lock);
    try
    {
        _listo = true;
        Monitor.PulseAll(_lock);
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Permite iniciar ejecución solo cuando una condición se activa.

✅ **¿Por qué `Monitor`?**  
Permite bloquear y despertar múltiples hilos por estado compartido.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: no puede esperar condicionalmente.
- 🧵 `AutoResetEvent`, `ManualResetEvent`: alternativa viable si se requiere más control externo.
- 🔄 `Barrier`: sincronización por fases, no por condición.
- 📉 `Semaphore(Slim)`: no maneja condiciones de espera internas.

---

## 🧪 Ejemplo 6: Límite de acceso con contador (tipo semáforo)

```csharp
private static readonly object _lock = new();
private static int _enUso = 0;
private static readonly int _max = 3;

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
    finally
    {
        Monitor.Exit(_lock);
    }

    Console.WriteLine($"{usuario} accediendo al recurso.");
    Thread.Sleep(500);

    Monitor.Enter(_lock);
    try
    {
        _enUso--;
        Monitor.Pulse(_lock);
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Controla cuántos hilos pueden entrar simultáneamente a una sección.

✅ **¿Por qué `Monitor`?**  
Permite una implementación tipo semáforo personalizada con lógica propia.

📊 **Comparación con otros mecanismos:**
- 📉 `SemaphoreSlim`: más directo, pero menos flexible si hay lógica adicional.
- 🔐 `lock`: no se puede usar para contar o esperar por condiciones.
- 🔄 `Barrier`: no adecuado aquí.

---

## 🧪 Ejemplo 7: Coordinación alterna entre hilos (ping-pong)

```csharp
private static readonly object _lock = new();
private static bool _turnoA = true;

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
    finally
    {
        Monitor.Exit(_lock);
    }
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
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Controla el orden entre hilos.

✅ **¿Por qué `Monitor`?**  
Ideal para controlar turnos entre hilos. `lock` no permite esto.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: no se puede usar para contar o esperar por condiciones.
- 🧵 `Mutex`: no soporta notificación entre hilos (como Pulse), solo bloqueo.
- 📉 `SemaphoreSlim`: permitiría limitar acceso, pero no gestionar turnos ni alternancia.
- 🔄 `Barrier`: no aplica, ya que no hay fases sincronizadas entre hilos.

---

## 🧪 Ejemplo 8: Búsqueda en paralelo con cancelación

```csharp
private static readonly object _lock = new();
private static bool _encontrado = false;

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
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Solo un hilo completa una tarea; los otros salen.

✅ **¿Por qué `Monitor`?**  
Permite lógica condicional con salida segura de hilos.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: permitiría exclusión mutua, pero no controlar múltiples condiciones como "ya encontrado".
- 🧵 `Mutex`: más costoso y sin beneficios en un entorno de un solo proceso.
- 🔄 `Barrier`: no aplica, no hay fases compartidas.
- 📉 `Semaphore(Slim)`: no resuelve cancelación basada en lógica compartida.

---

## 🧪 Ejemplo 9: Contador extendido con condición personalizada

```csharp
private static readonly object _lock = new();
private static int _contador = 0;

public static void Contar()
{
    Monitor.Enter(_lock);
    try
    {
        _contador++;
        if (_contador % 3 == 0)
            Console.WriteLine($"Contador alcanzó múltiplo de 3: {_contador}");
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Se puede extender con reglas de negocio más complejas.

✅ **¿Por qué `Monitor`?**  
`Interlocked` no permite lógica extendida. `lock` sería equivalente, pero `Monitor` es útil si más adelante se quiere integrar `Wait/Pulse`.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: funcionaría igual para este caso si solo incrementamos, pero no es extensible si luego se requiere Wait/Pulse.
- 🧵 `Mutex`: no aporta valor agregado, siendo más costoso.
- 🔄 `Barrier`: no aplica en absoluto, ya que no hay sincronización por barrera ni fases.
- 📉 `Semaphore(Slim)`: útil si quisiéramos limitar acceso concurrente, pero aquí el acceso es exclusivo.

---

## 🧪 Ejemplo 10: Acceso exclusivo con timeout simulado

```csharp
private static readonly object _lock = new();

public static void AccederConTimeout(string nombre)
{
    if (Monitor.TryEnter(_lock, 300))
    {
        try
        {
            Console.WriteLine($"{nombre} accedió al recurso.");
            Thread.Sleep(500);
        }
        finally
        {
            Monitor.Exit(_lock);
        }
    }
    else
    {
        Console.WriteLine($"{nombre} no pudo acceder (timeout).");
    }
}
```

🔍 Simula un recurso costoso al que se intenta acceder con tiempo limitado.

✅ **¿Por qué `Monitor`?**  
Proporciona control total sobre el acceso a la sección crítica.

📊 **Comparación con otros mecanismos:**
- 🔐 `lock`: no soporta timeout, por lo que el hilo se bloquearía indefinidamente.
- 🧵 `Mutex`: alternativa válida si se requiere sincronización entre procesos, pero más lento.
- 🔄 `Barrier`: no corresponde en este patrón.
- 📉 `Semaphore(Slim)`: puede ser usado con timeout, pero permite múltiples accesos. Monitor es mejor si se necesita exclusión total y control explícito.

---
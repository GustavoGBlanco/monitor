# Ejemplos de `Monitor` en C# (detalle por ejemplo)

Este documento explica en profundidad cada uno de los 10 ejemplos de uso de `Monitor` en C#, incluyendo su propósito, ejecución y por qué `Monitor` es la mejor opción para ese caso. Todos los ejemplos fueron organizados y adaptados para poder ejecutarse fácilmente en un entorno multihilo.

---

## 🧪 Ejemplo 1: Uso básico de Monitor.Enter/Exit

```csharp
private static object _lock = new();

public static string Mensaje()
{
    Monitor.Enter(_lock);
    try
    {
        return "Sección crítica ejecutada con Monitor.Enter/Exit";
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Marca la forma básica de sincronización usando `Monitor`.

✅ **¿Por qué `Monitor`?**  
Permite control explícito de adquisición y liberación de locks.

---

## 🧪 Ejemplo 2: Uso con try/finally seguro

```csharp
private static object _lock = new();

public static int Incrementar(int contador)
{
    Monitor.Enter(_lock);
    try
    {
        contador++;
        return contador;
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Incrementa una variable en forma segura.

✅ **¿Por qué `Monitor`?**  
Ideal para flujos donde la liberación debe ser garantizada manualmente.

---

## 🧪 Ejemplo 3: TryEnter con timeout

```csharp
private static object _lock = new();

public static string Intentar()
{
    if (Monitor.TryEnter(_lock, TimeSpan.FromMilliseconds(500)))
    {
        try { return "Entró a la sección crítica"; }
        finally { Monitor.Exit(_lock); }
    }
    else
    {
        return "Timeout: no se pudo entrar";
    }
}
```

🔍 Intenta entrar a la sección crítica sin bloquear indefinidamente.

✅ **¿Por qué `Monitor`?**  
Ofrece `TryEnter`, lo cual `lock` no tiene.

---

## 🧪 Ejemplo 4: Protección de contador compartido

```csharp
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
```

🔍 Incrementa y expone el contador protegido.

✅ **¿Por qué `Monitor`?**  
Más flexible para código donde se requiera lógica adicional.

---

## 🧪 Ejemplo 5: Acceso a lista compartida

```csharp
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
```

🔍 Permite observar el contenido de una lista protegida.

✅ **¿Por qué `Monitor`?**  
Flexible, ideal para estructuras más complejas.

---

## 🧪 Ejemplo 6: Lógica condicional protegida

```csharp
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
        else
        {
            return "Fondos insuficientes";
        }
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Ejecuta lógica bajo condiciones sincronizadas.

✅ **¿Por qué `Monitor`?**  
Permite controlar bloques más complejos y reusables.

---

## 🧪 Ejemplo 7: Locks anidados con Monitor

```csharp
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
    finally
    {
        Monitor.Exit(_lockA);
    }
}
```

🔍 Asegura operaciones compuestas.

✅ **¿Por qué `Monitor`?**  
Ideal cuando se deben adquirir múltiples recursos en orden.

---

## 🧪 Ejemplo 8: Wait y Pulse

```csharp
private static object _lock = new();
private static bool _listo = false;

public static string Esperar()
{
    Monitor.Enter(_lock);
    try
    {
        while (!_listo)
        {
            Monitor.Wait(_lock);
        }
        return "Continuando luego de señal";
    }
    finally
    {
        Monitor.Exit(_lock);
    }
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
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Muestra sincronización por condiciones.

✅ **¿Por qué `Monitor`?**  
Único mecanismo que soporta `Wait` y `Pulse`.

---

## 🧪 Ejemplo 9: Productor-consumidor simple

```csharp
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
    finally
    {
        Monitor.Exit(_lock);
    }
}

public static string Consumir()
{
    Monitor.Enter(_lock);
    try
    {
        while (_cola.Count == 0)
        {
            Monitor.Wait(_lock);
        }
        int val = _cola.Dequeue();
        return $"Consumido: {val}";
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Establece flujo entre productor y consumidor.

✅ **¿Por qué `Monitor`?**  
Permite coordinar sin necesidad de estructuras externas.

---

## 🧪 Ejemplo 10: Control de stock con condición

```csharp
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
        else
        {
            return $"{usuario} no pudo comprar. Sin stock.";
        }
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

🔍 Controla acceso a recurso limitado en contexto concurrente.

✅ **¿Por qué `Monitor`?**  
Ideal para lógica crítica con condiciones.

---
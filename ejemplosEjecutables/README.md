# 🚀 Instrucciones para ejecutar este proyecto en Visual Studio Code o Visual Studio

Este proyecto fue desarrollado con .NET 9 y contiene múltiples ejemplos prácticos del uso de `monitor` para sincronización multihilo. A continuación, se detallan los pasos para compilar y ejecutar correctamente la solución.

---

## 🧱 Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) instalado
- Visual Studio Code **o** Visual Studio 2022/2025
- Proyecto con `<TargetFramework>net9.0</TargetFramework>`

---

## ▶️ Usando Visual Studio Code

1. **Abrí la carpeta del proyecto**  
   Que contenga:
   - `MonitorExamples.csproj`
   - `Program.cs`
   - `MonitorExamples.cs`

2. **Verificá que el `.csproj` apunte a .NET 9**  
   ```xml
   <TargetFramework>net9.0</TargetFramework>
   ```

3. **Instalá la extensión de C#**  
   En VS Code buscá:  
   `C# for Visual Studio Code (OmniSharp)`

4. **Abrí la terminal y ejecutá el programa**  
   ```bash
   dotnet run
   ```

5. ✅ Verás en la consola los resultados de los 10 ejemplos ejecutados.

---

## ▶️ Usando Visual Studio (Windows)

1. **Abrí Visual Studio**  
   Seleccioná *“Abrir un proyecto o solución”* y abrí `MonitorExamples.csproj`.

2. **Verificá que esté usando .NET 9**  
   En *Propiedades del proyecto* → *Aplicación* → *Marco de destino*: `.NET 9`

3. **Ejecutá el proyecto**  
   Hacé clic en **Iniciar sin depurar** (`Ctrl + F5`).

4. ✅ Visual Studio abrirá una consola con la salida completa del programa.

---

## 🧼 Archivos importantes

- `Program.cs`: ejecuta todos los ejemplos.
- `MonitorExamples.cs`: contiene la lógica de sincronización.
- `log.txt`: generado automáticamente por el ejemplo 8.

---

## 📁 `.gitignore` sugerido

```
bin/
obj/
.vscode/
*.user
*.suo
*.log
log.txt
```

---


---

## 🖼 Captura de pantalla de resultados

A continuación se incluye una captura de pantalla con los resultados en consola al ejecutar el proyecto. Esto es útil para visualizar rápidamente el comportamiento de los hilos y la sincronización con `monitor`.

### 📸:

> ![Resultados en consola](/assets/screenshot/console_output.png)

---
# Sistema Hospital

Sistema de gestión hospitalaria desarrollado en **C# (.NET)** con arquitectura en capas, aplicando principios de **Programación Orientada a Objetos**: herencia, encapsulamiento, polimorfismo e interfaces. La persistencia de datos se maneja con archivos **CSV** usando la librería **CsvHelper**.
---

## Descripción general

El sistema permite gestionar las operaciones básicas de un hospital:

- Registrar y administrar **pacientes** con su historial médico integrado.
- Registrar y administrar **doctores**.
- Agendar **citas** entre pacientes y doctores.
- Gestionar **hospitalizaciones**, asignando habitaciones disponibles automáticamente y asignando un doctor supervisor.
- Consultar y actualizar el **historial médico** de cada paciente (diagnósticos, tratamientos y observaciones).

Todas las operaciones (crear, listar, actualizar, eliminar) están disponibles desde un menú de consola interactivo.

---

## Estructura del proyecto

```
SistemaHospital.slnx
│
├── SistemaHospital/                  ← Biblioteca de clases
│   ├── Interfaces/
│   │   └── IAsignacionHabitacion.cs
│   ├── Models/
│   │   ├── Persona.cs
│   │   ├── Paciente.cs
│   │   ├── Doctor.cs
│   │   ├── HistorialMedico.cs
│   │   ├── Cita.cs
│   │   ├── Hospitalizacion.cs
│   │   ├── Habitacion.cs
│   │   └── Hospital.cs
│   └── Repositories/
│       ├── CsvRepository.cs
│       ├── PacienteHistorialRepository.cs
│       ├── DoctorRepository.cs
│       ├── CitaRepository.cs
│       └── HospitalizacionRepository.cs
│
└── AppConsole/                       ← Proyecto de consola
    ├── Program.cs
    ├── UIPaciente.cs
    ├── UIDoctor.cs
    ├── UICita.cs
    ├── UIHospitalizacion.cs
    └── UIHistorial.cs
```

---

## Arquitectura y capas

El proyecto sigue una arquitectura de **3 capas**:

```
AppConsole (Presentación)
       │
       ▼
SistemaHospital/Repositories (Acceso a datos)
       │
       ▼
SistemaHospital/Models (Dominio)
```

| Capa | Proyecto | Responsabilidad |
|---|---|---|
| Presentación | `AppConsole` | Capturar input del usuario y mostrar resultados |
| Acceso a datos | `SistemaHospital/Repositories` | Leer y escribir CSV, lógica de persistencia |
| Dominio | `SistemaHospital/Models` | Representar las entidades del sistema |

---

## Modelos del dominio

### `Persona` (clase base)
Clase padre de `Paciente` y `Doctor`. Contiene los atributos comunes:
- `Nombre`, `Identificacion`, `Edad`, `Telefono`

### `Paciente` — hereda de `Persona`
Representa a un paciente del hospital. Lleva su historial médico integrado como propiedad.
- `FechaIngreso: DateTime`
- `Historial: HistorialMedico`

### `Doctor` — hereda de `Persona`
Representa a un médico del hospital.
- `Especialidad: string`
- `LicenciaMedica: int`

### `HistorialMedico`
Pertenece a un `Paciente`. No existe de forma independiente.
- `Diagnosticos: List<string>`
- `Tratamientos: List<string>`
- `Observaciones: List<string>`

### `Cita`
Registra una cita entre un paciente y un doctor. Almacena IDs en lugar de objetos completos para facilitar la serialización CSV.
- `Id`, `FechaCita`, `IdPaciente`, `IdDoctor`, `Motivo`

### `Hospitalizacion`
Registra la internación de un paciente en una habitación bajo supervisión de un doctor.
- `Id`, `IdPaciente`, `IdDoctor`, `NumeroHabitacion`
- `FechaIngreso`, `FechaAlta?`
- `EstaActiva: bool` — propiedad calculada

### `Habitacion`
Representa una habitación física del hospital.
- `NumeroHabitacion: int`
- `Ocupada: bool`

### `Hospital`
Clase de coordinación que agrupa doctores, pacientes, habitaciones y hospitalizaciones.
- Composición con `Habitacion` (las crea al inicializarse)
- Agregación con `Doctor` y `Paciente`
- Método `RegistrarHospitalizacion()`

---

## Interfaces

### `IAsignacionHabitacion`
Define el contrato para la asignación de habitaciones. Implementada por `HospitalizacionRepository`.

```csharp
public interface IAsignacionHabitacion
{
    int ObtenerHabitacionDisponible();
    bool HabitacionDisponible(int numeroHabitacion);
}
```

Esto permite que `UIHospitalizacion` consulte habitaciones libres sin conocer cómo se calcula internamente — respetando el principio de **separación de responsabilidades**.

---

## Repositorios

### `CsvRepository<T>` (clase base abstracta)
Clase genérica que elimina la duplicación de lógica CSV. Todos los repositorios concretos la heredan.

```csharp
public abstract class CsvRepository<T>
{
    public List<T> CargarTodos();
    public void GuardarTodos(List<T> registros);
}
```

### Repositorios concretos

| Clase | Hereda de | Archivo CSV | Notas |
|---|---|---|---|
| `PacienteHistorialRepository` | `CsvRepository<Paciente>` | `pacientes.csv` | Serializa listas internas del historial con `\|` |
| `DoctorRepository` | `CsvRepository<Doctor>` | `doctores.csv` | — |
| `CitaRepository` | `CsvRepository<Cita>` | `citas.csv` | — |
| `HospitalizacionRepository` | `CsvRepository<Hospitalizacion>` | `hospitalizaciones.csv` | Implementa `IAsignacionHabitacion` |

---

## Capa de presentación

Cada clase UI gestiona un módulo del sistema con operaciones CRUD completas. Ninguna UI contiene lógica de negocio ni acceso directo a archivos — todo delega al repositorio correspondiente.

| Clase | Repositorio que usa | Descripción |
|---|---|---|
| `UIPaciente` | `PacienteHistorialRepository` | CRUD de pacientes + acceso al historial |
| `UIDoctor` | `DoctorRepository` | CRUD de doctores |
| `UICita` | `CitaRepository` | CRUD de citas, selecciona paciente y doctor |
| `UIHospitalizacion` | `HospitalizacionRepository` | Hospitalizar, dar de alta, eliminar |
| `UIHistorial` | *(sin repositorio propio)* | Submenú invocado desde `UIPaciente` |
| `Program` | — | Menú principal, punto de entrada |

### `UIHistorial`
No tiene menú propio en el menú principal. Se invoca desde `UIPaciente` pasándole el `HistorialMedico` del paciente seleccionado. El historial se guarda cuando `UIPaciente` guarda el paciente completo.

---

## Persistencia CSV

Los archivos CSV se generan automáticamente en la carpeta de ejecución del programa (`bin/Debug/net9.0/`) la primera vez que se guardan datos.

| Archivo | Contenido |
|---|---|
| `pacientes.csv` | Datos del paciente + historial serializado con `\|` |
| `doctores.csv` | Datos del doctor |
| `citas.csv` | Id, fecha, IdPaciente, IdDoctor, motivo |
| `hospitalizaciones.csv` | Id, IdPaciente, IdDoctor, habitación, fechas |

El historial médico se serializa así dentro de `pacientes.csv`:

```
Identificacion,Nombre,Edad,Telefono,FechaIngreso,Diagnosticos,Tratamientos,Observaciones
1,Juan Pérez,35,3001234567,2025-01-10,Gripa|Fiebre,Ibuprofeno|Reposo,Mejoró en 3 días
```

---

## Diagrama de clases

El diagrama de clases completo (modelos, repositorios y UIs) se encuentra en el archivo `diagrama_clases.txt` incluido en el repositorio, con todas las propiedades, métodos y relaciones UML listadas para importar en draw.io.

Relaciones principales:
- `Paciente` y `Doctor` **heredan** de `Persona`
- Todos los repositorios **heredan** de `CsvRepository<T>`
- `HospitalizacionRepository` **implementa** `IAsignacionHabitacion`
- `Paciente` tiene **composición** con `HistorialMedico`
- `Hospital` tiene **composición** con `Habitacion` y **agregación** con `Doctor` y `Paciente`

---

## Requisitos

- [.NET 8 o superior](https://dotnet.microsoft.com/download)
- Paquete NuGet: [CsvHelper](https://joshclose.github.io/CsvHelper/) `>= 30.0`

Instalación de CsvHelper desde la consola del gestor de paquetes en Visual Studio:

```
Install-Package CsvHelper
```

O desde la terminal:

```bash
dotnet add package CsvHelper
```

---

## Cómo ejecutar

1. Clonar o descargar el repositorio.
2. Abrir `SistemaHospital.slnx` en **Visual Studio 2022** o superior.
3. Verificar que `AppConsole` está configurado como proyecto de inicio.
4. Asegurarse de que `AppConsole` tiene referencia al proyecto `SistemaHospital`.
5. Ejecutar con `F5` o `Ctrl + F5`.

Al iniciar verás el menú principal:

```
╔══════════════════════════════╗
║    Sistema Hospital          ║
╠══════════════════════════════╣
║  1. Pacientes                ║
║  2. Doctores                 ║
║  3. Citas                    ║
║  4. Hospitalizaciones        ║
║  5. Salir                    ╚
 ══════════════════════════════╝
Opción:
```

---

## Autores

Proyecto académico desarrollado como ejercicio de Programación Orientada a Objetos en C#.

# Simulación de Atención Hospitalaria en C#

## Descripción
Este proyecto simula la gestión de la atención hospitalaria utilizando hilos y concurrencia en C#. A lo largo de varios ejercicios, se implementan distintos escenarios relacionados con la llegada de pacientes, su atención en consulta, la asignación de prioridades y el uso de máquinas de diagnóstico. El objetivo principal es modelar el comportamiento del sistema hospitalario mediante el uso de programación concurrente.

## Tecnologías utilizadas
- **Lenguaje:** C#
- **Entorno de desarrollo:** Visual Studio
- **Concurrencia:** Uso de Threads, `SemaphoreSlim` y `PriorityQueue`

## Estructura del Proyecto
El proyecto está dividido en distintos ejercicios que aumentan en complejidad:

### **Ejercicio #1 - Consulta Médica**
- Se simula la llegada de pacientes y su atención por parte de los médicos.
- Cada paciente tiene un tiempo de consulta aleatorio.
- Se crean hilos para cada paciente y la consulta se gestiona con semáforos.

### **Ejercicio #2 - Unidades de Diagnóstico**
- Se introduce la posibilidad de que algunos pacientes necesiten diagnóstico adicional.
- Se agregan dos máquinas de diagnóstico que solo pueden atender a un paciente a la vez.
- Se sincroniza el acceso a las máquinas para mantener el orden de llegada.

### **Ejercicio #2 - Más Pacientes**
- Se incrementa el número de pacientes a 20.
- Los pacientes llegan secuencialmente cada 2 segundos y esperan su turno en consulta.

### **Ejercicio #2 - Prioridades de los Pacientes**
- Se introduce un sistema de prioridades:
  - **Emergencias (Nivel 1)**: Se atienden primero.
  - **Urgencias (Nivel 2)**: Se atienden después de las emergencias.
  - **Consultas Generales (Nivel 3)**: Se atienden al final.
- Se utiliza una cola de prioridad para garantizar el orden correcto.

### **Ejercicio #2 - Estadísticas y Logs**
- Se calculan y muestran las siguientes estadísticas al final de la simulación:
  - Cantidad de pacientes atendidos por nivel de prioridad.
  - Tiempo promedio de espera por paciente.
  - Porcentaje de uso de las máquinas de diagnóstico.

### **Ejercicio #3 - Pacientes Infinitos**
- Se crea un generador de pacientes que los genera de forma indefinida o hasta un límite especificado por el usuario.
- Se realizan pruebas con distintas cantidades de pacientes (50, 100, 1000) para evaluar el rendimiento del sistema.
- Se analizan los comportamientos inesperados y se plantean mejoras para adaptarse a escenarios más exigentes.

## Instrucciones de Ejecución
1. Abrir **Visual Studio**.
2. Crear un nuevo **proyecto de Aplicación de Consola en C#**.
3. Copiar y pegar el código en `Program.cs`.
4. Ejecutar el programa con `Ctrl + F5`.
5. En el Ejercicio #3, ingresar el número de pacientes que se desean generar.

## Conclusiones
Esta práctica permite explorar conceptos clave de la programación concurrente en C#, como la sincronización de hilos, el uso de semáforos y estructuras de datos eficientes para gestionar colas de espera. Además, proporciona una simulación realista de un entorno hospitalario, donde la gestión eficiente de recursos es fundamental.


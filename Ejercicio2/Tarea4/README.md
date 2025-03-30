# Simulación de Prioridades de Pacientes - Tarea #4

## Descripción
Este proyecto gestiona las prioridades de los pacientes al llegar al hospital. Los pacientes tienen tres niveles de prioridad: **Emergencias (1)**, **Urgencias (2)**, y **Consultas generales (3)**. Se atienden primero las emergencias, seguidas de las urgencias y, finalmente, las consultas generales.

## Tecnologías utilizadas
- **Lenguaje**: C#
- **Entorno de desarrollo**: Visual Studio
- **Concurrencia**: Threads

## Instrucciones de ejecución
1. Abrir Visual Studio.
2. Crear un nuevo proyecto de Aplicación de Consola.
3. Copiar el código en `Program.cs`.
4. Ejecutar con Ctrl + F5.

## Explicación del código
1. Se asigna un nivel de prioridad a cada paciente (Emergencia, Urgencia, o Consulta General).
2. Se utiliza una **cola de prioridad** para asegurar que los pacientes sean atendidos en el orden correcto: primero los de emergencia, luego los de urgencia, y finalmente los de consulta general.
3. El tiempo de consulta es aleatorio, variando entre 5 y 15 segundos.
4. Los pacientes son atendidos según su prioridad y el orden de llegada si tienen la misma prioridad.

## Preguntas y Respuestas

### 1️ Explica el planteamiento de tu código y plantea otra posibilidad de solución a la que has programado y por qué has escogido la tuya.
He utilizado una cola de prioridad para asegurar que los pacientes se atiendan según su nivel de prioridad. La ventaja de esta implementación es que es sencilla y eficiente, ya que la cola maneja automáticamente el orden de los pacientes.

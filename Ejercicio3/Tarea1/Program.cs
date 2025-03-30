using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Paciente
{
    public int Id { get; set; }
    public int LlegadaHospital { get; set; }
    public int TiempoConsulta { get; set; }
    public int Prioridad { get; set; }
    public bool RequiereDiagnostico { get; set; }
    public static int contador = 1;

    public Paciente(int llegada, int prioridad, bool requiereDiagnostico)
    {
        Id = contador++;
        LlegadaHospital = llegada;
        TiempoConsulta = new Random().Next(5, 16); // Tiempo de consulta entre 5 y 15 segundos
        Prioridad = prioridad;
        RequiereDiagnostico = requiereDiagnostico;
    }
}

class Program
{
    static SemaphoreSlim consultaMedica = new SemaphoreSlim(4, 4); // 4 consultas disponibles
    static List<Thread> hilos = new List<Thread>();

    static void Main(string[] args)
    {
        // Solicitar al usuario el número de pacientes a generar
        Console.WriteLine("Introduce el número de pacientes a generar (por ejemplo, 50, 1000):");
        int cantidadPacientes = int.Parse(Console.ReadLine()); // Leer la cantidad de pacientes

        // Empezamos la generación de pacientes en un hilo independiente
        Thread generador = new Thread(() => GenerarPacientes(cantidadPacientes));
        generador.Start();

        // Dejar que el generador siga funcionando mientras atendemos a los pacientes
        generador.Join(); // Esperamos que el generador termine antes de salir
        Console.WriteLine("Todos los pacientes han sido atendidos.");
    }

    static void GenerarPacientes(int cantidadPacientes)
    {
        int contador = 1;

        // Generar pacientes según la cantidad especificada por el usuario
        for (int i = 0; i < cantidadPacientes; i++)
        {
            // Crear un paciente con atributos aleatorios
            var prioridad = new Random().Next(1, 4); // Prioridad entre 1 (Emergencia) y 3 (Consulta general)
            var requiereDiagnostico = new Random().Next(0, 2) == 1; // Genera aleatoriamente si requiere diagnóstico
            var paciente = new Paciente(contador++, prioridad, requiereDiagnostico);

            // Mostrar información del paciente generado
            Console.WriteLine($"Paciente {paciente.Id} creado. Prioridad: {paciente.Prioridad}, Requiere diagnóstico: {paciente.RequiereDiagnostico}");

            // Crear un hilo para atender al paciente
            Thread hilo = new Thread(() => AtenderPaciente(paciente));
            hilos.Add(hilo);
            hilo.Start();

            Thread.Sleep(2000); // Esperar 2 segundos antes de generar otro paciente
        }

        // Esperar que todos los hilos terminen antes de continuar
        foreach (var hilo in hilos)
        {
            hilo.Join();
        }
    }

    static void AtenderPaciente(Paciente paciente)
    {
        consultaMedica.Wait(); // Espera que haya una consulta disponible
        Console.WriteLine($"Paciente {paciente.Id} de prioridad {paciente.Prioridad} ha entrado en consulta.");

        Thread.Sleep(paciente.TiempoConsulta * 1000); // Simula el tiempo de consulta

        Console.WriteLine($"Paciente {paciente.Id} ha salido de consulta.");
        consultaMedica.Release(); // Liberamos una consulta médica
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Paciente
{
    public int Id { get; set; }
    public int LlegadaHospital { get; set; }
    public int TiempoConsulta { get; set; }
    public int Prioridad { get; set; } // Nueva propiedad: Prioridad
    public static int contador = 1;

    public Paciente(int llegada, int prioridad)
    {
        Id = contador++;
        LlegadaHospital = llegada;
        TiempoConsulta = new Random().Next(5, 16);
        Prioridad = prioridad;
    }
}

class Program
{
    static SemaphoreSlim consultaMedica = new SemaphoreSlim(4, 4); // 4 consultas
    static PriorityQueue<Paciente, int> colaPacientes = new PriorityQueue<Paciente, int>();

    static void Main(string[] args)
    {
        // Simulamos pacientes con distintas prioridades
        colaPacientes.Enqueue(new Paciente(0, 1), 1); // EmergenciaA
        colaPacientes.Enqueue(new Paciente(2, 3), 3); // Consulta general
        colaPacientes.Enqueue(new Paciente(4, 2), 2); // Urgencia
        colaPacientes.Enqueue(new Paciente(6, 1), 1); // Emergencia

        // Crear hilos para la atención médica
        List<Thread> hilos = new List<Thread>();
        while (colaPacientes.Count > 0)
        {
            var paciente = colaPacientes.Dequeue();
            Thread hilo = new Thread(() => AtenderPaciente(paciente));
            hilos.Add(hilo);
            hilo.Start();
            Thread.Sleep(2000); // Llegada de pacientes cada 2 segundos
        }

        foreach (var hilo in hilos)
        {
            hilo.Join();
        }

        Console.WriteLine("Todos los pacientes han sido atendidos.");
    }

    static void AtenderPaciente(Paciente paciente)
    {
        consultaMedica.Wait(); // Esperar que haya consulta disponible
        Console.WriteLine($"Paciente {paciente.Id} de prioridad {paciente.Prioridad} ha entrado en consulta.");

        Thread.Sleep(paciente.TiempoConsulta * 1000); // Simular tiempo de consulta

        Console.WriteLine($"Paciente {paciente.Id} ha salido de consulta.");
        consultaMedica.Release(); // Liberar consulta médica
    }
}

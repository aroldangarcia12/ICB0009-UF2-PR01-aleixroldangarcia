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
    static List<Paciente> pacientesAtendidos = new List<Paciente>();
    static int totalPacientes = 0;
    static int totalTiempoEspera = 0;

    static void Main(string[] args)
    {
        List<Paciente> pacientes = new List<Paciente>
        {
            new Paciente(0, 1), // Emergencia
            new Paciente(2, 3), // Consulta general
            new Paciente(4, 2), // Urgencia
            new Paciente(6, 1) // Emergencia
        };

        List<Thread> hilos = new List<Thread>();
        foreach (var paciente in pacientes)
        {
            Thread hilo = new Thread(() => AtenderPaciente(paciente));
            hilos.Add(hilo);
            hilo.Start();
            Thread.Sleep(2000); // Llegada de pacientes cada 2 segundos
        }

        foreach (var hilo in hilos)
        {
            hilo.Join();
        }

        Console.WriteLine("Estadísticas:");
        Console.WriteLine($"Pacientes atendidos: Emergencias: {pacientesAtendidos.Count(p => p.Prioridad == 1)}, Urgencias: {pacientesAtendidos.Count(p => p.Prioridad == 2)}, Consultas generales: {pacientesAtendidos.Count(p => p.Prioridad == 3)}");
        Console.WriteLine($"Tiempo promedio de espera: {totalTiempoEspera / totalPacientes}s");
        Console.WriteLine($"Uso promedio de máquinas de diagnóstico: {((double)pacientesAtendidos.Count / 4) * 100}%");
    }

    static void AtenderPaciente(Paciente paciente)
    {
        DateTime inicioEspera = DateTime.Now;

        consultaMedica.Wait(); // Esperar que haya consulta disponible
        pacientesAtendidos.Add(paciente);
        totalPacientes++;

        Console.WriteLine($"Paciente {paciente.Id} de prioridad {paciente.Prioridad} ha entrado en consulta.");

        Thread.Sleep(paciente.TiempoConsulta * 1000); // Simular tiempo de consulta

        totalTiempoEspera += (int)(DateTime.Now - inicioEspera).TotalSeconds;

        Console.WriteLine($"Paciente {paciente.Id} ha salido de consulta.");
        consultaMedica.Release(); // Liberar consulta médica
    }
}


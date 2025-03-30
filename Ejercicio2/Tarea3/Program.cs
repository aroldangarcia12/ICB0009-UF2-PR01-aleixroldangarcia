using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Paciente
{
    public int Id { get; set; }
    public int LlegadaHospital { get; set; }
    public int TiempoConsulta { get; set; }
    public int Estado { get; set; }
    public static int contador = 1;

    public Paciente(int llegada)
    {
        Id = contador++;
        LlegadaHospital = llegada;
        TiempoConsulta = new Random().Next(5, 16);
        Estado = 0; // Estado: EsperaConsulta
    }
}

class Program
{
    static SemaphoreSlim consultaMedica = new SemaphoreSlim(4, 4); // 4 consultas
    static Queue<Paciente> colaPacientes = new Queue<Paciente>();

    static void Main(string[] args)
    {
        for (int i = 0; i < 20; i++)
        {
            Paciente paciente = new Paciente(i * 2); // Paciente llega cada 2 segundos
            colaPacientes.Enqueue(paciente);
            Console.WriteLine($"Paciente {paciente.Id} ha llegado al hospital en el segundo {paciente.LlegadaHospital}");
            Thread.Sleep(2000); // Esperar 2 segundos para el siguiente paciente
        }

        // Crear hilos para la atención médica
        List<Thread> hilos = new List<Thread>();
        foreach (var paciente in colaPacientes)
        {
            Thread hilo = new Thread(() => AtenderPaciente(paciente));
            hilos.Add(hilo);
            hilo.Start();
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
        paciente.Estado = 1; // Estado: Consulta
        Console.WriteLine($"Paciente {paciente.Id} ha entrado en consulta.");

        Thread.Sleep(paciente.TiempoConsulta * 1000); // Simular tiempo de consulta

        Console.WriteLine($"Paciente {paciente.Id} ha salido de consulta.");
        consultaMedica.Release(); // Liberar consulta médica
    }
}

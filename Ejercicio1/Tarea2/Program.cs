using System;
using System.Collections.Generic;
using System.Threading;

class Paciente
{
    public int Id { get; }
    public int LlegadaHospital { get; }
    public int TiempoConsulta { get; }
    public string Estado { get; set; }

    public Paciente(int id, int llegadaHospital, int tiempoConsulta)
    {
        Id = id;
        LlegadaHospital = llegadaHospital;
        TiempoConsulta = tiempoConsulta;
        Estado = "Espera";
    }
}

class Program
{
    static Random rand = new Random();
    static object lockObj = new object();
    static SemaphoreSlim medicosDisponibles = new SemaphoreSlim(4); // 4 médicos disponibles

    static void AtenderPaciente(Paciente paciente)
    {
        medicosDisponibles.Wait(); // Esperar turno para consulta

        lock (lockObj)
        {
            Console.WriteLine($"Paciente {paciente.Id} (Llegada: {paciente.LlegadaHospital}s) entra en consulta. Duración: {paciente.TiempoConsulta}s.");
        }

        paciente.Estado = "Consulta";
        Thread.Sleep(paciente.TiempoConsulta * 1000); // Simula el tiempo de consulta
        paciente.Estado = "Finalizado";

        lock (lockObj)
        {
            Console.WriteLine($"Paciente {paciente.Id} finaliza su consulta tras {paciente.TiempoConsulta}s.");
        }

        medicosDisponibles.Release(); // Liberar médico
    }

    static void Main()
    {
        List<Thread> hilosPacientes = new List<Thread>();
        List<Paciente> listaPacientes = new List<Paciente>();

        for (int i = 1; i <= 4; i++)
        {
            int id = rand.Next(1, 101);
            int tiempoConsulta = rand.Next(5, 16);
            Paciente paciente = new Paciente(id, (i - 1) * 2, tiempoConsulta);
            listaPacientes.Add(paciente);

            Console.WriteLine($"Paciente {paciente.Id} ha llegado al hospital (Tiempo de llegada: {paciente.LlegadaHospital}s).");

            Thread hiloPaciente = new Thread(() => AtenderPaciente(paciente));
            hilosPacientes.Add(hiloPaciente);
            hiloPaciente.Start();
            Thread.Sleep(2000); // Simula la llegada cada 2 segundos
        }

        foreach (var hilo in hilosPacientes)
        {
            hilo.Join(); // Esperar a que terminen todos los hilos
        }

        Console.WriteLine("Todos los pacientes han sido atendidos.");
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}

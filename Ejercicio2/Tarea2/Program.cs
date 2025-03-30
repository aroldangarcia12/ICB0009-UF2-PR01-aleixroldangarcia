using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static SemaphoreSlim semaforoMedicos = new SemaphoreSlim(4); // 4 médicos
    static SemaphoreSlim semaforoDiagnostico = new SemaphoreSlim(2); // 2 máquinas de diagnóstico
    static Queue<Paciente> colaDiagnostico = new Queue<Paciente>(); // Orden de llegada al diagnóstico
    static object lockObj = new object(); // Para sincronización
    static AutoResetEvent diagnosticoDisponible = new AutoResetEvent(false);
    static Random random = new Random();
    static int tiempoGlobal = 0;

    static void Main()
    {
        List<Paciente> pacientes = new List<Paciente>();
        for (int i = 1; i <= 4; i++)
        {
            Paciente paciente = new Paciente(i, tiempoGlobal, random.Next(5, 16), random.Next(0, 2) == 1);
            pacientes.Add(paciente);
            Thread hiloPaciente = new Thread(() => AtenderPaciente(paciente));
            hiloPaciente.Start();
            Thread.Sleep(2000); // Llega un paciente cada 2 segundos
            tiempoGlobal += 2;
        }
    }

    static void AtenderPaciente(Paciente paciente)
    {
        Console.WriteLine($"Paciente {paciente.Id} ha llegado. Estado: EsperaConsulta.");
        semaforoMedicos.Wait();
        paciente.Estado = "Consulta";
        Console.WriteLine($"Paciente {paciente.Id} en consulta. Duración: {paciente.TiempoConsulta} segundos.");
        Thread.Sleep(paciente.TiempoConsulta * 1000);
        semaforoMedicos.Release();

        if (paciente.RequiereDiagnostico)
        {
            lock (lockObj)
            {
                colaDiagnostico.Enqueue(paciente);
            }
            paciente.Estado = "EsperaDiagnostico";
            Console.WriteLine($"Paciente {paciente.Id} espera diagnóstico en la cola.");
            diagnosticoDisponible.WaitOne(); // Espera su turno en la cola
            RealizarDiagnostico(paciente);
        }
        else
        {
            paciente.Estado = "Finalizado";
            Console.WriteLine($"Paciente {paciente.Id} finalizó su atención.");
        }
    }

    static void RealizarDiagnostico(Paciente paciente)
    {
        semaforoDiagnostico.Wait();
        paciente.Estado = "Diagnostico";
        Console.WriteLine($"Paciente {paciente.Id} en diagnóstico. Duración: 15 segundos.");
        Thread.Sleep(15000);
        semaforoDiagnostico.Release();
        paciente.Estado = "Finalizado";
        Console.WriteLine($"Paciente {paciente.Id} finalizó su diagnóstico.");

        lock (lockObj)
        {
            colaDiagnostico.Dequeue(); // Elimina al paciente de la cola
            if (colaDiagnostico.Count > 0)
            {
                diagnosticoDisponible.Set(); // Libera al siguiente paciente en la cola
            }
        }
    }
}

class Paciente
{
    public int Id { get; set; }
    public int LlegadaHospital { get; set; }
    public int TiempoConsulta { get; set; }
    public bool RequiereDiagnostico { get; set; }
    public string Estado { get; set; }

    public Paciente(int id, int llegadaHospital, int tiempoConsulta, bool requiereDiagnostico)
    {
        Id = id;
        LlegadaHospital = llegadaHospital;
        TiempoConsulta = tiempoConsulta;
        RequiereDiagnostico = requiereDiagnostico;
        Estado = "EsperaConsulta";
    }
}

using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Random rand = new Random();
    static object lockObj = new object();

    static void AtenderPaciente(int pacienteId)
    {
        int medico = rand.Next(1, 5); // Médico aleatorio entre 1 y 4
        Console.WriteLine($"Paciente {pacienteId} entra en consulta con el Médico {medico}.");
        Thread.Sleep(10000); // Simula los 10 segundos de consulta
        Console.WriteLine($"Paciente {pacienteId} sale de consulta del Médico {medico}.");
    }

    static void Main()
    {
        List<Thread> hilosPacientes = new List<Thread>();

        for (int i = 1; i <= 4; i++)
        {
            int pacienteId = i;
            Console.WriteLine($"Paciente {pacienteId} ha llegado al hospital.");
            Thread hiloPaciente = new Thread(() => AtenderPaciente(pacienteId));
            hilosPacientes.Add(hiloPaciente);
            hiloPaciente.Start();
            Thread.Sleep(2000); // Simula la llegada cada 2 segundos
        }

        foreach (var hilo in hilosPacientes)
        {
            hilo.Join(); // Esperar a que terminen todos los hilos
        }

        Console.WriteLine("Todos los pacientes han sido atendidos.");
    }
}

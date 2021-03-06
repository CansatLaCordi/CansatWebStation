﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace Cansat.PruebaSerial
{
    class Program
    {
        static SerialParser sp;
        static string GetPort()
        {
            var ports = SerialPort.GetPortNames();
            int p = 0;
            string tmp;
            do
            {
                Console.WriteLine("Puertos seriales:");
                for (int i = 0; i < ports.Count(); i++)
                {
                    Console.WriteLine("[{0}] {1}", i + 1, ports[i]);
                }
                try
                {
                    tmp = Console.ReadLine();
                    p = int.Parse(tmp);
                }
                catch (Exception)
                {
                    p = 0;
                }
            } while (p <= 0 || p > ports.Count());

            return ports[p - 1];
        }

        static void Main(string[] args)
        {
            sp = new SerialParser();
            Trace.WriteLine("Tracking iniciado", "[Info]");

            if (SerialPort.GetPortNames().Count() == 0)
            {
                Console.WriteLine("No hay puertos!!");
                Console.ReadKey();
                return;
            }

            string port = GetPort();
            SerialPort ser = new SerialPort(port, 9600);
            ser.DataReceived += ser_DataReceived;
            ser.Open();
            Trace.WriteLine("Tracking iniciado","[Info]");

            Console.ReadKey();
            ser.Close();
        }

        static void ser_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort ser = sender as SerialPort;
            var line = ser.ReadLine();
            Console.WriteLine(line);
            sp.InsertLine(line);
        }
    }
}

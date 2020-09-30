using CykelLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CykelTcp
{
    class Server
    {
        private static readonly List<Cyklen> cykelList = new List<Cyklen>
        {
            new Cyklen(1, "Rød", 500, 12),
            new Cyklen(2, "Gul", 4500, 29),
            new Cyklen(3, "Hvid", 10000, 21),
            new Cyklen(4, "Sort", 100, 4),
            new Cyklen(5, "Blå", 2300, 20)
        };

        public void Start()
        {
            // Her kommer koden

            // Vi opretter server
            var server = new TcpListener(IPAddress.Loopback, 4646);
            server.Start();

            while (true)
            {
                var socket = server.AcceptTcpClient();

                Task.Run(() =>
                {
                    var tempSocket = socket;
                    DoClient(tempSocket);
                });
            }
        }

        public void DoClient(TcpClient socket)
        {
            StreamReader sr = new StreamReader(socket.GetStream());
            StreamWriter sw = new StreamWriter(socket.GetStream());

            String str = sr.ReadLine();
            Console.WriteLine($"Her er server input: {str}");

            // Disse to nedenunder bruger kun server, så der skal bruges socket test
            if (str == "HentAlle")
            {
                sw.WriteLine($"Cykellisten : ");
                Console.WriteLine($"Cykel i listen : ");
                foreach (var cykelList in cykelList) sw.WriteLine(cykelList);
                foreach (var cykelList in cykelList)
                {
                    Console.WriteLine(cykelList);
                }
            }
            else if (str == "Hent")
            {
                Console.WriteLine($"Skriv dit Id på cyklen : ");
                sw.WriteLine("Skriv Id'et : ");

                sw.Flush();

                string str2;
                str2 = sr.ReadLine();
                var i = int.Parse(str2);
                var c = cykelList.FirstOrDefault(cykelList => cykelList.Id == i);

                sw.WriteLine(c);
                Console.WriteLine(c);
            }
            // Ved denne bliver der brugt både server og client, så ingen behov for socket test
            else if (str == "Gem")
            {
                string str3;
                str3 = sr.ReadLine();
                Cyklen cyklen = JsonConvert.DeserializeObject<Cyklen>(str3);

                Console.WriteLine("Modtager cykel : " + cyklen);
                Console.WriteLine("Modtager cykel json string " + str3);
                //sw.WriteLine("Modtager cykel : " + str3);
            }

            sw.Flush();
        }
    }
}

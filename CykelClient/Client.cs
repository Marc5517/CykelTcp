using CykelLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CykelClient
{
    class Client
    {
        public void Start()
        {
            TcpClient socket = new TcpClient("localhost", 4646);

            StreamReader sr = new StreamReader(socket.GetStream());
            StreamWriter sw = new StreamWriter(socket.GetStream());


            Cyklen cyklen = new Cyklen(123, "Gul", 4700, 7);
            String json = JsonConvert.SerializeObject(cyklen);


            sw.WriteLine("Gem");
            sw.WriteLine(json);
            sw.Flush();

            socket.Close();
        }
    }
}

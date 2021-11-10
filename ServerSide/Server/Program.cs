using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using Server.PointItems;
using Server.Powerups;

namespace Server
{
    class Program
    {
        private static Facade facade = new Facade();
                
        static void Main(string[] args)
        {
            StartServer();
        }

        /// <summary>
        /// Main Server Loop
        /// </summary>
        private static void StartServer()
        {
            
            facade.GenerateAllEnemies();
            facade.GenerateItems();
            facade.AwaitPlayerConnections();
            facade.StartGame();

            // Infinite loop for program not to shut down, eventually will have to be replaced
            // with game end condition
            while (!false)
            {
            }
        }
    }
}

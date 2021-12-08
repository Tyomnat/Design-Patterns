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
using Server.State;

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
            Game Game = new Game();
            facade.GenerateAllEnemies();
            facade.GenerateItems();
            facade.GeneratePowerups();
            facade.AwaitPlayerConnections();
            facade.StartGame(Game);


            // Infinite loop for program not to shut down, eventually will have to be replaced
            // with game end condition
            while (Game.state.GetType().Name != typeof(GameEndState).Name)
            {
            }
            //facade.InvokeObserver(new Event("game_ended", Game.winnerId.ToString()));
            Game.state.Handle();

        }
    }
}

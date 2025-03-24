using System.Globalization;
using ITAI_Assignemnt_1.game;
using ITAI_Assignment_1.Game;

class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "tournament")
        {
            int numberOfGamesPerMatchup = 2;
            TournamentRunner.RunAllTournaments(numberOfGamesPerMatchup);
        }
        else
        {
            Engine engine = new Engine();
            engine.Start();
        }
    }
}

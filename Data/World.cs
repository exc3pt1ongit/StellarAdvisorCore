using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Logging;

namespace StellarAdvisorCore.Data
{
    public static class World
    {
        public static List<Character>? Characters { get; private set; }

        public static void Load()
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                try
                {
                    Characters = sqlite.Characters.ToList();
                    Logger.LogSuccess($"World: {Characters.Count} characters successfully loaded");
                }
                catch (Exception e)
                {
                    Logger.LogError($"Exception catched during initializing World: {e.Message}");
                }
            }
        }
    }
}

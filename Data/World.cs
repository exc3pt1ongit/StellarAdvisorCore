using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Entities.Settlements;
using StellarAdvisorCore.Logging;

namespace StellarAdvisorCore.Data
{
    public sealed class World
    {
        private static readonly Lazy<World> instance =
            new Lazy<World>(() => new World());

        public static World Instance { get { return instance.Value; } }

        public List<Character>? Characters { get; private set; }
        public List<SettlementBase>? Settlements { get; private set; }

        private World()
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                try
                {
                    Settlements = new List<SettlementBase>()
                    {
                        new AuroraEmpire()
                    };

                    Characters = sqlite.Characters.ToList();
                    Logger.LogSuccess($"World: {Characters.Count} characters successfully loaded");

                    foreach (var settlement in Settlements)
                    {
                        settlement.LoadResidents();
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError($"Exception catched during initializing World: {e.Message}");
                }
            }
        }
    }
}

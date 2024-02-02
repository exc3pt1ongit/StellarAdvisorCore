using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Models.Entities.Fractions;
using StellarAdvisorCore.Data.Models.Entities.Settlements;
using StellarAdvisorCore.Logging;

namespace StellarAdvisorCore.Data
{
    public sealed class World
    {
        private static readonly Lazy<World> instance =
            new Lazy<World>(() => new World());

        public static World Instance { get { return instance.Value; } }
        public List<Settlement>? Settlements { get; private set; }

        private World()
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                try
                {
                    Settlements = new List<Settlement>()
                    {
                        new Settlement("Aurora Empire", new List<FractionBase>()
                        {
                            new FractionBase("Aurora Empire Rangers"),
                            new FractionBase("Aurora Empire Guard")
                        })
                    };

                    var characters = sqlite.Characters.ToList();
                    Logger.LogSuccess($"World: {characters.Count} characters successfully loaded");

                    foreach (var settlement in Settlements)
                    {
                        settlement.LoadResidents();

                        if (settlement.Fractions == null) continue;
                        foreach (var fraction in settlement.Fractions)
                        {
                            fraction.Settlement = settlement;
                            fraction.LoadMembers();
                        }
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

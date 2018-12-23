using GameServerCore.Enums;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace MoltenShield
{
    internal class MoltenShield : IBuffGameScript
    {
        private StatsModifier _statMod;
        private IBuff _visualBuff;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _statMod = new StatsModifier();
            _statMod.Armor.FlatBonus = 20f + (10f * (ownerSpell.Level - 1));
            _statMod.MagicResist.FlatBonus = 20f + (10f * (ownerSpell.Level - 1));
            unit.AddStatModifier(_statMod);
            _visualBuff = AddBuffHudVisual("MoltenShield", 5.0f, 1, BuffType.SPELL_SHIELD, unit);
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            RemoveBuffHudVisual(_visualBuff);
            unit.RemoveStatModifier(_statMod);
        }

        public void OnUpdate(double diff)
        {

        }
    }
}

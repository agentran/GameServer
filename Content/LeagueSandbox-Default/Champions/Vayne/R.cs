using System;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class VayneInquisition : IGameScript
    {

        public void OnActivate(IChampion owner)
        {

        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {

        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            float duration = new float[] { 8.0f, 10.0f, 12.0f }[spell.Level - 1];

            ((ObjAiBase)target).AddBuffGameScript("FinalHourBuff", "FinalHourBuff", spell, duration, true);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
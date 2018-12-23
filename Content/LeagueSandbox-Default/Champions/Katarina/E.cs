using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.API;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class KatarinaE : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            if (target == null)
            {
                return;
            }

            spell.SpellAnimation("SPELL3", owner);
            AddParticleTarget(owner, "katarina_shadowStep_cas.troy", owner);
            TeleportTo(owner, target.X + 80, target.Y + 80);
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            if (target == null)
            {
                return;
            }

            AddParticleTarget(owner, "katarina_shadowStep_cas.troy", owner);

            var damage = new[] { 60, 85, 110, 135, 160 }[spell.Level - 1] + owner.Stats.AbilityPower.Total * 0.4f;

            if (target.Team != owner.Team)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {

        }

        public void OnUpdate(double diff)
        {
        }
    }
}

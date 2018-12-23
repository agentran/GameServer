using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class RengarW : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            
            AddParticle(owner, "Rengar_Base_W_Roar.troy", owner.X, owner.Y);
            AddParticle(owner, "Rengar_Base_W_Buf.troy", owner.X, owner.Y);
            spell.SpellAnimation("SPELL2", owner);
            
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var units = GetUnitsInRange(owner, 450, true);
            foreach (IAttackableUnit unit in units)
            {
                if(owner as IAttackableUnit != unit)
                {
                    ApplyEffects(owner, unit, spell, null);
                }
            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            target.TakeDamage(owner, 20F + spell.Level * 30F + owner.Stats.AbilityPower.Total * 0.8f,
                DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnUpdate(double diff)
        {
            
        }
    }
}

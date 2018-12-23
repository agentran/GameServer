using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class TwoShivPoison : IGameScript
    {

        public void OnActivate(IChampion owner)
        {
        }

        private void OnUnHi(IAttackableUnit target, bool isCrit)
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
            //ApiFunctionManager.AddParticleTarget(owner, "globalhit_blue_tar.troy", target);
            //var current = new Vector2(owner.X, owner.Y);
            //var to = Vector2.Normalize(new Vector2(target.X, target.Y) - current);
            //var range = to * 1150;
            //var trueCoords = current + range;
            spell.AddProjectileTarget("TwoShivPoison", target);
            //ApiFunctionManager.PrintChat("TwoShivPoison Cast Finished!");
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            var ap = owner.Stats.AbilityPower.FlatBonus * 1.0f;
            var dmg = owner.Stats.AttackDamage.FlatBonus * 1.0f;
            var damage = (float)(spell.Level > 1 ? 50 + (40 * spell.Level) : 50);
            damage += ap + dmg;
            if (target != null && !target.IsDead)
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            projectile.SetToRemove();
        }

        public void OnUpdate(double diff)
        {
        }

        public void OnDaTa()
        {
        }

    }
}

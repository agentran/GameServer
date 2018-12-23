using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Enums;

namespace Spells
{
    public class MoltenShield : IGameScript
    {
        IChampion _championRef;
        public void OnActivate(IChampion owner)
        {
            _championRef = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnUnitHit);
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("SPELL3", owner);

            ((ObjAiBase)target).AddBuffGameScript("MoltenShield", "MoltenShield", spell, 5.0f, true);

            var p = AddParticleTarget(owner, "Annie_E_buf.troy", target, 1);
            CreateTimer(5.0f, () =>
            {
                RemoveParticle(p);
            });
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            var champion = target as IChampion;
            if (champion == null)
                return;
            // Magic Damage: 20 / 30 / 40 / 50 / 60 (+ 20% AP)
            var ap = owner.Stats.AbilityPower.Total * 0.2f;
            var damage = (20f + (10f * (spell.Level - 1))) + ap;
        }

        private void OnUnitHit(IAttackableUnit target, bool isCrit)
        {
            if (!_championRef.IsVisibleByTeam(TeamId.TEAM_PURPLE))
            {
                _championRef.SetVisibleByTeam(TeamId.TEAM_PURPLE, true);
                ISpell spell = _championRef.GetSpell(1);
                //_championRef.Stats.CriticalChance.PercentBonus = spell.Level > 1 ? 1.40f + (2.0f * spell.Level) : 1.40f;
            }
        }

        public void OnUpdate(double diff)
        {
        }
    }
}


using System.Numerics;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.Content;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class Deceive : IGameScript
    {
        IChampion _championRef;
        public void OnActivate(IChampion owner)
        {
            _championRef = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnUnitHit);
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

        public void OnDeactivate(IChampion owner)
        {

        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            AddParticle(owner, "HallucinatePoof.troy", owner.X, owner.Y);
            owner.SetVisibleByTeam(TeamId.TEAM_PURPLE, false);
            CreateTimer(3.5f, () =>
            {
                owner.SetVisibleByTeam(TeamId.TEAM_PURPLE, true);
                //owner.Stats.CriticalChance.PercentBonus = spell.Level > 1 ? 1.40f + (2.0f * spell.Level) : 1.40f;
            });
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var trueCoords = this.getTrueCoords(owner, spell, target);
            TeleportTo(owner, trueCoords.X, trueCoords.Y);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            // no effects added yet
        }

        private Vector2 getTrueCoords(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = owner.GetPosition();
            var to = new Vector2(spell.X, spell.Y) - current;
            var trueCoords = new Vector2();

            if (to.Length() > 450)
            {
                to = Vector2.Normalize(to);
                var range = to * 450;
                trueCoords = new Vector2(current.X, current.Y) + range;
            }
            else
                trueCoords = new Vector2(spell.X, spell.Y);
            return trueCoords;
        }

        public void OnUpdate(double diff)
        {
            //throw new System.NotImplementedException();
        }
    }
}

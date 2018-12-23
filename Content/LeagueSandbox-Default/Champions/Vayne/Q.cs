using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class VayneTumble : IGameScript
    {

        private bool _nextAutoBonusDamage = false;
        private bool _listenerAdded = false;
        private IChampion _owningChampion;
        private ISpell _owningSpell;
        private IBuff _tumbleBuff;

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

            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 300;
            var trueCoords = current + range;

            DashToLocation(owner, trueCoords.X, trueCoords.Y, 1200, false, "Spell1");

            if (!_listenerAdded)
            {
                //ApiEventManager.OnHitUnit.AddListener(this, owner, OnAutoAttack);
                _listenerAdded = true;
            }
            if (_owningChampion != owner)
            {
                _owningChampion = owner;
            }
            if (_owningSpell != spell)
            {
                _owningSpell = spell;
            }
            _nextAutoBonusDamage = true;
            _tumbleBuff = AddBuffHudVisual("VayneTumble", 6.0f, 1, BuffType.COMBAT_ENCHANCER, owner);
            CreateTimer(6.0f, () =>
            {
                // If auto has not yet been consumed
                if (_nextAutoBonusDamage == true)
                {
                    RemoveBuffHudVisual(_tumbleBuff);
                    _nextAutoBonusDamage = false;
                }
            });
        }

        void OnAutoAttack(IAttackableUnit target, bool isCrit)
        {
            if (_nextAutoBonusDamage)
            {
                _nextAutoBonusDamage = false;
                RemoveBuffHudVisual(_tumbleBuff);
                var ad = (new float[] { 0.3f, 0.35f, 0.4f, 0.45f, 0.5f }[_owningSpell.Level - 1]) * _owningChampion.Stats.AttackDamage.Total;
                target.TakeDamage(_owningChampion, ad, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PASSIVE, false);
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

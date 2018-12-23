using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class VayneSilveredBolts : IGameScript
    {
        
        private bool _silverBoltsLearned;
        private double _lastAttackedValidTarget;
        private double _currentTime;
        private IChampion _owningChampion;
        private IObjAiBase _lastTarget;
        private ISpell _owningSpell;
        private byte _silverBoltsStacks;
        private Particle _silverBoltsParticle;
        private IBuff _silverBoltsBuff;

        public void OnActivate(IChampion owner)
        {
            _owningChampion = owner;
            CreateTimer(0.5f, () =>
             {
                 _owningSpell = owner.GetSpell(1);
             });
            //ApiEventManager.OnHitUnit.AddListener(owner, OnAutoAttack);
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            _owningChampion = owner;
            _owningSpell = spell;
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
        }

        void OnAutoAttack(IAttackableUnit target, bool isCrit)
        {
            if(_silverBoltsLearned == false)
            {
                if(_owningSpell.Level >= 1)
                {
                    _silverBoltsLearned = true;
                }
                else
                {
                    return;
                }
            }

            IObjAiBase silverTarget = target as IObjAiBase;
            if (silverTarget != null)
            {
                _lastAttackedValidTarget = _currentTime;

                if (_silverBoltsBuff != null)
                {
                    RemoveBuffHudVisual(_silverBoltsBuff);
                    _silverBoltsBuff = null;
                }

                if (silverTarget != _lastTarget)
                {
                    if (_silverBoltsParticle != null)
                    {
                        RemoveParticle(_silverBoltsParticle);
                        _silverBoltsParticle = null;
                    }
                    if (_lastTarget != null)
                    {
                        _silverBoltsStacks = 0;
                    }
                    _lastTarget = silverTarget;
                }
                _silverBoltsStacks += 1;
                if (_silverBoltsStacks < 3) {
                    _silverBoltsBuff = AddBuffHudVisual("VayneSilveredDebuff", 3.5f, _silverBoltsStacks, BuffType.SHRED, silverTarget, -1);
                }
                if (_silverBoltsParticle != null)
                {
                    RemoveParticle(_silverBoltsParticle);
                }
                if (_silverBoltsStacks == 1)
                {
                    _silverBoltsParticle = AddParticleTarget(_owningChampion, "vayne_W_ring1.troy", silverTarget);
                }
                else if (_silverBoltsStacks == 2)
                {
                    _silverBoltsParticle = AddParticleTarget(_owningChampion, "vayne_W_ring2.troy", silverTarget);
                }
                else
                {
                    _silverBoltsStacks = 0; // We're at 3 stacks. Apply damage and reset to zero.
                    float healthRatio = (new float[] { 0.04f, 0.05f, 0.06f, 0.07f, 0.08f }[_owningSpell.Level - 1]) * silverTarget.Stats.HealthPoints.Total;
                    float damage = new float[] { 20, 30, 40, 50, 60 }[_owningSpell.Level - 1] + healthRatio;
                    silverTarget.TakeDamage(_owningChampion, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PASSIVE, false);
                    AddParticleTarget(_owningChampion, "vayne_W_tar.troy", silverTarget);
                    _lastTarget = null;
                }

            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
            _currentTime += diff;
            if((_lastTarget != null) && _currentTime >= (_lastAttackedValidTarget + 3500)) // if Vayne has not damaged a valid target in 3.5 seconds
            {
                if (_silverBoltsParticle != null)
                {
                    RemoveParticle(_silverBoltsParticle);
                }
                _lastTarget = null;
                _silverBoltsParticle = null;
                _silverBoltsStacks = 0;
                if (_silverBoltsBuff != null)
                {
                    RemoveBuffHudVisual(_silverBoltsBuff);
                }
                _silverBoltsBuff = null;
            }
        }
    }
}

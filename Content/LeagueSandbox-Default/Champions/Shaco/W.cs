using GameServerCore;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class JackInTheBox : IGameScript
    {
        private IGame game;

        Random rand = new Random();

        public void OnActivate(IChampion owner)
        {
        }

        private void OnUnitHit(IAttackableUnit target, bool isCrit)
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
            bool game = owner.RefGame.IsRunning;
            int poof = rand.Next(1, 2);

            var health = 150;
            var fearduration = 0.5f + (0.25 * (spell.Level - 1));
            var apbonus = owner.Stats.AbilityPower.Total * 0.2f;
            var damage = 35 + ((15 * (spell.Level - 1)) + apbonus);
            var attspeed = 1 / 1.8f; // 1.8 attacks a second = .55... seconds per attack
            var castrange = spell.SpellData.CastRange[0];
            var triggrange = 300;
            var sightrange = 600;
            var armor = 50;
            var magresist = 100;
            var spellPos = new Vector2(spell.X, spell.Y);
            var ownerPos = new Vector2(owner.X, owner.Y);

            if (owner.WithinRange(ownerPos, spellPos, castrange))
            {
                IMinion Jack = new Minion(owner.RefGame, owner, spell.X, spell.Y, "ShacoBox", "ShacoBox", sightrange, 0);
                owner.RefGame.ObjectManager.AddObject(Jack);
                Jack.SetVisibleByTeam(owner.Team, true);
                if (poof == 1)
                {
                    AddParticle(owner, "JackintheboxPoof.troy", spell.X, spell.Y);
                } else if (poof == 2)
                {
                    AddParticle(owner, "JackintheboxPoof2.troy", spell.X, spell.Y);
                }

                if (Jack.IsVisibleByTeam(owner.Team))
                {
                    try
                    {
                        CreateTimer(attspeed, () =>
                        {
                            if (!Jack.IsDead)
                            {
                                var units = GetUnitsInRange(Jack, sightrange, true);
                                foreach (var value in units)
                                {
                                    if (owner.Team != value.Team && value is AttackableUnit)
                                    {
                                        Jack.SetTargetUnit(value);
                                        Jack.AutoAttackTarget = value;
                                        Jack.AutoAttackProjectileSpeed = 1450;
                                        Jack.AutoAttackHit(value);
                                        spell.AddProjectile("ShacoBoxBasicAttack", value.X, value.Y);
                                        value.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                                    }
                                }
                            }
                        });
                        CreateTimer(5, () =>
                        {
                            if (!Jack.IsDead)
                            {
                                Jack.Die(Jack);
                            }
                            LogInfo("Jack dead.");
                        });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }

        public void OnDaTa()
        {
        }

    }
}

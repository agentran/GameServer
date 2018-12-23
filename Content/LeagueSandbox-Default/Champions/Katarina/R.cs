using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class KatarinaR : IGameScript
    {
        public bool hasbuff = false;
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("SPELL4", owner);
            AddParticle(owner, "katarinaDeathLotus_indicator_cas.troy", owner.X, owner.Y);
            CreateTimer(0.51f, () =>
            {
                spell.SpellAnimation("SPELL4", owner);
            });
            var units = GetUnitsInRange(owner, 550, true);
            foreach (var unit in units)
            {
                if ((unit.Team != owner.Team) && unit is ObjAiBase && !(unit is BaseTurret))
                {
                    //MAGIC DAMAGE: 35 / 55 / 75 (+17.5% AP) (+ 30% BONUS AD)
                    var ap = owner.Stats.AbilityPower.Total * .175f;
                    var ad = owner.Stats.AttackDamage.FlatBonus * .3f;

                    var damage = new[] { 35, 55, 75 }[spell.Level - 1] + ap + ad;

                    for (var i = 0.0f; i < 2.0; i += 0.25f)
                    {
                        CreateTimer(i, () => { unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false); });
                    }

                    // Possibly for non-human champions or a different skin:
                    // AddParticleTarget(owner, "katarina_w_tar_sand.troy", unit);

                    //LogInfo("HasBuff: " + hasbuff);

                    if ((unit is Champion) && (hasbuff == false))
                    {
                        hasbuff = true;
                    }
                }
            }
            if (hasbuff == true)
            {
                ((ObjAiBase)target).AddBuffGameScript("KatarinaRSound", "KatarinaRSound", spell, 2.5f, true);
                var p = AddParticleTarget(owner, "katarina_deathLotus_tar.troy", owner, 1);

                CreateTimer(2.5f, () =>
                {
                    RemoveParticle(p);
                    hasbuff = false;
                });
            }
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}

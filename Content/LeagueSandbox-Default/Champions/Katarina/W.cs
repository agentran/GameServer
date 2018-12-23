using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class KatarinaW : IGameScript
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
            spell.SpellAnimation("SPELL2", owner);
            AddParticleTarget(owner, "katarina_w_cas.troy", owner, 1, "C_BUFFBONE_GLB_CHEST_LOC");

            // Unused:
            // AddParticle(owner, "katarina_w_mis.troy", owner.X, owner.Y); 

            // Should be in Buffs/SinisterSteel.cs along with its effects:
            // var visualBuff = AddBuffHudVisual("SinisterSteel", 3.0f, 1,
            // BuffType.COMBAT_ENCHANCER, owner, 3.0f);
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var units = GetUnitsInRange(owner, 400, true);
            foreach (var unit in units)
            {
                if ((unit.Team != owner.Team) && unit is ObjAiBase && !(unit is BaseTurret))
                {
                    //MAGIC DAMAGE: 40 / 75 / 110 / 145 / 180 (+25% AP) (+ 60% BONUS AD)
                    var ap = owner.Stats.AbilityPower.Total * .25f;
                    var ad = owner.Stats.AttackDamage.FlatBonus * .6f;

                    var damage = new[] { 40, 75, 110, 145, 180 }[spell.Level - 1] + ap + ad;

                    unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

                    AddParticleTarget(owner, "katarina_w_tar.troy", unit);

                    // Possibly for non-human champions or a different skin:
                    // AddParticleTarget(owner, "katarina_w_tar_sand.troy", unit);

                    LogInfo("HasBuff: " + hasbuff);

                    if ((unit is Champion) && (hasbuff == false))
                    {
                        hasbuff = true;
                    }
                }
            }
            if (hasbuff == true)
            {
                ((ObjAiBase)target).AddBuffGameScript("KatarinaWHaste", "KatarinaWHaste", spell, 1.0f, true);
                var p = AddParticleTarget(owner, "Global_Haste.troy", owner);
                CreateTimer(1.0f, () =>
                {
                    RemoveParticle(p);
                    hasbuff = false;
                });
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

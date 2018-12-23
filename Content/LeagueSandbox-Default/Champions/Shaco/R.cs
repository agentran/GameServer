//using LeagueSandbox.GameServer;
//using LeagueSandbox.GameServer.Logic;
//using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
//using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits.AI;
//using LeagueSandbox.GameServer.Logic.GameObjects.Missiles;
//using LeagueSandbox.GameServer.Logic.GameObjects.Spells;
//using LeagueSandbox.GameServer.Logic.GameObjects.Stats;
//using LeagueSandbox.GameServer.Logic.Packets;
//using LeagueSandbox.GameServer.Logic.Scripting.CSharp;
//namespace Spells
//{
//    public class HallucinateFull : IGameScript
//    {

//        Game _game;
//        NetworkIdManager _netManager;

//        public void OnActivate(Champion owner)
//        {
//#pragma warning disable CS0618 // Type or member is obsolete
//            _game = Program.ResolveDependency<Game>();
//            _netManager = Program.ResolveDependency<NetworkIdManager>();
//#pragma warning restore CS0618 // Type or member is obsolete
//        }

//        private void OnUnHi(AttackableUnit target, bool isCrit)
//        {
//        }

//        public void OnDeactivate(Champion owner)
//        {
//        }

//        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
//        {
//            owner.ClearAllCrowdControl();
//            spell.AddProjectileTarget("BloodBoil2_cas.troy", owner);
//        }

//        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
//        {
//            //Champion shacoClone = new Champion("Shaco", _netManager.GetNewNetID(), _netManager.GetNewNetID(), new LeagueSandbox.GameServer.Logic.Content.RuneCollection(), null, _netManager.GetNewNetID());
//            //shacoClone.setPosition(owner.X + 10, owner.Y + 5);
//            AttackableUnit clone = new AttackableUnit("Shaco", new Stats(), 40, owner.X, owner.Y);
//            _game.ObjectManager.AddObject(clone);
//            //ApiFunctionManager.SetGameObjectVisibility(clone, true);
//        }

//        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
//        {

//        }

//        public void OnUpdate(double diff)
//        {
//        }

//        public void OnDaTa()
//        {
//        }

//    }
//}

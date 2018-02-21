﻿namespace Game.Modes {
    class FreeForAll : Objects.GameMode {

        private int intCurrentLeaderKills = 0;
        private int intMaximumKills = 0;
        private byte bSpawnPoint = 0;
        private bool blnFirstSpawn = true;

        private readonly object _syncObject;

        public FreeForAll()
            : base(1, "Free For All") {
            _syncObject = new object();
        }

        public override void Initilize(Entities.Room room) {
            base.Initilize(room);

            // Validate based on the room setting here.
            intCurrentLeaderKills = 0;
            intMaximumKills = 10 + (5 * room.Setting);

            Room.DownTick = 3600000;
            Initilized = true;
            FreezeTick = false;
            blnFirstSpawn = true;
        }

        public override Enums.Team Winner() {
            return Enums.Team.None;
        }

        public override void Process() {

        }

        public override bool IsGoalReached() {
            return (intCurrentLeaderKills >= intMaximumKills);
        }

        public override byte SpawnSlot()
        {
            if (!blnFirstSpawn)
            {
                bSpawnPoint++;
                if (bSpawnPoint >= 20)
                {
                    bSpawnPoint = 0;
                }
            }
            else
            {
                bSpawnPoint = 0;
                blnFirstSpawn = false;
            }

            return bSpawnPoint;
        }

        private void ProcessKill() {

        }

        protected override void OnDeath(Entities.Player killer, Entities.Player target)
        {
            if (killer != null)
            {
                if (killer.Kills > intCurrentLeaderKills)
                    intCurrentLeaderKills = killer.Kills;

                if (intCurrentLeaderKills >= intMaximumKills)
                    this.Room.EndGame(Enums.Team.Derbaran);
            }
        }

        protected override void OnObjectDestory() {
            throw new System.NotImplementedException();
        }

        public override void HandleExplosives(string[] blocks, Entities.Player p) {
            
        }

        public override byte CurrentRoundTeamA() {
            return 0;
        }

        public override byte CurrentRoundTeamB() {
            return 0;
        }

        public override ushort ScoreboardA() {
            return (ushort)intMaximumKills;
        }

        public override ushort ScoreboardB() {
            return (ushort)intCurrentLeaderKills;
        }
    }
}

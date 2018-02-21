﻿using Core.Networking;
using Game.Enums;

namespace Game.Packets {
    public class Authorization : OutPacket {
        public enum ErrorCodes : uint {
            NormalProcedure = 73030,    // Please log in using the normal procedure!
            InvalidPacket = 90100,      // Invalid Packet.
            UnregisteredUser = 90101,   // Unregistered User.
            AtLeast6Chars = 90102,      // You must type at least 6 characters .
            NicknameToShort = 90103,    // Nickname should be at least 6 charaters.
            IdInUseOtherServer = 90104, // Same ID is being used on the server.
            NotAccessible = 90105,      // Server is not accessible.
            TrainingServer = 90106,     // Trainee server is accesible until the rank of a private..
            ClanWarError = 90112,       // You cannot participate in Clan War
            LackOfResponse = 91010,     // Connection terminated because of lack of response for a while.
            ServerIsFull = 91020,       // You cannot connect. Server is full.
            InfoReqInTrafic = 91030,    // Info request are in traffic.
            AccountUpdateFailed = 91040,// Account update has failed.
            BadSynchronization = 91050, // User Info synchronization has failed.
            IdInUse = 92040,            // That ID is currently being used.
            PremiumOnly = 98010         // Available to Premium users only.
        }

        public Authorization(ErrorCodes errorCode) : base((ushort)Enums.Packets.Authorization) {
            Append((uint)errorCode);
        }

        public Authorization(Entities.User u)
            : base((ushort)Enums.Packets.Authorization) {
                Append(Core.Constants.Error_OK);
                Append(string.Concat("Gameserver", Config.SERVER_ID));
                Append(u.SessionID);
                Append(u.ID);               // User id.
                Append(u.SessionID);        // User session id.
                Append(u.Displayname);      // User Displayname (Nickname).
            // CLAN BLOCKS //
                Fill(4, -1); // TODO
            // CLAN BLOCKS
                Append((byte)u.Premium);    // Premium Type.
                Append(0);                  // Unknown.
                Append(0);                  // Unknown.
                Append(Core.LevelCalculator.GetLevelforExp(u.XP)); // User Level (based on XP).
                Append(u.XP);               // User XP.
                Append(0);                  // Unknown.
                Append(0);                  // Unknown.
                Append(u.Money);            // User Money
                Append(u.Kills);            // User Kills
                Append(u.Deaths);           // User Deaths
                Fill(5, 0);                 // 5 Unknown blocks
            // SLOT STATE //
                Append(u.Inventory.SlotState); // T = Slot Enabled, F = Slot disabled.
            // EQUIPMENT //
                Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Engineer]);    // Equipment - Engeneer
                Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Medic]);       // Equipment - Medic
                Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Sniper]);      // Equipment - Sniper
                Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Assault]);     // Equipment - Assault
                Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Heavy]);       // Equipment - Heavy
            // INVENTORY //
                Append(u.Inventory.Itemlist);
            // END INVENTORY //
                Fill(2, 0); // Two unknown blocks.
        }
    }
}

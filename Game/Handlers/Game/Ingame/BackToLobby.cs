﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Handlers.Game.Ingame {
    class BackToLobby : Networking.GameDataHandler {
        protected override void Handle() {
            if (Room.State == Enums.RoomState.Waiting) { // TODO Implement already in lobby ;)
                if (!Player.InLobby) {
                    Player.BackToLobby();
                    respond = true;
                    selfTarget = true;

                    //TODO: Lobby update if all players are back in the lobby :)
                    if (Room.Players.Values.All(p => p.InLobby)) {
                        // SEND THE ROOM UPDATE TO THE LOBBY //
                        byte roomPage = (byte)Math.Floor((decimal)(Room.ID / 8));
                        var targetList = Managers.ChannelManager.Instance.Get(Room.Channel).Users.Select(n => n.Value).Where(n => n.RoomListPage == roomPage && n.Room == null);
                        if (targetList.Count() > 0) {
                            byte[] outBuffer = new Packets.RoomUpdate(Room, true).BuildEncrypted();
                            foreach (Entities.User usr in targetList)
                                usr.Send(outBuffer);
                        }
                    }
                }
            }
        }
    }
}

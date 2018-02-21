﻿using System;
using System.Collections;

namespace Authorization.Managers {
    class ServerManager {
        private const byte MAX_SERVERS = 4;

        private Hashtable servers = new Hashtable();

        public byte Add(Entities.Server s, string name, string ip, int port, Core.Enums.ServerTypes type) {

            byte serverId = 0;

            for (byte i = 1; i <= MAX_SERVERS; i++) {
                if (!servers.ContainsKey(i)) {
                    serverId = i;
                    break;
                }
            }

            if (serverId > 0) {
                // Add server to the hashtable :p
                s.OnAuthorize(serverId, name, ip, port, type);
                servers.Add(serverId, s);
            }

            return serverId;
        }

        public void Remove(byte serverId) {
            if (servers.ContainsKey(serverId)) {
                servers.Remove(serverId);
            }
        }

        public ArrayList GetAllAuthorized() {
            ArrayList authorizedServers = new ArrayList();

            foreach(Entities.Server server in servers.Values) {
                if (server != null) {
                    if (server.Authorized) {
                        authorizedServers.Add(server);
                    }
                }
            }

            return authorizedServers;
        }

        public ArrayList GetAll() {
            return new ArrayList(servers.Values);
        }

        private static ServerManager instance = null;
        public static ServerManager Instance { get { if (instance == null) instance = new ServerManager(); return instance; } set { } }
    }
}

﻿using System.Collections.Concurrent;

using MySql.Data.MySqlClient;
using System.Collections.Generic;

using Game.Objects;
using Game.Enums;

namespace Game.Managers
{
    class MapManager
    {

        public ConcurrentDictionary<byte, Map> Maps;
        public HashSet<byte>[][] MapRotation;

        public bool Load()
        {
            ConcurrentDictionary<byte, Map> tempMaps = new ConcurrentDictionary<byte, Map>();
            HashSet<byte>[][] Rotation = new HashSet<byte>[3][];
            for (byte i = 0; i < (byte)Rotation.Length; i++)
            {
                Rotation[i] = new HashSet<byte>[3];
                Rotation[i][0] = new HashSet<byte>();
                Rotation[i][1] = new HashSet<byte>();
                Rotation[i][2] = new HashSet<byte>();
            }

            using (var connection = Databases.Game.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM map_data", connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte id = reader.GetByte("id");
                        string name = reader.GetString("name");
                        byte flagCount = reader.GetByte("flag_count");
                        string flagState = reader.GetString("flag_state");
                        string channels = reader.GetString("channels");
                        string gameMode = reader.GetString("gamemode");
                        byte minimumPremium = (byte)reader.GetUInt16("restriction_pay_type");

                        string[] channel = channels.Split(',');
                        string[] gameModes = gameMode.Split(',');
                        byte FFACount = byte.Parse(gameModes[3].ToUpper());

                        for (byte i = (byte)ChannelType.CQC - 1; i < (byte)ChannelType.AI_Channel - 1; i++)
                        {
                            if (channel[i].ToUpper() == "T")
                            {
                                switch (i)
                                {
                                    case 0:
                                        { // CQC
                                            if (gameModes[0].ToUpper() == "T")
                                            { // Explosive
                                                Rotation[i][0].Add(id);
                                            }
                                            if (FFACount > 0)
                                            { // FFA
                                                Rotation[i][1].Add(id);
                                            }
                                            if (gameModes[1].ToUpper() == "T")
                                            { // TDM
                                                Rotation[i][2].Add(id);
                                            }
                                            break;
                                        }
                                    case 1:
                                        {
                                            if (gameModes[1].ToUpper() == "T")
                                            { // TDM
                                                Rotation[i][2].Add(id);
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            if (gameModes[1].ToUpper() == "T")
                                            { // TDM
                                                Rotation[i][2].Add(id);
                                            }
                                            break;
                                        }
                                }
                            }
                        }

                        Map map = new Map(id, name, FFACount, flagCount, flagState, minimumPremium);
                        tempMaps.TryAdd(id, map);
                    }
                }
                reader.Close();

                MapRotation = Rotation;
                Maps = tempMaps;
            }

            return true;
        }

        public Map Get(byte mapId)
        {
            Map output = null;
            try
            {
                Maps.TryGetValue(mapId, out output);
            }
            catch { output = null; }
            return output;
        }

        private static MapManager instance;
        public static MapManager Instance { get { if (instance == null) { instance = new MapManager(); } return instance; } }
    }
}

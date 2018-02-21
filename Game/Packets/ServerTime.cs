﻿using System;
using System.Globalization;

namespace Game.Packets {
    class ServerTime : Core.Networking.OutPacket {
        public enum ErrorCodes : uint {
            FormatCPrank = 90010,           // Format C Drive?
            DiffrentClientVersion = 90020,  // Client version is different. Please download the patch
            ReInstallWindowsPrank = 90030,  // Reinstalling Windows.?
        }

        public ServerTime(ErrorCodes errorCode) 
            : base((ushort)Enums.Packets.ServerTime){
            Append((uint)errorCode);
        }

        public ServerTime()
            : base((ushort)Enums.Packets.ServerTime) {
            Append(Core.Constants.Error_OK);

            DateTime now = DateTime.Now.ToUniversalTime();
            int month = now.Month - 1;
            int year = now.Year - 1900;

            Append(now.ToString(@"ss\/mm\/HH\/dd") + "/" + month + "/" + year + "/" + WeekCalc(now) + "/" + now.DayOfYear + "/0");
        }

        public int WeekCalc(DateTime dt) {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
    }
}

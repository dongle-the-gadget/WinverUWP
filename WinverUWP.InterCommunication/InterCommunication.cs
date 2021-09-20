using System;

namespace WinverUWP.InterCommunication
{
    public enum InterCommunicationType
    {
        Exit,
        OSInfo
    }

    public class InterCommunicationMessage
    {

        public InterCommunicationType Type { get; set; }

        public OSInfoData OSInfo { get; set; }
    }

    public class OSInfoData
    {
        public string Edition { get; set; }

        public string Version { get; set; }

        public string InstalledOn { get; set; }

        public string Build { get; set; }

        public string Experience { get; set; }

        public string Owner { get; set; }

        public string Corporation { get; set; }

        public OSInfoData(string edition, string version, string installedOn, string build, string experience, string owner, string corporation)
        {
            Edition = edition;
            Version = version;
            InstalledOn = installedOn;
            Build = build;
            Experience = experience;
            Owner = owner;
            Corporation = corporation;
        }
    }

    public class InterCommunicationConstants
    {
        public const string MessageKey = "InterCommunication";
    }
}

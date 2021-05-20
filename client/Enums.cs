using System;

namespace Structures
{
    public enum PacketType
    {
        Spawn = 0,

        Target = 1,

        Zone = 4,

        GroundItem = 5,

        GetProcessInfo = 6,

        SetProcess = 7,

        World = 8,

        Player = 253
    }

    public enum LogLevel
    {
        Off = 0,            // Set maxLogLevel to Off and no logging occurs
        Error = 1,          // Used for exceptions and other errors
        Warning = 2,        // Used for les
        Info = 3,           // Used for information ("Loaded map XYZ")
        Debug = 4,          // Used for debug stuff, not too often though

        Default = Error,        // Used when WriteLine is called without a level
        DefaultMaxLevel = Error // Starting log level
    };

    [Flags]
    public enum DrawOptions
    {
        /// <summary>
        /// Maximum savings - all enabled
        /// </summary>
        None = 0x00000000,

        DrawMap = 0x00000001,

        Readjust = 0x00000002,

        Player = 0x00000004,

        SpotLine = 0x00000008,

        Spawns = 0x00000010,

        SpawnTrails = 0x00000020,

        GroundItems = 0x00000040,

        SpawnTimers = 0x00000080,

        DirectionLines = 0x00000100,

        SpawnRings = 0x00000200,

        GridLines = 0x00000400,

        DrawEvenLess = DrawLess          // Takes away substantially

                                      - Readjust,

        DrawLess = DrawNormal        // Takes away more uncritically things

                                      - DirectionLines

                                      - GroundItems

                                      - SpawnTimers

                                      - ZoneText,

        ZoneText = 0x00000800,

        DrawNormal = DrawMap           // Standard, nice, savings

                                      + Readjust

                                      + Player

                                      + SpotLine

                                      + Spawns

                                      + GroundItems

                                      + SpawnTimers

                                      + DirectionLines

                                      + GridLines

                                      + ZoneText,

        DrawAll = 0x0fffffff,
    }

    public enum FollowOption
    {
        None,
        Player,
        Target
    }

    [Flags]
    public enum RequestTypes
    {
        None = 0,

        //Bit Flags determining what data to send to the client
        ZONE = 0x00000001,

        PLAYER = 0x00000002,
        TARGET = 0x00000004,
        MOBS = 0x00000008,
        GROUND_ITEMS = 0x00000010,
        GET_PROCESSINFO = 0x00000020,
        SET_PROCESSINFO = 0x00000040,
        WORLD = 0x00000080
    }
}
using System;
namespace Structures
{
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
    };
}
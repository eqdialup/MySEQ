namespace Structures
{
    public enum DrawOptions
    {
        DrawMap = 0x00000001,       // Do we want to draw the map?

        Readjust = 0x00000002,       // Readjust?

        Player = 0x00000004,       // Draw the player?

        SpotLine = 0x00000008,       // Draw the shift-click line?

        Spawns = 0x00000010,       // Draw all spawns?

        SpawnTrails = 0x00000020,       // Draw the mob trails?

        GroundItems = 0x00000040,       // Draw the ground items?

        SpawnTimers = 0x00000080,       // Draw the spawn timers?

        DirectionLines = 0x00000100,       // Draw Direction lines (direction lines and such)

        SpawnRings = 0x00000200,       // Draw Shopkeeper(etc) rings around mobs

        GridLines = 0x00000400,       // Draw grid lines

        ZoneText = 0x00000800,       // Draw zone text

        //

        DrawAll = 0x0fffffff,

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

        DrawLess = DrawNormal        // Takes away more uncritically things

                              - DirectionLines

                              - GroundItems

                              - SpawnTimers

                              - ZoneText,

        DrawEvenLess = DrawLess          // Takes away substantially

                              - Readjust,

        DrawNone = 0x00000000,       // Maximum savings - all enabled
    };
}
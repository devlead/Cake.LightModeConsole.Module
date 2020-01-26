using System;

namespace Cake.LightModeConsole.Module
{
    internal static class ConsoleColorExtensions
    {
        internal static ConsoleColor Invert(this ConsoleColor color)
            => color switch
            {
                ConsoleColor.Black => ConsoleColor.White,
                ConsoleColor.Blue => ConsoleColor.DarkBlue,
                ConsoleColor.Cyan => ConsoleColor.DarkCyan,
                ConsoleColor.DarkBlue => ConsoleColor.Blue,
                ConsoleColor.DarkCyan => ConsoleColor.Cyan,
                ConsoleColor.DarkGray => ConsoleColor.Gray,
                ConsoleColor.DarkGreen => ConsoleColor.Green,
                ConsoleColor.DarkMagenta => ConsoleColor.Magenta,
                ConsoleColor.DarkRed => ConsoleColor.Red,
                ConsoleColor.DarkYellow => ConsoleColor.Yellow,
                ConsoleColor.Gray => ConsoleColor.DarkGray,
                ConsoleColor.Green => ConsoleColor.DarkGreen,
                ConsoleColor.Magenta => ConsoleColor.DarkMagenta,
                ConsoleColor.Red => ConsoleColor.DarkRed,
                ConsoleColor.White => ConsoleColor.Black,
                ConsoleColor.Yellow => ConsoleColor.DarkYellow,
                _ => color
            };
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using ITAI_Assignemnt_1.game;
using Spectre.Console;
using ValidationResult = Spectre.Console.ValidationResult;
using Rule = Spectre.Console.Rule;

namespace ITAI_Assignment_1.Game
{
    public class TerminalInterface
    {

        private int boardTopPosition;
        private  int _promptCursorTop;

        public TerminalInterface()
        {
            // When the TerminalInterface is created,
            // capture the current cursor position (the board’s start row).
            boardTopPosition = Console.CursorTop;
            _promptCursorTop = Console.CursorTop + 15;
        }

        /// <summary>
        /// Re-renders the board in place.
        /// This method moves the cursor back to the saved position
        /// and writes the new table without clearing the entire screen.
        /// </summary>
        /// <param name="state">The current KalahaState.</param>
        public void DisplayBoard(KalahaState state)
        {
            // Reset the cursor to the stored board top position.
            Console.SetCursorPosition(0, boardTopPosition);

            // Build the table from the current state.
            var table = BuildTable(state);

            // Write the updated table.
            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Builds a 2x8 table representing the Mancala (Kalaha) board.
        /// The layout is as follows:
        /// Top row: M (store at index 13) | pits 12,11,10,9,8,7 | M (store at index 6)
        /// Bottom row: M (store at index 13) | pits 0,1,2,3,4,5 | M (store at index 6)
        /// Additionally, a row is appended for current player info.
        /// </summary>
        private Table BuildTable(KalahaState state)
        {
            // Get a copy of the board array.
            int[] board = state.Board; // Board returns a copy via _board.ToArray()

            // Create the table and define columns.
            var table = new Table().Border(TableBorder.Rounded);

            // There are 8 columns: the first and last are the stores.
            table.AddColumn(new TableColumn("[green]13[/]").Centered());
            for (int i = 0; i < 6; i++)
                table.AddColumn(new TableColumn($"{12 - i}").Centered());
            table.AddColumn(new TableColumn("[green]Player 1 store[/]").Centered());

            // Top row: Player 2 side (store at index 13, then pits 12..7, then store at index 6)
            table.AddRow(
                new Markup($""),
                new Markup($"{board[12]}"),
                new Markup($"{board[11]}"),
                new Markup($"{board[10]}"),
                new Markup($"{board[9]}"),
                new Markup($"{board[8]}"),
                new Markup($"{board[7]}"),
                new Markup($"")
            );

            table.AddRow(
                new Markup($"[bold cyan]{board[13]}[/]"),
                new Markup($""),
                new Markup($""),
                new Markup($""),
                new Markup($""),
                new Markup($""),
                new Markup($""),
                new Markup($"[bold green]{board[6]}[/]")

            );


            // Bottom row: Player 1 side (store at index 13, then pits 0..5, then store at index 6)
            table.AddRow(
                new Markup($""),
                new Markup($"{board[0]}"),
                new Markup($"{board[1]}"),
                new Markup($"{board[2]}"),
                new Markup($"{board[3]}"),
                new Markup($"{board[4]}"),
                new Markup($"{board[5]}"),
                new Markup($"")
            );

            table.AddRow(
                new Markup("[dim]──────[/]"),
                new Markup("[dim]────[/]"),
                new Markup("[dim]────[/]"),
                new Markup("[dim]────[/]"),
                new Markup("[dim]────[/]"),
                new Markup("[dim]────[/]"),
                new Markup("[dim]────[/]"),
                new Markup("[dim]──────[/]")
            );

            table.AddRow(
                new Markup($"[green]Player 2 store[/]"),
                new Markup($"0"),
                new Markup($"1"),
                new Markup($"2"),
                new Markup($"3"),
                new Markup($"4"),
                new Markup($"5"),
                new Markup($"6")
            );




            return table;
        }
    
    
    
    /// <summary>
    /// Displays the player prompt in place (without creating new lines).
    /// It moves the cursor back each time to overwrite the previous prompt.
    /// </summary>
    /// <param name="state">Current game state</param>
    /// <returns>The chosen valid pit number</returns>
    public int GetUserMove(KalahaState state)
    {
        while (true)
        {
            // Move cursor back to the fixed position
            Console.SetCursorPosition(0, _promptCursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); // Clear previous input line
            Console.SetCursorPosition(0, _promptCursorTop); // Reset cursor to input position

            // Display the prompt
            AnsiConsole.Markup("[bold yellow]Select a pit:[/] ");

            // Read user input
            string? input = Console.ReadLine();
                
            // Try parsing and validating input
            if (int.TryParse(input, out int pit) && state.GetPossibleMoves().Contains(pit))
            {
                return pit; // Return valid move
            }

            // If invalid, show error message but keep the prompt in place
            Console.SetCursorPosition(0, _promptCursorTop + 1);
            AnsiConsole.MarkupLine("[bold red]Invalid pit. Try again.[/]");
        }
    }
}
}
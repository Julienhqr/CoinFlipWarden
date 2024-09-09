using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace AliveCount;

public class Plugin : BasePlugin
{
    public override string ModuleName => "Coin Flip Warden";
    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "Dowz";

    private bool commandCooldown = false; // To avoid spam commands

    // Load the commands when the plugin is loaded
    public override void Load(bool hotReload)
    {
        AddCommand("coinflip", "Coin flip command for CT only", Command_CoinFlip);
        AddCommand("cf", "Short command for coin flip", Command_CoinFlip); // Alias for the same command
    }

    // Unload the commands when the plugin is unloaded
    public override void Unload(bool hotReload)
    {
        RemoveCommand("coinflip", Command_CoinFlip);
        RemoveCommand("cf", Command_CoinFlip);
    }

    // Command to flip a coin (only available to Counter-Terrorists)
    public void Command_CoinFlip(CCSPlayerController? player, CommandInfo command)
    {
        string playerName = player.PlayerName;

        if (player == null)
        {
            command.ReplyToCommand("This command can only be used by a player.");
            return;
        }

        // Ensure the player is on the CT team
        if (player.Team != CsTeam.CounterTerrorist)
        {
            command.ReplyToCommand("Only Counter-Terrorists can use this command.");
            return;
        }

        if (commandCooldown)
        {
            command.ReplyToCommand("Wait a moment before flipping a coin again.");
            return;
        }

        // Display the initial message
        Server.PrintToChatAll($" {ChatColors.LightPurple} [COINFLIP] {ChatColors.LightBlue}{playerName}{ChatColors.Green} tossed a coin...");

        // Set command cooldown to prevent spamming
        commandCooldown = true;

        AddTimer(4.0f, () =>
        {
            // Perform the coin flip (50/50)
            var result = new Random().Next(0, 2) == 0 ? "Heads" : "Tails";

            // Print the result in the chat for everyone
            Server.PrintToChatAll($" {ChatColors.LightPurple} [COINFLIP] {ChatColors.LightBlue}{playerName}{ChatColors.Green} tossed a coin and the result is : {ChatColors.Gold}{result}");

            // Reset the cooldown
            commandCooldown = false;
        });
    }
}

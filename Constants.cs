using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetLib;

/// <summary>
/// Standard tag names to indicate which entities should be targeted by who. 
/// </summary>
public static class TargetingTags
{
    /// <summary>
    /// By default, only entities tagged with this can be targeted.
    /// </summary>
    public static readonly string NPC = "npc";

    /// <summary>
    /// Player entities. Attacked by enemies and monsters.
    /// </summary>
    public static readonly string PLAYER = "player";

    /// <summary>
    /// NPCs allied with players. Matches player targeting rules.
    /// </summary>
    public static readonly string PLAYER_FRIENDLY = "player_friendly";

    /// <summary>
    /// NPCs that have no affiliation and generally don't get into fights.
    /// </summary>
    public static readonly string NEUTRAL = "neutral";

    /// <summary>
    /// Enemies of the player. Attacked by PLAYER_FRIENDLY.
    /// </summary>
    public static readonly string PLAYER_ENEMY = "player_enemy";

    /// <summary>
    /// Neutral monsters that may attack anyone. Attacked by everyone but other monsters.
    /// </summary>
    public static readonly string MONSTER = "monster";
}
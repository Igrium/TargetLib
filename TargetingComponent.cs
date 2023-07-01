using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetLib;

public enum Relationship
{
    Like,
    Neutral,
    Hate,
    Fear
}

public enum Behavior
{
    Attack,
    Evade,
    Ignore,
    Follow
}

/// <summary>
/// Manages the target aquisition of an NPC.
/// </summary>
public partial class TargetingComponent : EntityComponent
{
    /// <summary>
    /// The relationships this NPC has with different entity groups, dictated by tags.
    /// </summary>
    public IDictionary<string, Relationship> Relationships { get; private set; } = new Dictionary<string, Relationship>();

    /// <summary>
    /// The relationships this NPC has with specific entities.
    /// </summary>
    public IDictionary<Entity, Relationship> EntityRelationships { get; private set; } = new Dictionary<Entity, Relationship>();

    /// <summary>
    /// Whether this NPC should evade other NPCs that plan to attack it and their relationship is neutral.
    /// </summary>
    public bool EvadeAttackers { get; set; } = false;

    public TargetingComponent() {
        ShouldTransmit = false;
    }

    /// <summary>
    /// Setup default relationships based on the entity's current tags.
    /// The default implementation sets up attackers as defined in the Tags class
    /// and makes everyone except monsters evade attackers.
    /// </summary>
    public virtual void SetupRelationships()
    {
        if (Entity.Tags.Has(Tags.PLAYER_FRIENDLY))
        {
            Relationships.Add(Tags.PLAYER_ENEMY, Relationship.Hate);
            Relationships.Add(Tags.MONSTER, Relationship.Hate);
        } else if (Entity.Tags.Has(Tags.PLAYER_ENEMY))
        {
            Relationships.Add(Tags.PLAYER_FRIENDLY, Relationship.Hate);
            Relationships.Add(Tags.PLAYER, Relationship.Hate);
            Relationships.Add(Tags.MONSTER, Relationship.Hate);
        } else if (Entity.Tags.Has(Tags.NEUTRAL))
        {
            Relationships.Add(Tags.MONSTER, Relationship.Hate);
        } else if (Entity.Tags.Has(Tags.MONSTER))
        {
            Relationships.Add(Tags.NPC, Relationship.Hate);
        }

        if (!Entity.Tags.Has(Tags.MONSTER))
        {
            EvadeAttackers = true;
        }
    }

    /// <summary>
    /// Whether this NPC should attack a given entity.
    /// </summary>
    /// <param name="entity">The entity to check.</param>
    /// <returns>Whether to attack.</returns>
    public bool ShouldAttack(Entity entity)
    {
        return GetBehavior(entity) == Behavior.Attack;
    }

    /// <summary>
    /// Whether this NPC should evade a given entity.
    /// </summary>
    /// <param name="entity">The entity to check.</param>
    /// <returns>Whether to evade.</returns>
    public bool ShouldEvade(Entity entity)
    {
        return GetBehavior(entity) == Behavior.Evade;
    }

    /// <summary>
    /// Whether this NPC should follow a given entity.
    /// </summary>
    /// <param name="entity">The entity to check.</param>
    /// <returns>Whether to follow.</returns>
    public bool ShouldFollow(Entity entity)
    {
        return GetBehavior(entity) == Behavior.Follow;
    }

    /// <summary>
    /// Get this NPC's behavior in relation to a given entity.
    /// </summary>
    /// <param name="entity">The entity to check.</param>
    /// <returns>The desired behavior.</returns>
    public virtual Behavior GetBehavior(Entity entity)
    {
        if (entity == Entity) return Behavior.Ignore;

        if (EntityRelationships.ContainsKey(entity))
        {
            return GetRelationBehavior(EntityRelationships[entity]);
        }

        foreach (var relation in Relationships)
        {
            if (entity.Tags.Has(relation.Key))
            {
                return GetRelationBehavior(relation.Value);
            }
        }

        //if (EvadeAttackers)
        //{
        //    TargetingComponent component;
        //    if (entity.Components.TryGet<TargetingComponent>(out component))
        //    {
        //        if (component.ShouldAttack(this.Entity)) return Behavior.Evade;
        //    }
        //}

        return Behavior.Ignore;
    }
    
    protected virtual Behavior GetRelationBehavior(Relationship relation)
    {
        switch (relation)
        {
            case Relationship.Hate:
                return Behavior.Attack;

            case Relationship.Like:
                return Behavior.Follow;

            case Relationship.Neutral:
                return Behavior.Ignore;

            case Relationship.Fear:
                return Behavior.Evade;

            default:
                return Behavior.Ignore;
        }
    }
}
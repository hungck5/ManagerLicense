using System.ComponentModel.DataAnnotations;
using EcommerceApi.Framework.Utils;

namespace EcommerceApi.Framework.Domain;

public interface IIdentity
{
    Guid Id { get; }
}

public interface IEntity : IIdentity
{
}

public interface IAggregateRoot : IEntity
{
 
}

public abstract class AggregateRoot : EntityBase, IAggregateRoot
{
    protected AggregateRoot() : this(IdHelper.GenerateId())
    {
    }

    protected AggregateRoot(Guid id)
    {
        Id = id;
        Created = DateTime.UtcNow;
    }
}

/// <inheritdoc />
/// <summary>
///     Source: https://github.com/VaughnVernon/IDDD_Samples_NET
/// </summary>
public abstract class EntityBase : IEntity
{
    protected EntityBase() : this(IdHelper.GenerateId())
    {
    }

    protected EntityBase(Guid id)
    {
        Id = id;
        Created = DateTime.UtcNow;
    }

    public DateTime Created { get; protected set; }

    public DateTime Updated { get; protected set; }

    [Key]
    public Guid Id { get; protected set; }
}
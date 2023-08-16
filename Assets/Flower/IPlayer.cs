using Flower;
using System;

public interface IPlayer : IEntityInterface
{
    public event Action<object[]> HasShoot;
}

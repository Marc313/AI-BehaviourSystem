public interface IDamagable
{
    public void TakeDamage(int _damage) { }
}

public interface IHealthUser : IDamagable
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
}

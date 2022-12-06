public interface IDamagable
{
    public void TakeDamage(int _damage) { }
}

public interface IHealthUser : IDamagable
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
}

public interface ISpottable
{
    public bool isSpotted { get; set; }
}

public interface IWeaponWielder
{
    public Weapon weapon { get; set; }
}

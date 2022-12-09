using System.Collections.Generic;
using UnityEngine;

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
    public bool isSpotted { get; }
    public List<GameObject> spotters { get; }

    public void Spot(GameObject _spotter, bool _spotted);
}

public interface IWeaponWielder
{
    public Weapon weapon { get; set; }
}

public interface IAlertable
{
    public bool isAlerted { get; }
    public void Alert();
}

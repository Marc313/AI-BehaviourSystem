using UnityEngine;

public class Attacker : MonoBehaviour
{
    private Character owner;

    private void Awake()
    {
        owner = GetComponentInParent<Character>();
    }

    public void CheckHit()
    {
        Weapon currentOwnerWeapon = owner.blackBoard.Get<Weapon>("BestWeapon");
        int attackDamage = currentOwnerWeapon == null ? 5 : currentOwnerWeapon.damage;
        float attackRange = owner.blackBoard.Get<float>("AttackRange");

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            player?.TakeDamage(owner.gameObject, attackDamage);
            //Debug.Log($"Damaged Player {player} with damage: {attackDamage}");
        }
    }
}

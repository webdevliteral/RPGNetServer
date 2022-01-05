using UnityEngine;
public class Ability : ScriptableObject
{
    [SerializeField] new private string name = "New Ability";
    public string Name => name;

    private int id;
    public int Id => id;

    private float maxDistance;
    public float MaxDistance => maxDistance;

    private int baseDamage;
    public int BaseDamage => baseDamage;

    protected float abilityCooldown;
    public float AbilityCooldown => abilityCooldown;

    protected float castTime;
    public float CastTime => castTime;

    public void InitializeAbility(int _id, string _name, float _maxDistance, 
        int _baseDamage, float _abilityCooldown, float _castTime)
    {
        id = _id;
        maxDistance = _maxDistance;
        baseDamage = _baseDamage;
        abilityCooldown = _abilityCooldown;
        castTime = _castTime;
        name = _name;
    }
}

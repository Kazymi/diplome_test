public interface IDamageable
{
    bool CanTakeDamage { get; }
    void TakeDamage(int amount);
}



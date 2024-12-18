using EntityClass;

namespace AttackClass;

public static class DoAttack {
    
    public static void Attack(this Entity Attacker, Entity Victim) {
        
        if(Victim.Stamina < Attacker.Damage) {

            int DamageToHP = Attacker.Damage - Victim.Stamina;
            Victim.Stamina = 0;
            Victim.Hp -= DamageToHP;
        } else {
            Victim.Stamina -= Attacker.Damage;
        }

    }
}

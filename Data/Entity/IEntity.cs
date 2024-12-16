using AbilityClass;

namespace EntityClass;

public interface IEntity {
    
    public int x {set; get;}
    public int y {set; get;}

    public int Hp {set; get;} 
    public int Stamina {set; get;}
    public int Damage {set; get;}

    public bool AttackType {set; get;}
    public Ability ActiveAbility {set; get;}

} 

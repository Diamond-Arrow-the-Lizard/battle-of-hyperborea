using AbilityClass;

namespace EntityClass;

public class Entity : IEntity {

    public int x {set; get;}
    public int y {set; get;}

    public int Hp {set; get;}

    private int _stamina;
    public int Stamina {
        set {
            if(value > Hp) {
                _stamina = Hp;
            } else {
                _stamina = value;
            }
        }
        get {
            return _stamina;
        }
    }

    public int Damage {set; get;}

    public bool AttackType {set; get;}

    private Ability _active_ability = new Ability();
    public Ability ActiveAbility {
        set {
            _active_ability = value;
        }
        get {
            return _active_ability;
        }
    }

    public Entity(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Entity(int x, int y, int Hp, int Stamina, int Damage, bool AttackType, Ability ActiveAbility) {
        this.x = x;
        this.y = y;
        this.Hp = Hp;
        this.Stamina = Stamina;
        this.Damage = Damage;
        this.AttackType = AttackType;
        this.ActiveAbility = ActiveAbility;
    }
}

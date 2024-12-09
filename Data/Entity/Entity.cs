namespace Entity;

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
    //TODO AttackType, Abilities

    public Entity(int x, int y) {
        this.x = x;
        this.y = y;
    }
    
    //TODO add Abilities to constructor params
    public Entity(int x, int y, int Hp, int Stamina, int Damage) {
        this.x = x;
        this.y = y;
        this.Hp = Hp;
        this.Stamina = Stamina;
        this.Damage = Damage;
    }
}

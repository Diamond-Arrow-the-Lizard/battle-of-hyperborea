namespace AbilityClass;

public class Ability : IAbility {

    public int Damage {set; get;} = 0;
    public int StaminaDrain {set; get;} = 0;

    public bool AttackType {set; get;} = false;

}

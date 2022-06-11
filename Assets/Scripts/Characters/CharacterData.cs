using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class CharacterData : ScriptableObject {
    [SerializeField] private Enums.Affinity primaryAffinity;
    [SerializeField] private Enums.Affinity secondaryAffinity;

    //Base values for a new character asset (which can be modified in the inspector)
    [SerializeField] private new string name = "Character";
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private Sprite avatar;
    [SerializeField] private List<Card> BossDeck = new List<Card>();
    [SerializeField] private int orderSpeed = 4;

    [SerializeField] public Damage basicAttack = new Damage(1,4,0);
    [SerializeField] private List<Enums.DamageType> weaknesses = new List<Enums.DamageType>();
    [SerializeField] private List<Enums.DamageType> resistances = new List<Enums.DamageType>();
    public Enums.Affinity PrimaryAffinity { get { return primaryAffinity; } }
    public Enums.Affinity SecondaryAffinity { get { return secondaryAffinity; } }
    public string Name { get { return name; } }
    public int MaxHP { get { return maxHealth; } }
    public Sprite Avatar { get { return avatar; } }

    public List<Card> Deck { get { return BossDeck; } }
    public int Speed { get { return orderSpeed; } }

    public List<Enums.DamageType> Weaknesses { get { return weaknesses; } }
    public List<Enums.DamageType> Resistances { get { return resistances; } }
}

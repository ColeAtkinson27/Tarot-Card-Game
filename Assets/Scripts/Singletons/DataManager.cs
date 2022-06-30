using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    private static DataManager instance;
    public static DataManager Instance { get { return instance; } }

    [SerializeField] private List<CharacterData> characters = new List<CharacterData>();

    [Header("Particles")]
    [SerializeField] private GameObject CorruptionCheck;
    [SerializeField] private GameObject CorruptionFail;
    [SerializeField] private GameObject ArmoredEffect;
    [SerializeField] private GameObject BleedEffect;
    [SerializeField] private GameObject BuffEffect;
    [SerializeField] private GameObject BurnEffect;
    [SerializeField] private GameObject DebuffEffect;
    [SerializeField] private GameObject RejuvenateEffect;
    [SerializeField] private GameObject ShroudEffect;
    [SerializeField] private GameObject StunEffect;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (this != instance) {
            Destroy(gameObject);
        }
    }

    public CharacterData PartyMember(int index) { return characters[index]; }

    public IEnumerator PlayParticle(Character target, Enums.CardEffects effect) {
        GameObject particles;
        switch (effect) {
            case Enums.CardEffects.Armored:
                particles = Instantiate(ArmoredEffect);
                break;
            case Enums.CardEffects.Bleed:
                particles = Instantiate(BleedEffect);
                break;
            case Enums.CardEffects.Bolster:
                particles = Instantiate(BuffEffect);
                break;
            case Enums.CardEffects.Burn:
                particles = Instantiate(BurnEffect);
                break;
            case Enums.CardEffects.Cripple:
                particles = Instantiate(DebuffEffect);
                break;
            case Enums.CardEffects.Fortify:
                particles = Instantiate(BuffEffect);
                break;
            case Enums.CardEffects.Rejuvenate:
                particles = Instantiate(RejuvenateEffect);
                break;
            case Enums.CardEffects.Shroud:
                particles = Instantiate(ShroudEffect);
                break;
            case Enums.CardEffects.Stun:
                particles = Instantiate(StunEffect);
                break;
            case Enums.CardEffects.Weaken:
                particles = Instantiate(DebuffEffect);
                break;
            default:
                yield break;
        }
        particles.transform.parent = target.ParticlePoint.transform;
        particles.transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
    }
    public IEnumerator PlayParticle(Character target, bool corCheck) {
        GameObject particles = Instantiate(CorruptionCheck);
        particles.transform.parent = target.transform;
        particles.transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(2f);
        if (!corCheck) {
            particles = Instantiate(CorruptionFail);
            particles.transform.parent = target.ParticlePoint.transform;
            particles.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

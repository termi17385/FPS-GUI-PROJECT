using FPSProject.Player.Manager;
using System.Collections;
using UnityEngine;

public class target : MonoBehaviour
{
    PlayerManager pManager;
    private bool targetArmed;
    [SerializeField] private Animator anim;
    [SerializeField] TargetRange tRange;
    private static readonly int Hit = Animator.StringToHash("Hit");

    void Awake() => pManager = FindObjectOfType<PlayerManager>();
    void Start() => StartCoroutine(HitAnimation());
    
    public void Damage(int amt)
    {
        if (targetArmed)
        {
            AudioSource sound = pManager.soundEffect;
            sound.Play();
            Debug.Log("Player Hit target for" + amt + "damage");
            
            StartCoroutine(HitMarker());
            StartCoroutine(HitAnimation());
        }
    }

    #region Hit IEnumerators
    IEnumerator HitAnimation()
    {
        targetArmed = false;
        anim.SetBool(Hit, true);
        tRange.TargetsHandler(1);
        yield return null;
    }
    IEnumerator HitMarker()
    {
        GameObject hitMarker = pManager.hitMarker;
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitMarker.SetActive(false);
    }
    #endregion
    public IEnumerator ResetTargets()
    {
        anim.SetBool(Hit, false);
        targetArmed = true;
        tRange.TargetsHandler(-1);
        yield return null;
    }
}

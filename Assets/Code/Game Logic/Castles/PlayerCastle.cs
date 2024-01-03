using UnityEngine;

public class PlayerCastle : MonoBehaviour
{
    [SerializeField]
    private Animator kingAnimator;

    public void OnDestroyed()
    {
        kingAnimator.Play("Eyes_Cry");
        kingAnimator.Play("Fear");
    }
}

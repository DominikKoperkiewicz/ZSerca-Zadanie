using UnityEngine;

public class AnimationSoundTrigger : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip coughSound;

    public void PlayCoughSound()
    {
        if (coughSound != null)
        {
            SoundManager.Instance.PlaySFX(coughSound);
        }
    }
}
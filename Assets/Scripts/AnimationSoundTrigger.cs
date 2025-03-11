using UnityEngine;

public class AnimationSoundTrigger : MonoBehaviour
{
    public void PlaySFX(AudioClip audioClip)
    {
        SoundManager.Instance.PlaySFX(audioClip);
    }
}
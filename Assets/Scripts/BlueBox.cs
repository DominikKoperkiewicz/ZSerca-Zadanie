using UnityEngine;

public class BlueBox : MonoBehaviour
{
    public BoomEffect boomEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<Player>(out Player player))
        {
            boomEffect.EnableGameObject();
        }
    }
}

using UnityEngine;

public class OrangeBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.Cough();
        }
    }
}

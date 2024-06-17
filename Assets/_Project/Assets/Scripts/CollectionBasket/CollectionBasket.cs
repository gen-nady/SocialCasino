using _Project.Scripts;
using UnityEngine;

public class CollectionBasket : MonoBehaviour
{
    public float multiplier;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        decimal amount = collision2D.gameObject.GetComponent<BallScript>().GetBetValue() * (decimal)multiplier;

        PlayerData.Amount.Value += amount;
        Debug.Log("Amount = "+ amount);
        Destroy(collision2D.gameObject);
    }
}

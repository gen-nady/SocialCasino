using UnityEngine;

public class BallScript : MonoBehaviour
{


    [SerializeField] private ForceMode2D forcemode = ForceMode2D.Impulse;

    [SerializeField] private float thrust = 1f;

    private Rigidbody2D rigidbody2D;

    private string lastHit = "";
    private decimal betAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Setup(decimal betAmount = 5){
        this.betAmount = betAmount;
    }

    public decimal GetBetValue(){
        return betAmount;
    }


    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.gameObject.CompareTag("StaticBall") && lastHit != collision2D.gameObject.name)
        {
            lastHit = collision2D.gameObject.name;
            int randomValue = Random.Range(0,100);

            rigidbody2D.AddForce(randomValue > 50 ? new Vector2(thrust, 0) : new Vector2(-thrust, 0), forcemode);
        }
    }
}

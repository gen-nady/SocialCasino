using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject ball;
    public static BallSpawner instance {get; private set;}
    
    void Awake()
    {
        instance = this;
    }


    void OnDestroy(){
        CancelInvoke();
    }

    public void SpawnBall(decimal betAmount)
    {
        GameObject ballSpawned = Instantiate(ball, transform);
        ballSpawned.transform.localPosition = Vector3.zero;
        ballSpawned.GetComponent<BallScript>().Setup( betAmount);
    }
    
}

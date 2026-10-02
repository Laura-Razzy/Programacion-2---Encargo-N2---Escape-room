using UnityEngine;

public class NoiseInteraction : MonoBehaviour
{
    private float elapsedTime, noiseDuration = 1f;
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= noiseDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyPatrol>().ChangeState(EnemyPatrol.STATE.ChasingSound);
            other.gameObject.GetComponent<EnemyPatrol>().heardNoise.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }
}

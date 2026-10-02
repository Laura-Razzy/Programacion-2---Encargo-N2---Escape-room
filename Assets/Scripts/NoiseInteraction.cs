using UnityEngine;

public class NoiseInteraction : MonoBehaviour
{
    private float elapsedTime, noiseDuration = 1f; // Estas cositas spawnean cuando el player deja un peo
    void Update() // Solo duran un segundo
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= noiseDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) // Si el enemigo choca con uno, busca donde esta el jugador.
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyPatrol>().heardNoise.position = GameObject.Find("Player").GetComponent<Transform>().position;
            other.gameObject.GetComponent<EnemyPatrol>().ChangeState(EnemyPatrol.STATE.ChasingSound);
            Destroy(gameObject);
        }
    }
}
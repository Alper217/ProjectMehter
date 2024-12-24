using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public Rigidbody diceRigidbody;
    public float forceMultiplier = 30f;
    public float torqueMultiplier = 30f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollDice();
        }
    }

    void RollDice()
    {
        diceRigidbody.rotation = Random.rotation;
        diceRigidbody.velocity = Vector3.zero;
        diceRigidbody.angularVelocity = Vector3.zero;

        Vector3 randomForce = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1.5f),
            Random.Range(-1f, 1f)
        ).normalized * forceMultiplier;

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * torqueMultiplier;

        diceRigidbody.AddForce(randomForce, ForceMode.Impulse);
        diceRigidbody.AddTorque(randomTorque, ForceMode.Impulse);
    }
}


using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public GameObject EnemyInstance;

    public Vector2 DisplacementPosition;

    void Update()
    {
        CheckIfEnemyDead();

        // Only update when the enemy is not yet deleted/marked for deletion.
        if (EnemyInstance != null)
        {
            slider.value = EnemyInstance.GetComponent<Health>().health;

            gameObject.transform.position = Camera.main.WorldToScreenPoint((Vector2)EnemyInstance.transform.position + DisplacementPosition);
        }
    }


    void CheckIfEnemyDead()
    {
        // If enemy dies/gets deleted, delete this also.
        if (EnemyInstance == null)
        {
            Destroy(gameObject);
        }
    }
}

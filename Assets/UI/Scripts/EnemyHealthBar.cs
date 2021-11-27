
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public GameObject EnemyInstance;
    public GameObject EnemyUIDummy;
    public Health enemyHealth;

    public Vector2 DisplacementPosition;

    private void Start() {
        EnemyUIDummy = EnemyInstance.transform.Find("UIDummy").gameObject;
        enemyHealth = EnemyInstance.GetComponentInParent<Health>();
    }

    void Update()
    {
        CheckIfEnemyDead();

        // Only update when the enemy is not yet deleted/marked for deletion.
        if (EnemyInstance != null)
        {
            slider.value = enemyHealth.health;

            gameObject.transform.position = Camera.main.WorldToScreenPoint((Vector2)EnemyUIDummy.transform.position + DisplacementPosition);
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

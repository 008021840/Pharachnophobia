using UnityEngine;

public class ClickableMonster : MonoBehaviour
{
    public enum MonsterType
    {
        Ghost,
        Spider,
        Demon
    }

    [Header("Monster Type")]
    public MonsterType monsterType;

    private void OnMouseDown()
    {
        if (FlashlightModeManager.Instance == null)
            return;

        bool blueMode = FlashlightModeManager.Instance.IsBlueMode;

        if (monsterType == MonsterType.Demon)
        {
            if (!blueMode)
            {
                Debug.Log("You need the blue flashlight to click the demon!");
                return;
            }

            DestroyMonster();
        }
        else
        {
            if (blueMode)
            {
                Debug.Log("Blue flashlight cannot remove ghosts or spiders!");
                return;
            }

            DestroyMonster();
        }
    }

    private void DestroyMonster()
    {
        gameObject.SetActive(false);
    }
}
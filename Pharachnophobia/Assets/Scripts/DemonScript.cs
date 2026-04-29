using System.Collections;
using UnityEngine;

public class DemonScript : MonoBehaviour
{
    public AudioSource source;
    public AudioClip DemonLaugh;
    [SerializeField]
    Sprite[] sprites;
    int spriteNum = 0;
    GameObject sanityManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChangeSprite());
        sanityManager = GameObject.FindWithTag("SanityManager");
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[spriteNum];

    }
    private void OnMouseOver()
    {
        Debug.Log("overlapping demon");
        if (Input.GetMouseButtonDown(1) && spriteNum == 2)
        {
            source.PlayOneShot(DemonLaugh);
            spriteNum = 0;
            Debug.Log("clicked demon");
            
        }
    }
    IEnumerator ChangeSprite()
    {
        float time;
        if (spriteNum >= 2)
        {
            time = Random.Range(5f, 15f);
        }
       else time = Random.Range(0f, 10f);
        yield return new WaitForSeconds(time);
        if(spriteNum < 2)
        {
            spriteNum = spriteNum + 1;
            
        }
        else if(spriteNum >= 2)
        {
            sanityManager.GetComponent<SanityManager>().LoseGame();
        }
        
        StartCoroutine(ChangeSprite());

    }
}

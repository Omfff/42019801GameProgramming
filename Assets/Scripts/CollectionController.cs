using Boo.Lang;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemType type;
    public string name;
    public string description;
    public Sprite itemImage;
}

public enum ItemType
{
    Buff,
    Storage,
    Weapon
}

public class CollectionController : MonoBehaviour
{

    public Item item;
    public int healthChange;
    public float moveSpeedChange;
    public float bulletSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    public int bulletCountChange;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch (this.item.type)
            {
                case ItemType.Buff:
                    GameController.HealPlayer(healthChange);
                    GameController.Buff(moveSpeedChange, attackSpeedChange, bulletSizeChange, bulletCountChange, bulletSpeedChange);
                    Destroy(gameObject);
                    break;
                case ItemType.Storage:
                    GameController.AddItems(item);
                    Destroy(gameObject);
                    break;
                case ItemType.Weapon:
                    GameObject prefab = GameController.instance.prefabList[Random.Range(0, 4)];
                    Instantiate(prefab, transform.position, transform.rotation);
                    Destroy(gameObject);
                    break;
            }

        }
    }
}

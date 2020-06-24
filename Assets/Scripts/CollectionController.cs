using UnityEngine;

[System.Serializable]
public class Item
{
<<<<<<< HEAD
=======
    public ItemType type;
>>>>>>> omf
    public string name;
    public string description;
    public Sprite itemImage;
}

<<<<<<< HEAD
=======
public enum ItemType
{
    Buff,
    Storage,
    Weapon
}

>>>>>>> omf
public class CollectionController : MonoBehaviour
{

    public Item item;
<<<<<<< HEAD
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
=======
    public int healthChange;
    public float moveSpeedChange;
    public float bulletSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    public int bulletCountChange;
>>>>>>> omf
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
<<<<<<< HEAD
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
=======
        //Destroy(GetComponent<PolygonCollider2D>());
        //gameObject.AddComponent<PolygonCollider2D>();
>>>>>>> omf
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
<<<<<<< HEAD
            PlayerController.collectedAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.instance.UpdateCollectedItems(this);
            Destroy(gameObject);
=======
            switch (this.item.type)
            {
                case ItemType.Buff:
                    GameController.HealPlayer(healthChange);
                    Debug.Log(bulletCountChange);
                    GameController.Buff(moveSpeedChange, attackSpeedChange, bulletSizeChange, bulletCountChange, bulletSpeedChange);
                    Destroy(gameObject);
                    break;
                case ItemType.Storage:
                    GameController.AddItems(item);
                    Destroy(gameObject);
                    break;
                case ItemType.Weapon:
                    GameObject prefab = GameController.instance.prefabList[Random.Range(0, 3)];
                    Instantiate(prefab, transform.position, transform.rotation);
                    Destroy(gameObject);
                    break;
            }

>>>>>>> omf
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFun : MonoBehaviour
{
    public static UIFun instance;  //这个类的实例

    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private GridLayoutGroup items;
    [SerializeField] private GameObject[] InvisibleWalls;

    private void Awake()
    {
        //++++++++++++++++单例模式的基本写法++++++++++++++++++++
        if (instance != null)
        {
            Destroy(gameObject);  // 销毁新创建的重复实例
            return;
        }
        instance = this;
        // DontDestroyOnLoad(gameObject);  //防止切换场景时被销毁
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++
    }

    public void showMianPlayer()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
            return;
        SpriteRenderer first = null;
        foreach (var r in spriteRenderers)
        {
            if (r != null)
            {
                first = r;
                break;
            }
        }
        if (first == null)
            return;
        bool newEnabled = !first.enabled;
        foreach (var r in spriteRenderers)
        {
            if (r != null)
                r.enabled = newEnabled;
        }
    }

    public void GetOneItem()
    {
        if (items == null)
            return;
    
        Transform root = items.transform;
        for (int i = 0; i < root.childCount; i++)
        {
            Image img = root.GetChild(i).GetComponent<Image>();
            if (img == null)
                continue;
    
            Color c = img.color;
            if (c.a < 1f)
            {
                c.a = 1f;
                img.color = c;
                return;
            }
        }
    }

    public void OpenInvisibleWall()
    {
        GameManager ins = GameManager.Instance;
        if (InvisibleWalls[ins.CurrentScore - 1] == null)
            return;
    
        Transform root = InvisibleWalls[ins.CurrentScore - 1].transform;
        root.GetComponent<Collider2D>().isTrigger = true;
    }


}

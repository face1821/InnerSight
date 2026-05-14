using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFun : MonoBehaviour
{
    public static UIFun instance; //这个类的实例

    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private GridLayoutGroup items;
    [SerializeField] private GameObject[] InvisibleWalls;

    private void Awake()
    {
        //++++++++++++++++单例模式的基本写法++++++++++++++++++++
        if (instance != null)
        {
            Destroy(gameObject); // 销毁新创建的重复实例
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

        foreach (var r in spriteRenderers)
        {
            r.enabled = !r.enabled;
            r.color = new Color(r.color.r, r.color.g, r.color.b, 1f);
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
        if (ins.CurrentScore >= ins.ScoreArr[ins.CurrentLevel - 1])
            return;

        if (InvisibleWalls[ins.CurrentScore - 1] == null)
            return;

        Transform root = InvisibleWalls[ins.CurrentScore - 1].transform;
        root.GetComponent<Collider2D>().isTrigger = true;
    }

    public void ExittGame() { Application.Quit(); }
}
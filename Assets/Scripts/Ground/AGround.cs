using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 碎裂 A 型地面：玩家从上方落到该 Tilemap 上后，与当前块在父级 Grid 上「四邻格子」相连的其它 AGround 会作为一整组，
/// 先经历若干秒的透明度渐隐（通过 Tilemap.color），再关闭碰撞让玩家可掉落，等待后再恢复初始颜色与碰撞。
/// 要求：与本脚本挂在同一物体上的 Tilemap；多块相连判定依赖父物体链上的 Grid。
/// </summary>
[RequireComponent(typeof(Tilemap))]
public class AGround : MonoBehaviour
{
    [Header("时间")]
    [Tooltip("从踩上到完全透明（alpha 乘子为 0）的渐隐时长（秒）。")]
    [SerializeField] private float fadeSeconds = 3f;

    [Tooltip("平台已消失（碰撞关闭）后，等待多少秒再恢复原状。")]
    [SerializeField] private float goneSeconds = 2f;

    [Header("落地判定")]
    [Tooltip("接触法线向上分量 ≥ 该值才视为踩在顶面，用于减少侧面蹭到就触发碎裂。")]
    [SerializeField] private float landingNormalYMin = 0.55f;
    

    /// <summary>本物体上的 Tilemap，用于读格子、改整块染色（含透明度）。</summary>
    private Tilemap _tilemap;

    /// <summary>与本物体上所有 2D 碰撞体缓存，碎裂时统一开关 enabled。</summary>
    private readonly List<Collider2D> _colliders2D = new List<Collider2D>();

    /// <summary>碎裂流程开始前记录的 Tilemap.color，用于恢复时还原。</summary>
    private Color _tilemapBaseColor;

    /// <summary>本块是否已参与当前一轮碎裂（防止重复开协程）。</summary>
    private bool _crumbleActive;

    /// <summary>在统一 Grid 坐标下用于四邻扩展的四个方向。</summary>
    private static readonly Vector3Int[] Neighbors4 =
    {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
    };

    /// <summary>
    /// Unity 生命周期：在场景加载时抓取 Tilemap、缓存所有 Collider2D，并记录 Tilemap 初始颜色供之后恢复。
    /// </summary>
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        GetComponents(_colliders2D);
        if (_tilemap != null)
            _tilemapBaseColor = _tilemap.color;
    }

    /// <summary>
    /// Unity 物理回调：当别的碰撞体「刚进入」与本物体接触时调用。
    /// 用于检测玩家是否从可接受的角度落到平台上；通过校验后找出相连的一组 AGround 并启动碎裂协程。
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_crumbleActive)
            return;
        if (!IsPlayerObject(collision.gameObject))
            return;
        if (!HasLandingContact(collision))
            return;

        var cluster = BuildConnectedCluster(this);
        foreach (var ag in cluster)
            ag._crumbleActive = true;

        Debug.Log("cluster count = " + cluster.Count + " names: " + string.Join(", ", cluster.ConvertAll(a => a.name)));
        StartCoroutine(CrumbleRoutine(cluster));
    }

    /// <summary>
    /// 判断碰撞对象的层级里是否存在玩家（Player 组件），用于过滤非玩家物体触发的碰撞。
    /// </summary>
    /// <param name="go">碰撞到的根或子物体上的某个 GameObject。</param>
    /// <returns>若在父级链上找到 Player 则返回 true。</returns>
    private static bool IsPlayerObject(GameObject go)
    {
        return go.GetComponentInParent<Player>() != null;
    }

    /// <summary>
    /// 根据接触点法线判断这次碰撞是否更像「踩在平台顶面」而不是侧面顶撞。
    /// </summary>
    /// <param name="collision">本次 2D 碰撞信息。</param>
    /// <returns>若任一接触点法线向上分量足够大则视为落地，返回 true。</returns>
    private bool HasLandingContact(Collision2D collision)
    {
        int count = collision.contactCount;
        for (int i = 0; i < count; i++)
        {
            var ct = collision.GetContact(i);
            if (Mathf.Abs(ct.normal.y) >= landingNormalYMin)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 从被踩中的这一块出发，在父级 Grid 的格子坐标下做广度优先搜索，收集所有「四邻相接」的 AGround，组成一整组一起碎裂。
    /// 若没有 Grid，则无法定义跨物体的格子邻接，此时只返回起点自身，避免误伤全场景其它平台。
    /// </summary>
    /// <param name="start">玩家本次踩中的 AGround。</param>
    /// <returns>与 start 在 Grid 上四邻连通的所有 AGround 列表。</returns>
    private static List<AGround> BuildConnectedCluster(AGround start)
    {
        Grid grid = start.GetComponentInParent<Grid>();
        AGround[] all = Object.FindObjectsOfType<AGround>(false);

        if (grid == null)
        {
            Debug.LogWarning("[AGround] 父物体链上未找到 Grid，无法判断「相连」，仅碎裂当前这一块: " + start.name, start);
            return new List<AGround> { start };
        }

        var cellSets = new Dictionary<AGround, HashSet<Vector3Int>>();
        var cellToOwners = new Dictionary<Vector3Int, List<AGround>>();

        foreach (AGround ag in all)
        {
            if (ag == null || ag._tilemap == null)
                continue;

            var set = new HashSet<Vector3Int>();
            BoundsInt b = ag._tilemap.cellBounds;
            foreach (Vector3Int p in b.allPositionsWithin)
            {
                if (!ag._tilemap.HasTile(p))
                    continue;
                Vector3Int gc = grid.WorldToCell(ag._tilemap.GetCellCenterWorld(p));
                set.Add(gc);
                if (!cellToOwners.TryGetValue(gc, out List<AGround> owners))
                {
                    owners = new List<AGround>();
                    cellToOwners[gc] = owners;
                }
                if (!owners.Contains(ag))
                    owners.Add(ag);
            }
            cellSets[ag] = set;
        }

        if (!cellSets.TryGetValue(start, out HashSet<Vector3Int> startCells) || startCells.Count == 0)
            return new List<AGround> { start };

        var visited = new HashSet<AGround> { start };
        var queue = new Queue<AGround>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            AGround g = queue.Dequeue();
            if (!cellSets.TryGetValue(g, out HashSet<Vector3Int> cells))
                continue;

            foreach (Vector3Int c in cells)
            {
                foreach (Vector3Int dir in Neighbors4)
                {
                    Vector3Int n = c + dir;
                    if (!cellToOwners.TryGetValue(n, out List<AGround> owners))
                        continue;
                    for (int i = 0; i < owners.Count; i++)
                    {
                        AGround h = owners[i];
                        if (h == null || visited.Contains(h))
                            continue;
                        visited.Add(h);
                        queue.Enqueue(h);
                    }
                }
            }
        }
        return new List<AGround>(visited);
    }

    /// <summary>
    /// 碎裂流程协程：整组平台在 fadeSeconds 内把 Tilemap 的 alpha 乘子从 1 收到 0；
    /// 然后关闭整组所有 Collider2D；等待 goneSeconds；再重新启用碰撞并把 Tilemap 颜色恢复为 Awake 时记录的值；
    /// 最后清除各成员上的「正在碎裂」标记，以便玩家下次再踩时重新触发。
    /// </summary>
    /// <param name="group">本次需要同步碎裂与恢复的一组 AGround。</param>
    private IEnumerator CrumbleRoutine(List<AGround> group)
    {
        fadeSeconds = Mathf.Max(0.01f, fadeSeconds);
        goneSeconds = Mathf.Max(0f, goneSeconds);

        float t = 0f;
        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeSeconds);
            float a = Mathf.Lerp(1f, 0f, k);
            ApplyGroupAlpha(group, a);
            yield return null;
        }

        ApplyGroupAlpha(group, 0f);
        SetGroupCollidersEnabled(group, false);

        if (goneSeconds > 0f)
            yield return new WaitForSeconds(goneSeconds);

        SetGroupCollidersEnabled(group, true);
        RestoreGroupVisual(group);

        foreach (AGround ag in group)
        {
            if (ag != null)
                ag._crumbleActive = false;
        }
    }

    /// <summary>
    /// 对一组 AGround 同步设置 Tilemap 的透明度：在初始颜色的基础上，把 alpha 乘以传入的乘子（0 完全透明，1 为初始 alpha）。
    /// </summary>
    /// <param name="group">同一碎裂组内的多个 AGround。</param>
    /// <param name="alphaMultiplier">与初始颜色 alpha 相乘的系数，范围建议 0～1。</param>
    private static void ApplyGroupAlpha(List<AGround> group, float alphaMultiplier)
    {
        for (int i = 0; i < group.Count; i++)
        {
            AGround ag = group[i];
            if (ag == null || ag._tilemap == null)
                continue;
            Color c = ag._tilemapBaseColor;
            c.a = ag._tilemapBaseColor.a * alphaMultiplier;
            ag._tilemap.color = c;
        }
    }

    /// <summary>
    /// 将组内每个 Tilemap 的颜色恢复为 Awake 时缓存的初始值（碎裂流程结束后的视觉还原）。
    /// </summary>
    /// <param name="group">同一碎裂组内的多个 AGround。</param>
    private static void RestoreGroupVisual(List<AGround> group)
    {
        for (int i = 0; i < group.Count; i++)
        {
            AGround ag = group[i];
            if (ag == null || ag._tilemap == null)
                continue;
            ag._tilemap.color = ag._tilemapBaseColor;
        }
    }

    /// <summary>
    /// 批量开关组内每个物体上缓存的所有 Collider2D：碎裂中段设为 false 以去掉碰撞，恢复时再设为 true。
    /// </summary>
    /// <param name="group">同一碎裂组内的多个 AGround。</param>
    /// <param name="enabled">是否启用碰撞体。</param>
    private static void SetGroupCollidersEnabled(List<AGround> group, bool enabled)
    {
        for (int i = 0; i < group.Count; i++)
        {
            AGround ag = group[i];
            if (ag == null)
                continue;
            for (int j = 0; j < ag._colliders2D.Count; j++)
            {
                Collider2D col = ag._colliders2D[j];
                if (col != null)
                    col.enabled = enabled;
            }
        }
    }
}
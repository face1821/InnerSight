using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通碰撞体版「碎裂 A 型地面」：玩家进入指定的顶层 Trigger 后，经过 fadeSeconds 渐隐，
/// 再关闭碰撞与渲染（消失），goneSeconds 后再恢复。需在 Inspector 指定上层用于检测的 Collider2D（Is Trigger）。
/// </summary>
public class newAGround : MonoBehaviour
{
    [Header("时间")]
    [Tooltip("从触发到完全透明（alpha 乘子为 0）的渐隐时长（秒）。")]
    [SerializeField] private float fadeSeconds = 3f;

    [Tooltip("平台已消失（碰撞关闭）后，等待多少秒再恢复原状。")]
    [SerializeField] private float goneSeconds = 2f;

    [Header("检测")]
    [Tooltip("平台上层、用于判断玩家踩上的触发器（须勾选 Is Trigger）。")]
    [SerializeField] private Collider2D trigger;

    private readonly List<Collider2D> _colliders2D = new List<Collider2D>();

    private struct RendererCache
    {
        public Renderer Renderer;
        public Color BaseColor;
        public bool WasEnabled;
    }

    private readonly List<RendererCache> _renderers = new List<RendererCache>();

    private bool _crumbleActive;

    private void Awake()
    {
        GetComponentsInChildren(true, _colliders2D);

        foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
        {
            if (r == null)
                continue;
            _renderers.Add(new RendererCache
            {
                Renderer = r,
                BaseColor = ReadRendererColor(r),
                WasEnabled = r.enabled
            });
        }

        if (trigger == null)
        {
            Debug.LogWarning("[newAGround] 未指定顶层触发器 Trigger，平台不会因玩家踩上而消失。", this);
            return;
        }

        if (!trigger.isTrigger)
            Debug.LogWarning("[newAGround] 指定的 Collider2D 建议勾选 Is Trigger。", trigger);

        var relay = trigger.gameObject.GetComponent<NewAGroundTriggerRelay>();
        if (relay == null)
            relay = trigger.gameObject.AddComponent<NewAGroundTriggerRelay>();
        relay.Bind(this);
    }

    private static Color ReadRendererColor(Renderer r)
    {
        if (r is SpriteRenderer sr)
            return sr.color;
        return r.material.color;
    }

    private static void WriteRendererColor(Renderer r, Color c)
    {
        if (r is SpriteRenderer sr)
            sr.color = c;
        else
            r.material.color = c;
    }

    /// <summary>由挂在触发器物体上的 Relay 调用。</summary>
    internal void OnPlayerEnteredTopTrigger(Collider2D other)
    {
        if (_crumbleActive)
            return;
        if (other.GetComponentInParent<Player>() == null)
            return;

        _crumbleActive = true;
        StartCoroutine(CrumbleRoutine());
    }

    private IEnumerator CrumbleRoutine()
    {
        fadeSeconds = Mathf.Max(0.01f, fadeSeconds);
        goneSeconds = Mathf.Max(0f, goneSeconds);

        float t = 0f;
        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeSeconds);
            float a = Mathf.Lerp(1f, 0f, k);
            ApplyAlphaMultiplier(a);
            yield return null;
        }

        ApplyAlphaMultiplier(0f);
        SetRenderersEnabled(false);
        SetCollidersEnabled(false);

        if (goneSeconds > 0f)
            yield return new WaitForSeconds(goneSeconds);

        SetCollidersEnabled(true);
        RestoreVisual();

        _crumbleActive = false;
    }

    private void ApplyAlphaMultiplier(float alphaMultiplier)
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
            RendererCache entry = _renderers[i];
            if (entry.Renderer == null)
                continue;
            Color c = entry.BaseColor;
            c.a = entry.BaseColor.a * alphaMultiplier;
            WriteRendererColor(entry.Renderer, c);
        }
    }

    private void SetRenderersEnabled(bool enabled)
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
            Renderer r = _renderers[i].Renderer;
            if (r != null)
                r.enabled = enabled;
        }
    }

    private void RestoreVisual()
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
            RendererCache entry = _renderers[i];
            if (entry.Renderer == null)
                continue;
            WriteRendererColor(entry.Renderer, entry.BaseColor);
            entry.Renderer.enabled = entry.WasEnabled;
        }
    }

    private void SetCollidersEnabled(bool enabled)
    {
        for (int i = 0; i < _colliders2D.Count; i++)
        {
            Collider2D col = _colliders2D[i];
            if (col != null)
                col.enabled = enabled;
        }
    }
}

/// <summary>
/// 挂在「顶层触发器」所在物体上，把 OnTriggerEnter2D 转给 newAGround（因 Unity 只在碰撞体所在物体上派发触发消息）。
/// </summary>
[DisallowMultipleComponent]
public class NewAGroundTriggerRelay : MonoBehaviour
{
    private newAGround _owner;

    public void Bind(newAGround owner)
    {
        _owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_owner != null)
            _owner.OnPlayerEnteredTopTrigger(other);
    }
}
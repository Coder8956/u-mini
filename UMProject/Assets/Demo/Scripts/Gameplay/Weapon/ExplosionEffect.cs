using UnityEngine;

/// <summary>
/// 爆炸特效控制器
/// 特效播放完成后自动销毁所在GameObject。
/// 通过计算所有 ParticleSystem 和 AudioSource 的最长持续时间来设定定时销毁。
/// </summary>
public class ExplosionEffect : MonoBehaviour
{
    // ==================== 生命周期 ====================

    private void Start()
    {
        float maxDuration = CalculateMaxDuration();
        Destroy(gameObject, maxDuration);
    }

    // ==================== 逻辑 ====================

    /// <summary>
    /// 计算所有粒子和音效的最长持续时间（秒）
    /// 粒子持续时间 = main.duration + main.startLifetime
    /// </summary>
    private float CalculateMaxDuration()
    {
        float maxDuration = 0f;

        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps == null)
                continue;

            if (!ps.main.playOnAwake)
                ps.Play();

            float duration = ps.main.duration + ps.main.startLifetime.constantMax;
            if (duration > maxDuration)
                maxDuration = duration;
        }

        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audio in audioSources)
        {
            if (audio == null || audio.clip == null)
                continue;

            if (!audio.playOnAwake)
                audio.Play();

            if (audio.clip.length > maxDuration)
                maxDuration = audio.clip.length;
        }

        // 兜底：至少存活 0.1 秒
        return Mathf.Max(maxDuration, 0.1f);
    }
}

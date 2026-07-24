using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class PlayerDamageController : DamageController
{
    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    [SerializeField]
    private GameObject gameOverScreen;

    private float timeScale = 1;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        Assert.IsNotNull(gameOverScreen);
    }

    /// <inheritdoc />
    protected override void Internal_Damage(float amount, EffectSource source)
    {
        base.Internal_Damage(amount, source);
        impulseSource.GenerateImpulse();
    }

    /// <inheritdoc />
    protected override void Internal_Destroy(EffectSource source)
    {
        base.Internal_Destroy(source);
        gameOverScreen.SetActive(true);
        StartCoroutine(FadeOut(5f));
    }
    
    private IEnumerator FadeOut(float duration)
    {
        while (timeScale > 0)
        {
            timeScale -= 0.01f;
            yield return new WaitForSecondsRealtime(duration * 0.01f);
        }
    }
}

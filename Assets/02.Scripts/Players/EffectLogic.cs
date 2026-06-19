using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _02.Scripts.Players
{
    public class EffectLogic : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO playerChannel;
        [SerializeField] private EventChannelSO enemyChannel;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private VolumeProfile volumeProfile;
        
        [Header("Player Hit Shake")]
        [SerializeField] private float playerShakeDuration = 0.3f;
        [SerializeField] private float playerShakeForce = 1f;
        [SerializeField] private Vector3 playerShakeDirection = new Vector3(0, 1, 0);

        [Header("Enemy Hit Shake")]
        [SerializeField] private float enemyShakeDuration = 0.2f;
        [SerializeField] private float enemyShakeForce = 0.5f;
        [SerializeField] private Vector3 enemyShakeDirection = new Vector3(1, 0, 0);

        private Vignette _vignette;
        private ColorAdjustments _colorAdjustments;
        public void Initialize(ModuleOwner owner)
        {
            playerChannel.AddListener<TakeDamageEvent>(HandlePlayerTakeDamage);
            enemyChannel.AddListener<TakeDamageEvent>(HandleEnemyTakeDamage);
            playerChannel.AddListener<HealthChangedEvent>(HandlePlayerHealthChange);
            _vignette = volumeProfile.TryGet<Vignette>(out var vignette) ? vignette : new Vignette();
            _colorAdjustments = volumeProfile.TryGet<ColorAdjustments>(out var colorAdjustments) ? colorAdjustments : new ColorAdjustments();
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<TakeDamageEvent>(HandlePlayerTakeDamage);
            enemyChannel.RemoveListener<TakeDamageEvent>(HandleEnemyTakeDamage);
            playerChannel.RemoveListener<HealthChangedEvent>(HandlePlayerHealthChange);
        }

        private void HandlePlayerTakeDamage(TakeDamageEvent evt)
        {
            impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
            PlayCameraShake(evt.Value, playerShakeDuration, playerShakeForce, playerShakeDirection);
            if (evt.Value > 7)
            PlayRedFlash();
        }

        private void HandleEnemyTakeDamage(TakeDamageEvent evt)
        {
            impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Bump;
            PlayCameraShake(1, enemyShakeDuration, enemyShakeForce, enemyShakeDirection);
        }

        private void PlayCameraShake(int amount, float duration, float force, Vector3 direction)
        {
            float multiplier = amount <= 3 ? 0.5f : amount <= 7 ? 1f : 1.5f;
            impulseSource.DefaultVelocity = direction * force * multiplier;
            impulseSource.GenerateImpulse();
        }

        private void HandlePlayerHealthChange(HealthChangedEvent evt)
        {
            if (_vignette == null) return;
            float healthRatio = (float)evt.CurrentHealth / Player.Instance.MaxHealth;
            _vignette.intensity.value = Mathf.Lerp(0.5f, 0f, healthRatio);
        }
        
        private void PlayRedFlash()
        {
            if (_colorAdjustments == null) return;
            DOTween.Kill("DamageFlash");
            _colorAdjustments.colorFilter.value = Color.red;
            DOTween.To(
                () => _colorAdjustments.colorFilter.value,
                x => _colorAdjustments.colorFilter.value = x,
                Color.white,
                0.5f
            ).SetId("DamageFlash");
        }
    }
}
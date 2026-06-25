using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _02.Scripts
{
    public class GameStartSeq : MonoBehaviour
    {
        [SerializeField] private VolumeProfile volumeProfile;
        [SerializeField] private TextMeshPro questText;
        [SerializeField] private TextMeshPro gameTutorialText;
        [SerializeField] private List<string> tutorialText;
        [SerializeField] private CardSelectionManager cardSelectionManager;

        [Header("Blur")]
        [SerializeField] private float blurFadeDuration = 1.5f;
        [SerializeField] private float focusDistance = 100f;

        private DepthOfField _depthOfField;
        private Tween _blurTween;

        private float _initialGaussianStart;
        private float _initialGaussianEnd;
        private bool _initialActive;

        private int count = 0;
        private void Awake()
        {
            volumeProfile.TryGet(out _depthOfField);

            if (_depthOfField != null)
            {
                _initialGaussianStart = _depthOfField.gaussianStart.value;
                _initialGaussianEnd = _depthOfField.gaussianEnd.value;
                _initialActive = _depthOfField.active;
            }
        }

        public void ChangeQuestText(string quest)
        {
            questText.text = quest;
        }

        public void NextTutorial()
        {
            if (count >= tutorialText.Count)
            {
                gameTutorialText.text = "";
                return;
            }
            
            gameTutorialText.text = tutorialText[count];
            count++;
        }
        
        public void OnStartButton()
        {
            FadeOutBlur();
            NextTutorial();
            cardSelectionManager.StartGameCardSelection();
        }

        private void FadeOutBlur()
        {
            if (_depthOfField == null) return;

            _blurTween?.Kill();
            _depthOfField.active = true;
            _blurTween = DOTween.To(
                    () => _depthOfField.gaussianStart.value,
                    x =>
                    {
                        _depthOfField.gaussianStart.value = x;
                        _depthOfField.gaussianEnd.value = x;
                    },
                    focusDistance,
                    blurFadeDuration)
                .SetEase(Ease.InOutSine);
            //.OnComplete(() => _depthOfField.active = false);
        }

        private void OnDestroy()
        {
            _blurTween?.Kill();
Debug.Log("OnDestroy");
            if (_depthOfField == null) return;
            _depthOfField.gaussianStart.value = _initialGaussianStart;
            _depthOfField.gaussianEnd.value = _initialGaussianEnd;
            _depthOfField.active = _initialActive;
        }
    }
}
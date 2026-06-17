using System;
using System.Collections;
using _02.Scripts.CoreSystem;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.Stage
{
    public class StageTransitionAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject[] inProgressObjects;
        [SerializeField] private GameObject[] stageEndObjects;

        [SerializeField] private float hiddenYOffset = 1000f;
        [SerializeField] private float moveDuration = 0.4f;
        [SerializeField] private float delayBetween = 0.1f;

        private Vector3[] _inProgressOriginalPos;
        private Vector3[] _stageEndOriginalPos;

        private void Awake()
        {
            _inProgressOriginalPos = new Vector3[inProgressObjects.Length];
            for (int i = 0; i < inProgressObjects.Length; i++)
                _inProgressOriginalPos[i] = inProgressObjects[i].transform.localPosition;

            _stageEndOriginalPos = new Vector3[stageEndObjects.Length];
            for (int i = 0; i < stageEndObjects.Length; i++)
                _stageEndOriginalPos[i] = stageEndObjects[i].transform.localPosition;

            foreach (var obj in inProgressObjects)
                obj.transform.localPosition += Vector3.up * hiddenYOffset;
        }

        public void PlayStageStartTransition(Action onComplete)
        {
            StartCoroutine(StageStartRoutine(onComplete));
        }

        public void PlayStageEndTransition()
        {
            StartCoroutine(StageEndRoutine());
        }

        private IEnumerator StageStartRoutine(Action onComplete)
        {
            using (PresentationControl.Busy())
            {
                yield return StartCoroutine(SequentialMoveOut(stageEndObjects, _stageEndOriginalPos));
                yield return StartCoroutine(SequentialMoveIn(inProgressObjects, _inProgressOriginalPos));
            }
            onComplete?.Invoke();
        }

        private IEnumerator StageEndRoutine()
        {
            using (PresentationControl.Busy())
            {
                yield return StartCoroutine(SequentialMoveOut(inProgressObjects, _inProgressOriginalPos));
                yield return StartCoroutine(SequentialMoveIn(stageEndObjects, _stageEndOriginalPos));
            }
        }

        private IEnumerator SequentialMoveOut(GameObject[] objects, Vector3[] originalPositions)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].transform.DOLocalMove(
                    originalPositions[i] + Vector3.up * hiddenYOffset,
                    moveDuration).SetEase(Ease.OutQuint);

                if (i < objects.Length - 1)
                    yield return new WaitForSeconds(delayBetween);
            }
            yield return new WaitForSeconds(moveDuration);
        }

        private IEnumerator SequentialMoveIn(GameObject[] objects, Vector3[] originalPositions)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].transform.localPosition = originalPositions[i] + Vector3.up * hiddenYOffset;
                objects[i].transform.DOLocalMove(originalPositions[i], moveDuration)
                    .SetEase(Ease.OutQuint);

                if (i < objects.Length - 1)
                    yield return new WaitForSeconds(delayBetween);
            }
            yield return new WaitForSeconds(moveDuration);
        }
    }
}

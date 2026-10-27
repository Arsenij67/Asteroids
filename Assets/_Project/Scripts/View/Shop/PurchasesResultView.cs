using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Asteroid.Generation;

namespace Asteroid.UI
{
    public class PurchasesResultView : MonoBehaviour
    {
        private const float PUNCH_DURATION = 1;
        private const int COUNT_PUNCHES = 2;
        private const float DEVIATION = 0;
        private const float ANGLE_ROTATION = 360;
        private const float TIME_DISAPPEAR = 0.5f;

        public event Action OnPlayerClickApply;

        [SerializeField] private TMP_Text _textProductTitle;
        [SerializeField] private Button _buttonApply;
        [SerializeField] private Image _imageProduct;

        private IResourceLoader _resourceLoader;

        private void OnDestroy()
        {
            _buttonApply.onClick.RemoveListener(CloseWindow);
        }

        public void Initialize(IResourceLoader resourceLoader)
        {
            _buttonApply.onClick.AddListener(CloseWindow);
            _resourceLoader = resourceLoader;
        }

        public UniTask OpenWindow(string textForProduct, Sprite imageForProduct)
        {
            _imageProduct.sprite = imageForProduct;
            _textProductTitle.text = textForProduct;
            Sequence openAnimationAsync = DOTween.Sequence()
                .Append(transform.DOPunchScale(Vector2.one, PUNCH_DURATION, COUNT_PUNCHES, DEVIATION))
                .Join(transform.DORotate(new Vector3(0,0,ANGLE_ROTATION), TIME_DISAPPEAR,RotateMode.FastBeyond360));
            return openAnimationAsync.AsyncWaitForCompletion().AsUniTask();
        }

        private void CloseWindow()
        {
            Sequence closeAnimationAsync = DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, TIME_DISAPPEAR)
                .OnComplete(DeleteWindow));

            closeAnimationAsync.Play();
        }

        private void DeleteWindow()
        {
            _resourceLoader.UnloadResource(gameObject.name); 
        }
    }
}
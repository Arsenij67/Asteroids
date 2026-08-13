using Asteroid.Database;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroid.UI
{
    public class SaveModeUI : MonoBehaviour
    {
        public event Func<SaveChoice,UniTask> OnButtonApplyPressed;
        public event Action OnButtonClosePressed;

        public int NumberActiveItem => _tmpSaveModeDrop.value;
        public bool ChoiceIsMade => _choiceIsMade;

        [SerializeField] private TMP_Dropdown _tmpSaveModeDrop;
        [SerializeField] private Button _applyButton;

        private bool _choiceIsMade = false;
        private SaveChoice _selectedChoice = SaveChoice.NoChoice;

        private void OnDestroy()
        {
            _applyButton.onClick.RemoveListener(ButtonApplyEventHandler);
            _tmpSaveModeDrop.onValueChanged.RemoveListener(SelectFromList);
        }

        public void Initialize()
        {
            _tmpSaveModeDrop.onValueChanged.AddListener(SelectFromList);
            _applyButton.onClick.AddListener(ButtonApplyEventHandler);
        }

        public void CloseWindow()
        {
            Destroy(gameObject);
        }

        private void ButtonApplyEventHandler()
        {
            OnButtonApplyPressed?.Invoke(_selectedChoice);
            OnButtonClosePressed?.Invoke();
        }

        private void SelectFromList(int numberItem)
        {
            _choiceIsMade = (SaveChoice) numberItem != SaveChoice.NoChoice;
           _selectedChoice =(SaveChoice)numberItem;
        }
    }
}

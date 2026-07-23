using Asteroid.Database;
using Asteroid.Generation;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Asteroid.UI
{
    public class SaveModeUI : MonoBehaviour
    {
        public event UnityAction<SaveChoice> OnActiveItemChanged;
        public event UnityAction OnButtonClosePressed;

        public int NumberActiveItem => _tmpSaveModeDrop.value;
        public bool ChoiceIsMade => _choiceIsMade;

        [SerializeField] private TMP_Dropdown _tmpSaveModeDrop;
        [SerializeField] private Button _closeButton;

        private bool _choiceIsMade=false;

        private void OnDestroy()
        {
            _tmpSaveModeDrop.onValueChanged.RemoveListener(SelectFromList);
            _closeButton.onClick.RemoveListener(ButtonCloseEventHandler);

        }

        public void Initialize()
        {
            _tmpSaveModeDrop.onValueChanged.AddListener(SelectFromList);
            _closeButton.onClick.AddListener(ButtonCloseEventHandler);
        }

        public void CloseWindow()
        {
            Destroy(gameObject);
        }

        private void ButtonCloseEventHandler()
        {
            OnButtonClosePressed?.Invoke();
        }

        private void SelectFromList(int numberItem)
        {
            _choiceIsMade = (SaveChoice) numberItem != SaveChoice.NoChoice;
            OnActiveItemChanged?.Invoke((SaveChoice)numberItem);
        }
    }
}

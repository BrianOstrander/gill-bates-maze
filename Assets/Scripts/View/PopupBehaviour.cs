using System;
using UnityEngine;
using UnityEngine.UI;

namespace GillBates.View
{
    public class PopupBehaviour : MonoBehaviour
    {
        [SerializeField]
        Text titleLabel;
        
        [SerializeField]
        Text descriptionLabel;

        Action onClickOkay;
        
        public void Open(
            string title,
            string description,
            Action onClickOkay
        )
        {
            gameObject.SetActive(true);
            
            titleLabel.text = title ?? "Alert";
            descriptionLabel.text = description ?? string.Empty;

            this.onClickOkay = onClickOkay;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void OnClickOkay()
        {
            Close();
            
            try
            {
                onClickOkay?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
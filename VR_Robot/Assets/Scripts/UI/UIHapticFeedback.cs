using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UI
{
    public class UIHapticFeedback : MonoBehaviour
    {
        private XRControllerWithRumble controller;
        // https://forum.unity.com/threads/haptic-feedback-for-ui.1420253/
        // onPointerEnter 

        public XRUIInputModule inputModule;
        //=> EventSystem.current.currentInputModule as XRUIInputModule;

        public void OnUIPointerEnter(BaseEventData data)
        {
            Debug.Log("on pointer being called");
            // feedback
            if (data is not PointerEventData eventData) return;
            NearFarInteractor interactor = inputModule.GetInteractor(eventData.pointerId) as NearFarInteractor;
            if (!interactor)
            {
                return;
            }

            interactor.SendHapticImpulse(.5f, .5f);
        }

        public void OnUIPointerExit(BaseEventData data)
        {
            // small feedback
            //if (xr != null)
            //{
            //    xr.SendImpulse(.5f, .5f);

            // }
        }
    }
}
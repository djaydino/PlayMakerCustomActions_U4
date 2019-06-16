// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Created By DJAYDINO http://www.jinxtergames.com/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GameObject)]

    [Tooltip("Check if a gameObject is active.This lets you know if a gameObject is active in the game. That is the case if its GameObject.activeSelf property is enabled, as well as that of all it's parents.")]
    public class IsActiveMulti : FsmStateAction
    {


        [RequiredField]
        [CompoundArray("Count", "Game Object", "Active")]
        [Tooltip("The GameObjects to check activate state.")]
        public FsmGameObject[] gameObject;
        [UIHint(UIHint.Variable)]
        [Tooltip("The active state of this gameObject. It uses activeInHierarchy, not activeSelf. So it will return true if this gameobject is active in the game.")]
        public FsmBool[] isActive;

        [Tooltip("Repeat this action every frame. Useful if Activate changes over time.")]
        public bool everyFrame;

        [ActionSection("Results")]

        [UIHint(UIHint.Variable)]
        [Tooltip("Returns True if all are active")]
        public FsmBool allActive;

        [Tooltip("Returns True if all are inactive")]
        [UIHint(UIHint.Variable)]
        public FsmBool allInActive;



        [Tooltip("Returns True if a mixed result was met (not all active but also not all inacive)")]
        [UIHint(UIHint.Variable)]
        public FsmBool mixedResult;

        [ActionSection("Events")]

        public FsmEvent isAllActiveEvent;
        public FsmEvent isNotAllActiveEvent;
        public FsmEvent isAllInactiveEvent;
        public FsmEvent isNotAllInactiveEvent;
        public FsmEvent isMixedResult;


        public override void Reset()
        {
            gameObject = null;
            isActive = null;
            isAllActiveEvent = null;
            isNotAllActiveEvent = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoIsActiveGameObject();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoIsActiveGameObject();
        }

        void DoIsActiveGameObject()
        {
            bool allAreActive = true;
            bool allAreInactive = true;

            for (int i = 0; i < gameObject.Length; i++)
            {
                var go = gameObject[i].Value;

                if (go == null)
                {
                    return;
                }

                if (go.activeInHierarchy) allAreInactive = false;
                else allAreActive = false;
            }
            if (allAreActive)
            {
                allActive.Value = true;
                Fsm.Event(isAllActiveEvent);

            }
            else
            {
                allActive.Value = false;
                Fsm.Event(isNotAllActiveEvent);
            }

               

            if (allAreInactive)
            {
                allInActive.Value = true;
                Fsm.Event(isAllInactiveEvent);
            }
            else
            {
                allInActive.Value = false;
                Fsm.Event(isNotAllInactiveEvent);
            }

            if (!allInActive.Value && !allInActive.Value)
            {
                mixedResult.Value = true;
                Fsm.Event(isMixedResult);
            }
            else mixedResult.Value = false;
        }
    }
}

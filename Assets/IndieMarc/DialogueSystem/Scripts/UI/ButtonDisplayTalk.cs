using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.DialogueSystem
{

    public class ButtonDisplayTalk : MonoBehaviour
    {

        public Sprite controller_icon;
        public Sprite keyboard_icon;
        public DialogueActor actor;
        public Vector3 offset;

        private SpriteRenderer render;
        private float timer = 0f;

        void Start()
        {
            render = GetComponent<SpriteRenderer>();
            transform.position = new Vector3(-9999f, -9999f, -9999f);
        }

        void Update()
        {
            if (NarrativeManager.Get().IsPaused())
                return;
            
            transform.position = new Vector3(-9999f, -9999f, -9999f);

            if (actor == null)
                return;

            GameObject target = NarrativeManager.Get().GetNearestPlayer(actor.transform.position, actor.trigger_radius);

            if (target != null)
            {
                float dist = (target.transform.position - actor.transform.position).magnitude;
                if (dist < actor.trigger_radius && actor.CanBeTalked(target))
                {
                    transform.position = actor.transform.position + offset;

                    Camera cam = IndieMarc.TopDown.FollowCamera.GetCamera();
                    if(cam != null)
                        transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f);

                    if (ButtonIconUI.IsUsingController())
                        render.sprite = controller_icon;
                    else
                        render.sprite = keyboard_icon;
                }
            }
        }
    }

}
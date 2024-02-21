using UnityEngine;

namespace KeyboardDefense.UI
{
    public class PromptBinding : MonoBehaviour
    {
        private UIFollowObjectInWorld _uiFollowObjectInWorld;

        public void SetObjToFollow(GameObject obj)
        {
            _uiFollowObjectInWorld.SetObjectToFollow(obj);
        }
        
        public void SetFollowOffset(Vector2 offset)
        {
            _uiFollowObjectInWorld.SetFollowOffset(offset);
        }

        public void Reset()
        {
        }

        private void Awake()
        {
            _uiFollowObjectInWorld = GetComponent<UIFollowObjectInWorld>();
        }
    }

}
namespace Gameplay
{
    using UnityEngine;

    public interface IViewportObject
    {
        void HandleViewportPosition();
        (Vector3, Vector3) GetMinMaxSize();
    }
}
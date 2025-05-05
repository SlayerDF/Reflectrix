namespace Reflectrix.PlayerController
{
    public interface IPlayerInputHandler
    {
        /// <summary>
        /// Initialize player input handler.
        /// </summary>
        void Initialize(PlayerController playerController, PlayerControls controls);
    }
}
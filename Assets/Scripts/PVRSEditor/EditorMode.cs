namespace Assets.Scripts.PVRSEditor
{
    /// <summary>
    /// This interface allows for the creation of several flexible editor modes. Editor modes should implement it.
    /// </summary>
    interface IEditorMode
    {
        void OnGUI();

        /// <summary>
        /// This method will be called by the EditorManager each time it updates itself.
        /// </summary>
        void Update();

        /// <summary>
        /// This method will be called by the EditorManager each time the mode is switched to.
        /// </summary>
        void Enable();

        /// <summary>
        /// This method will be called each time the mode is closed to switch to a different one.
        /// </summary>
        void Disable();

        /// <summary>
        /// This method is used to determine the name of the editor mode.
        /// </summary>
        /// <returns>The name of the mode as a String. Will be shown in UI.</returns>
        string ToString();
    }
}

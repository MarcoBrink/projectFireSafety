using System;

namespace Assets.Scripts.Saving
{
    /// <summary>
    /// The data to save to a PVRS file, consists of a scenario and an auth string.
    /// </summary>
    [Serializable]
    public class PVRS
    {
        /// <summary>
        /// An Auth string to prevent fake files from loading.
        /// </summary>
        private string Auth;

        /// <summary>
        /// Saved Scenario Data.
        /// </summary>
        private VRScenario.ScenarioData Scenario;

        /// <summary>
        /// Constructor for PVRS file data.
        /// </summary>
        /// <param name="auth">The hashed auth string.</param>
        /// <param name="scenario">The scenario to save.</param>
        public PVRS(string auth, VRScenario.Scenario scenario)
        {
            Auth = auth;
            Scenario = new VRScenario.ScenarioData(scenario);
        }

        /// <summary>
        /// Validates the file based on auth string and scenario data.
        /// </summary>
        /// <param name="auth">The auth string.</param>
        /// <returns></returns>
        public int IsValid(string auth)
        {
            // Default return for valid files.
            int returnVal = 0;

            if (Auth == null && Scenario == null)
            {
                // File is incomplete.
                returnVal = -1;
            }
            else if (Auth != auth)
            {
                // Auth string incorrect.
                returnVal = -2;
            }
            else if (Scenario.GetScenario() == null)
            {
                // Scenario data is invalid.
                returnVal = -3;
            }

            return returnVal;
        }

        /// <summary>
        /// Get the scenario saved in the PVRS file.
        /// </summary>
        /// <param name="auth">The Auth string.</param>
        /// <returns>The saved scenario, null if the scenario is invalid.</returns>
        public VRScenario.Scenario GetScenario(string auth)
        {
            VRScenario.Scenario scenario = null;

            // Only return the scenario if the PVRS file is valid.
            if (IsValid(auth) == 0)
            {
                scenario = Scenario.GetScenario();
            }

            return scenario;
        }
    }
}

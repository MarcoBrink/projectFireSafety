using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using Assets.Scripts;
using Assets.Scripts.Saving;
using Assets.Scripts.VRScenario;

public class CustomNetworkManager : NetworkManager
{
    public GameObject DefaultEnvironment;

    /// <summary>
    /// Should this instance be a server?
    /// </summary>
	public bool ShouldBeServer;

    /// <summary>
    /// The VR player Prefab.
    /// </summary>
	public GameObject vrPlayerPrefab;

    /// <summary>
    /// The amount of connected players.
    /// </summary>
	private int playerCount = 0;

    /// <summary>
    /// Executes when a player is added to the server.
    /// </summary>
    /// <param name="conn">The outgoing connection.</param>
    /// <param name="playerControllerId">The ID of the player controller.</param>
    /// <param name="extraMessageReader">A reader that reads any incoming messages from the player.</param>
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		SpawnMessage message = new SpawnMessage ();
		message.Deserialize (extraMessageReader);

		bool isVrPlayer = message.isVrPlayer;

		Transform spawnPoint = this.startPositions [playerCount];

		GameObject newPlayer;
        Debug.Log(isVrPlayer);
		if (isVrPlayer)
        {
			newPlayer = (GameObject)Instantiate (this.vrPlayerPrefab, spawnPoint.position, spawnPoint.rotation);
		}
        else
        {
			newPlayer = (GameObject)Instantiate (this.playerPrefab, spawnPoint.position, spawnPoint.rotation);
		}
		NetworkServer.AddPlayerForConnection (conn, newPlayer, playerControllerId);
		playerCount++;
	}

    /// <summary>
    /// Executes when a player is removed from the server.
    /// </summary>
    /// <param name="conn">The outgoing connection.</param>
    /// <param name="player">The removed player.</param>
	public override void OnServerRemovePlayer (NetworkConnection conn, UnityEngine.Networking.PlayerController player)
	{
		base.OnServerRemovePlayer (conn, player);
		playerCount--;
	}

    /// <summary>
    /// Initialisation code for the Network Manager.
    /// </summary>
	void Start ()
    {
        LoadSettings();
        LoadPrefabs();

        Debug.Log ("Starting Network");
		if (ShouldBeServer)
		{
			StartHost ();
		}
		else
		{
			StartClient ();
		}
        LoadScenario();
    }

    /// <summary>
    /// Executes when a client connects.
    /// </summary>
    /// <param name="conn">The incoming connection.</param>
	public override void OnClientConnect (NetworkConnection conn)
	{
        SpawnMessage extraMessage = new SpawnMessage()
        {
            isVrPlayer = UnityEngine.VR.VRSettings.enabled
        };

		ClientScene.AddPlayer (client.connection, 0, extraMessage);
	}

    /// <summary>
    /// Load all Network-supported Prefabs from the resources.
    /// </summary>
    private void LoadPrefabs()
    {
        PrefabManager.Prefab[] prefabs = PrefabManager.Prefabs;
        foreach (PrefabManager.Prefab prefab in prefabs)
        {
            GameObject item = prefab.Item;

            if (item.GetComponent<NetworkIdentity>() != null)
            {
                this.spawnPrefabs.Add(item);
            }
        }
    }

    /// <summary>
    /// Load the Manager settings from the settings file.
    /// </summary>
    private void LoadSettings()
    {
        GameObject simulationSettings = GameObject.Find("SimulatieData(Clone)");
        SimulationDataScript sds = simulationSettings.GetComponent<SimulationDataScript>();
        ShouldBeServer = sds.IsHost;
        if(!ShouldBeServer)
        {
            networkAddress = sds.IP;
        }

        string settingsPath = Application.dataPath + "/settings.cfg";
        if (File.Exists(settingsPath))
        {
            StreamReader textReader = new StreamReader(settingsPath, System.Text.Encoding.ASCII);
            ShouldBeServer = textReader.ReadLine() == "Server";
            networkAddress = textReader.ReadLine();
            textReader.Close();
        }
    }

    /// <summary>
    /// Load the current scenario.
    /// </summary>
    private void LoadScenario()
    {
        GameObject simulationSettings = GameObject.Find("SimulatieData(Clone)");
        SimulationDataScript sds = simulationSettings.GetComponent<SimulationDataScript>();

        Scenario scenario;
        if (SaveLoad.LoadSavedScenario(Application.persistentDataPath + "/Scenarios/" + sds.ScenarioFile, out scenario))
        if (scenario == null)
        {
            string[] saved;
            bool scenarioLoaded = false;

            if (SaveLoad.GetSavedScenarios(SaveLoad.SaveDirectory, out saved))
            {
                if (SaveLoad.LoadSavedScenario(SaveLoad.GetFilePath(saved[0]), out scenario))
                {
                    scenarioLoaded = true;
                }
            }

            if (!scenarioLoaded)
            {
                Scenario newScenario = new Scenario("Default", DefaultEnvironment);
                scenario = newScenario;
            }
        }

        Object.Instantiate<GameObject>(scenario.Environment);

        foreach (ScenarioObject item in scenario.Objects)
        {
            GameObject spawned = Instantiate<GameObject>(PrefabManager.GetPrefab(item.PrefabName), item.Position, item.Rotation);
            spawned.tag = "Scenario Object";
            NetworkServer.Spawn(spawned);
        }
    }
}



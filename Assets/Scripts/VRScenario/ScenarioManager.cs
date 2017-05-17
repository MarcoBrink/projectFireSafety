using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour {

 //   public Scenario CurrentScenario;
 //   public GameObject TestCubePrefab;

 //   List<GameObject> Cubes = new List<GameObject>();

	//// Initialization for ScenarioManager;
	//void Start ()
 //   {
 //       List<Scenario> savedScenarios = SaveLoad.GetSavedScenarios();

 //       if (savedScenarios.Count != 0)
 //       {
 //           CurrentScenario = savedScenarios[0];
 //       }
 //       else
 //       {
 //           CurrentScenario = new Scenario();
 //           CurrentScenario.Cubes.Add(new Cube(-2, 2, 1));
 //           CurrentScenario.Cubes.Add(new Cube(-3, 2, 1));
 //       }

 //       foreach (Cube cube in CurrentScenario.Cubes)
 //       {
 //           GameObject testCube = Instantiate(TestCubePrefab);
 //           testCube.transform.position = new Vector3(cube.X, cube.Y, cube.Z);
 //           Cubes.Add(testCube);
 //           Debug.Log(Cubes.Count);
 //       }
	//}

 //   void Update()
 //   {
 //       if (Input.anyKeyDown)
 //       {
 //           if (Input.GetKey(KeyCode.T))
 //           {
 //               Debug.Log(Cubes.Count + " | " + CurrentScenario.Cubes.Count);
 //               for (int i = 0; i < Cubes.Count; i++)
 //               {
 //                   GameObject sceneCube = Cubes[i];
 //                   Vector3 cubePos = sceneCube.transform.position;
 //                   Cube cube = new Cube(cubePos.x, cubePos.y, cubePos.z);
 //                   CurrentScenario.Cubes[i] = cube;
 //               }

 //               SaveLoad.SaveScenario("Test", CurrentScenario);
 //           }
 //       }
 //   }
}

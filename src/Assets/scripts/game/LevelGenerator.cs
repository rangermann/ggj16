using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

  #region Inspector

  [SerializeField]
  private List<GameObject> sectionPrefabs;

  #endregion

  private List<GameObject> currentSections { get; set; }
  private float sectionStart;
  private float sectionEnd;
  private bool isFirstSection;

  public void Awake() {
    currentSections = new List<GameObject>();
  }

  public void StartGenerating(int seed) {
    Random.seed = seed;
    sectionStart = 0.0f;
    sectionEnd = 0.0f;
  }

  public void CleanUp() {
    currentSections.ForEach (delegate(GameObject obj) {
      RemoveSection(obj);
    });

    currentSections.Clear();
    sectionStart = 0.0f;
    sectionEnd = 0.0f;
    isFirstSection = true;
  }

  public void Update() {
    CameraBounds cameraPosition = GameController.Instance.GetCameraXBounds(); 

    float cameraStart = cameraPosition.start;
    float cameraEnd = cameraPosition.end; 
    float sectionPadding = GameController.Instance.GameConfig.levelGeneratorSectionPadding;

    // Only holds true for first call to Update()
    if (isFirstSection) {
      sectionStart = cameraStart;
      sectionEnd = cameraStart;
    }

    while (sectionEnd < cameraEnd + sectionPadding) {
      GameConfig gameConfig = GameController.Instance.GameConfig;

      int nextLevelIdx = Mathf.RoundToInt(Random.value * (sectionPrefabs.Count - 1));
      GameObject nextPrefab = sectionPrefabs[nextLevelIdx];
      GameObject section = GameObject.Instantiate (nextPrefab);
      float sectionWidth = GetSectionWidth(section);
      section.transform.position = new Vector2(sectionEnd, 0.0f);
      currentSections.Add (section);

      sectionEnd += sectionWidth;

      List<GameObject> spawnList = new List<GameObject> ();

      if (!isFirstSection) {
        for (int i = 0; i < gameConfig.levelGeneratorTrapsPerSection; i++) {
          spawnList.Add (GameController.Instance.PrefabTrapObstacle);
        }
        
        for (int i = 0; i < gameConfig.levelGeneratorPriestsPerSection; i++) {
          spawnList.Add (GameController.Instance.PrefabPriestObstacle);
        }
      }

      for (int i = 0; i < gameConfig.levelGeneratorFollowersPerSection; i++) {
        spawnList.Add (GameController.Instance.PrefabFollowerObstacle);
      }
      spawnList.OrderBy( a => { return Random.value; });
      SpawnObstaclesInSection (section, spawnList);
    }

    bool sectionRemoved;
    do {
      sectionRemoved = false;

      if(currentSections.Count == 0) {
        break;
      }

      GameObject firstSection = currentSections[0];
      float sectionWidth = GetSectionWidth(firstSection);

      if (sectionStart + sectionWidth < cameraStart - sectionPadding) {
        RemoveSection(firstSection);

        sectionStart += sectionWidth;
        sectionRemoved = true;
      }
    } while (sectionRemoved);

    isFirstSection = false;
  }

  private void RemoveSection (GameObject section) {
    GameObject.Destroy (section);
    currentSections.Remove (section);
  }

  private float GetSectionWidth(GameObject section){
    var boxCollider = section.GetComponent<BoxCollider2D>();
    float sectionWidth = boxCollider.bounds.size.x;

    // TODO: cache section width of current section to avoid GetComponent() call every frame
    return sectionWidth;
  }

  private void SpawnObstaclesInSection(GameObject section, List<GameObject> types) {
    List<GameObject> spawnLocations = ExtractSpawnLocations (section);
    spawnLocations.OrderBy(a => Random.value);
    int i;
    for (i = 0; i < Mathf.Min (spawnLocations.Count, types.Count); i++) {
      GameObject obstacle = GameObject.Instantiate(types[i]);

      obstacle.transform.parent = section.transform;
      obstacle.transform.position = spawnLocations[i].transform.position;
    }
  }

  private List<GameObject> ExtractSpawnLocations(GameObject section) {
    List<GameObject> spawnLocations = new List<GameObject>();

    Transform[] objects = section.GetComponentsInChildren<Transform>();

    foreach(Transform t in objects) {
      GameObject obj = t.gameObject;

      if (obj.tag == "SpawnPoint") {
        spawnLocations.Add (obj);
      }
    }

    return spawnLocations;
  }
}

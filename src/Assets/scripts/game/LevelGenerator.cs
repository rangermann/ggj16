using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

  #region Inspector

  [SerializeField]
  private List<GameObject> sectionPrefabs;

  #endregion

  private List<GameObject> currentSections { get; set; }
  private float sectionStart;
  private float sectionEnd;

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
  }

  public void Update() {
    Camera mainCamera = Camera.main;
    Vector3 cameraPosition = GameController.Instance.TransformLevelCamera.position;

    float xDist = mainCamera.aspect * mainCamera.orthographicSize; 

    float cameraStart = cameraPosition.x - xDist;
    float cameraEnd = cameraPosition.x + xDist; 

    // Only holds true for first call to Update()
    if (sectionStart > cameraStart) {
      sectionStart = cameraStart;
      sectionEnd = cameraStart;
    }

    while (sectionEnd < cameraEnd) {
      int nextLevelIdx = Mathf.RoundToInt(Random.value * (sectionPrefabs.Count - 1));
      GameObject nextPrefab = sectionPrefabs[nextLevelIdx];
      GameObject sectionPrefab = GameObject.Instantiate (nextPrefab);
      float sectionWidth = GetSectionWidth(sectionPrefab);

      currentSections.Add (sectionPrefab);

      sectionPrefab.transform.Translate(new Vector2(sectionEnd + sectionWidth/2, 0.0f));
      sectionEnd += sectionWidth;
    }

    bool sectionRemoved;
    do {
      sectionRemoved = false;

      if(currentSections.Count == 0) {
        break;
      }

      GameObject firstSection = currentSections[0];
      float sectionWidth = GetSectionWidth(firstSection);

      if (sectionStart + sectionWidth < cameraStart) {
        RemoveSection(firstSection);

        sectionStart += sectionWidth;
        sectionRemoved = true;
      }
    } while (sectionRemoved);
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
}

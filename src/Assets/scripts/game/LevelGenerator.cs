using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

  #region Inspector

  [SerializeField]
  private List<GameObject> sectionPrefabs;

  #endregion

  private List<GameObject> CurrentSections { get; set; }

  public void Awake() {
    CurrentSections = new List<GameObject>();
  }

  public void StartGenerating(int seed) {
    Random.seed = seed;

  }

  public void CleanUp() {
    // TODO delete all level elements


    CurrentSections.Clear();
  }

  public void Update() {
    // add/remove elements

    // add:
    // GameObject.Instantiate(randomPrefab);

    // remove:
    //GameObject.Destroy(section);
  }
}

using UnityEngine;

public abstract class AbstractPanelDeclaration : MonoBehaviour {

  public void Init() {
    OnInit();
  }

  public void Enter(object onEnterParams) {
    OnEnter(onEnterParams);
  }


  public void Leave() {
    OnLeave();
  }


  public void Update() {
    OnUpdate();
  }

  protected virtual void OnInit() { }
  protected virtual void OnEnter(object onEnterParams) { }
  protected virtual void OnLeave() { }
  protected virtual void OnUpdate() { }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract	class AbstractState {

  public string StateName { get; private set; }

  public AbstractPanelDeclaration Panel { get; private set; }

  public AbstractState(string stateName, AbstractPanelDeclaration panelDeclaration = null) {
    StateName = stateName;
    Panel = panelDeclaration;

    Initialize();
  }

  public void Initialize() {
    OnInitialize();

    if (Panel != null) {
      Panel.Init();
      Panel.gameObject.SetActive(false);
    }
  }

  public void Enter(object onEnterParams = null) {
    if (Panel != null) {
      Panel.gameObject.SetActive(true);
      Panel.Enter(onEnterParams);
    }

    OnEnter(onEnterParams);
  }

  public void Leave() {
    if (Panel != null) {
      Panel.gameObject.SetActive(false);
      Panel.Leave();
    }
    OnLeave();
  }

  public void Update() {
    OnUpdate();
  }

  public void OnGUI() {
    OnGUICustom();
  }

  protected abstract void OnInitialize();
  protected abstract void OnEnter(object onEnterParams = null);
  protected abstract void OnLeave();
  protected abstract void OnUpdate();
  protected virtual void OnGUICustom() { }

  public T GetPanel<T>() where T : AbstractPanelDeclaration {
    return Panel as T;
  }
}

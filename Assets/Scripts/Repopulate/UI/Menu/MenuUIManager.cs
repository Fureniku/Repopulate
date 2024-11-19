using UnityEngine;

public class MenuUIManager : MonoBehaviour {

    [SerializeField] private PopupMenuBase _defaultMenu;
    [SerializeField] private PopupMenuBase _activeMenu;

    private void OnEnable() {
        SetActiveMenu(_defaultMenu);
    }

    public void CloseAllMenus() {
        Destroy(_activeMenu);
        _defaultMenu.gameObject.SetActive(true);
    }

    public void SetActiveMenu(PopupMenuBase menu) {
        if (menu == _defaultMenu) {
            CloseAllMenus();
            return;
        }
        
        _defaultMenu.gameObject.SetActive(false);
        _activeMenu = menu;
    }
}

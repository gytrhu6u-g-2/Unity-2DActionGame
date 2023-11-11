using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField, Header("HPアイコン")]
    private GameObject _playerIcon;

    private Player _player;
    private int _beforeHP;

    // Start is called before the first frame update
    void Start()
    {
        // Hierarchyからコンポーネントの取得
        _player = FindObjectOfType<Player>();
        //playerクラスの関数呼び出し
        _beforeHP = _player.GetHP();

        _CreateHPIcon();
    }

    // 体力アイコン生成
    private void _CreateHPIcon()
    {
        for (int i = 0; i < _player.GetHP(); i++)
        {
            // オブジェクト生成
            GameObject _playerHPObj = Instantiate(_playerIcon);
            // 親オブジェクトにする
            _playerHPObj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _ShowHPIcon();
    }

    // HP表示
    private void _ShowHPIcon()
    {
        if (_beforeHP == _player.GetHP()) return;

        // Image型の変数
        // 子オブジェクトを探して当てはまる全てのコンポーネントを配列で取得
        Image[] icons = transform.GetComponentsInChildren<Image>();

        // 体力分の表示
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(i < _player.GetHP());
        }
        _beforeHP = _player.GetHP();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField, Header("ゲームオーバー")]
    private GameObject _gameOverUI;

    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        // playerのコンポーネント取得
        _player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _ShowGameOverUI();
    }

    // ゲームオーバー表示
    private void _ShowGameOverUI()
    {
        // playreが存在する時
        if (_player != null) return;

        // 存在しない時
        _gameOverUI.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField, Header("振動する時間")]
    private float _shakeTime;
    [SerializeField, Header("振動の大きさ")]
    private float _shakeMagnitude;

    private Player _player;
    private float _shakeCount;
    private int _currentPlayerHP;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _currentPlayerHP = _player.GetHP();
    }

    // Update is called once per frame
    void Update()
    {
        _ShakeCheck();
    }

    // 画面を揺らすか確認
    private void _ShakeCheck()
    {
        // 現在のHPと取得したHPが異なった場合
        if (_currentPlayerHP != _player.GetHP())
        {
            // 現在のHPを更新
            _currentPlayerHP = _player.GetHP();
            _shakeCount = 0.0f;
            StartCoroutine(_Shake());
        }
    }

    // 画面を揺らす
    IEnumerator _Shake()
    {
        // カメラの現在地取得
        Vector3 initPos = transform.position;

        // 振動する時間が_shakeCountよりも小さい間ループ
        while (_shakeCount < _shakeTime)
        {
            // カメラのx座標にランダムの数値をいれる
            float x = initPos.x + Random.Range(-_shakeMagnitude, _shakeMagnitude);
            float y = initPos.y + Random.Range(-_shakeMagnitude, _shakeMagnitude);
            transform.position = new Vector3(x, y, initPos.z);

            // _shakeCountに時間をたす
            _shakeCount += Time.deltaTime;

            // 処理を一度中断
            yield return null;
        }

        // カメラを元の位置に戻す
        transform.position = initPos;
    }
}

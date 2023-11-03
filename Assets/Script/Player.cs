using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 変数の意味を視覚的にわかるようにする
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2Dのコンポーネント取得
        _rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
    }

    // 移動処理
    private void _Move()
    {
        // X軸の移動
        _rigid.velocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.velocity.y);
    }

    // 移動した入力情報を代入する
    public void _OnMove(InputAction.CallbackContext context)
    {
        // vector2型に変換
        _inputDirection = context.ReadValue<Vector2>();
    }
}

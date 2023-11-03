using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // ヘッダーの設定
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float _jumpSpeed;
    [SerializeField, Header("体力")]
    private int _hp;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;
    private bool _bJump;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2Dのコンポーネント取得
        _rigid = GetComponent<Rigidbody2D>();
        _bJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        Debug.Log(_hp);
    }

    // 移動処理
    private void _Move()
    {
        // X軸の移動
        _rigid.velocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.velocity.y);
    }

    // 移動した入力情報を代入する
    public void OnMove(InputAction.CallbackContext context)
    {
        // vector2型に変換
        _inputDirection = context.ReadValue<Vector2>();
    }

    // 当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面
        if (collision.gameObject.tag == "Floor")
        {
            _bJump = false;
        }

        // 敵
        if (collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject);
        }
    }

    // 敵を倒す処理
    private void _HitEnemy(GameObject enemy)
    {
        // 縦の大きさの半分
        float halfScaleY = transform.lossyScale.y / 2.0f;
        // 敵の半分の大きさ
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;

        // 敵を踏める条件
        if (transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy);
        }
        // ダメージを受ける
        else
        {
            enemy.GetComponent<Enemy>().PlayerDamage(this);
        }
    }

    // Jumpアクション
    public void OnJump(InputAction.CallbackContext context)
    {
        // jumpキーではない場合
        if (!context.performed || _bJump) return;

        // 上方向に力を加える
        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);  // ForceMode2D.Impulseは初速が早く、徐々に減速させる
        _bJump = true;
    }

    // ダメージ処理
    public void Damage(int damage)
    {
        // 0と比べて大きい方の数値を_hpに代入
        _hp = Mathf.Max(_hp - damage, 0);
    }
}

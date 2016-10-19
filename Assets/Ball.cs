using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public GameObject platform;


	enum BALL_STATE {
		LEFT,
		RIGHT
	}

	GameObject _lastPlatform;
	Rigidbody _rigidBody;
	BALL_STATE _ballState = BALL_STATE.RIGHT;
	bool _isStarted = false;
	float _speed = 3;

	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody>();
		_lastPlatform = Instantiate (platform, platform.transform) as GameObject;
		for (int i = 0; i < 50; i++) {
			SpawnPlatform ();
		}
	}

	void SpawnPlatform() {
		Vector3 position = _lastPlatform.transform.position;
		int random = Random.Range (0, 2);
		if (random == 0) {
			position.x += 1;
		} else {
			position.z += 1;
		}
		_lastPlatform = Instantiate (_lastPlatform, position, Quaternion.identity) as GameObject;
	}

	// Update is called once per frame
	void Update () {
		onTap ();
	}

	void onTap() {
		if (Input.GetMouseButtonDown (0)) {
			if (!_isStarted) {
				_isStarted = true;
				_rigidBody.velocity = new Vector3 (_speed, 0, 0);
			}
			switch (_ballState) {
			case BALL_STATE.LEFT:
				_ballState = BALL_STATE.RIGHT;
				_rigidBody.velocity = new Vector3 (_speed, 0, 0);
				break;
			case BALL_STATE.RIGHT:
				_ballState = BALL_STATE.LEFT;
				_rigidBody.velocity = new Vector3 (0, 0, _speed);
				break;
			}
		}
	}

	void OnCollisionExit (Collision other)
	{
		// 这里只有平台与小球碰撞，所以退出平台后直接生成新平台
		SpawnPlatform ();
		// 新建游戏对象保存当前平台
		GameObject platform = other.gameObject;
		// 关掉isKinematic属性让平台下落
		platform.GetComponent<Rigidbody> ().isKinematic = false;
		// 下落1秒后销毁平台
		InvokeDestroyPlatfrom (platform);
	}

	void InvokeDestroyPlatfrom (GameObject platform)
	{
		// 调用协程销毁平台
		StartCoroutine (DestroyPlatform (platform));
	}

	IEnumerator DestroyPlatform (GameObject platform)
	{
		// 等待1秒
		yield return new WaitForSeconds (1.0f);
		// 销毁平台
		Destroy (platform);
	}
}

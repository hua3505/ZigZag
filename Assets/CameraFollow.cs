using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public bool shouldRotate = false;

	// 跟随目标
	public Transform target;
	// 相机距xz平面的距离
	public float distance = 10.0f;
	// 相机在目标上方的高度
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	float wantedRotationAngle;
	float wantedHeight;
	float currentRotationAngle;
	float currentHeight;
	Quaternion currentRotation;

	void LateUpdate() {
		if (target) {
			// 计算当前旋转角度
			wantedRotationAngle = target.eulerAngles.y;
			wantedHeight = target.position.y + height;
			currentRotationAngle = transform.eulerAngles.y;
			currentHeight = transform.position.y;
			currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
			// 计算当前高度
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
			// 将角度转换为欧拉角
			currentRotation =Quaternion.Euler(9, currentRotationAngle, 0);
			// 设置相机至xz平面的距离
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;
			// 设置相机高度
			transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
			if (shouldRotate) {
				transform.LookAt (target);
			}
		}
	}
}

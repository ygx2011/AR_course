/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class FiringCannon : MonoBehaviour {

    public Transform Turret;
    public Transform FireTarget;
    public GameObject FireParticle;
    public GameObject Projectile;
    public float TurretAngle = 45.0f;
    public float TurnSpeed = 35.0f;
    public float TargetWaitTime = 1.0f;
    public float AfterShotWaitTime = 3.0f;

    public float CameraShakeDistance = 20.0f;

    public float[] FireTargetAngles;

    private Quaternion initialRotation;

    private int nextFireTargetAngle = 0;

    private MarioVerySmartCamera smartCamera;

	void Start () {
        Turret.rotation = Quaternion.AngleAxis(TurretAngle, Turret.forward) * Turret.rotation;

        initialRotation = transform.rotation;

        smartCamera = Camera.main.GetComponent<MarioVerySmartCamera>();

        StartCoroutine(MoveToAndFire(FireTargetAngles[nextFireTargetAngle]));
	}

    IEnumerator MoveToAndFire(float angle)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(angle, transform.up) * initialRotation;

        GetComponent<AudioSource>().Play();

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
            yield return 0;
        }

        GetComponent<AudioSource>().Stop();

        yield return new WaitForSeconds(TargetWaitTime);

        GetComponent<Animation>().Play();

        float distance = Vector3.Distance(smartCamera.transform.position, FireTarget.position);

        if (distance < CameraShakeDistance)
        {
            float magnitude = Mathf.Lerp(0.4f, 0, Mathf.InverseLerp(5.0f, CameraShakeDistance, distance));

            smartCamera.Shake(magnitude, 20.0f, 0.5f);
        }

        Instantiate(FireParticle, FireTarget.position, Turret.rotation);

        Instantiate(Projectile, FireTarget.position, Quaternion.LookRotation(Turret.up));

        yield return new WaitForSeconds(AfterShotWaitTime + GetComponent<Animation>().clip.length);

        nextFireTargetAngle = (nextFireTargetAngle + 1) % FireTargetAngles.Length;

        StartCoroutine(MoveToAndFire(FireTargetAngles[nextFireTargetAngle]));
    }
}
